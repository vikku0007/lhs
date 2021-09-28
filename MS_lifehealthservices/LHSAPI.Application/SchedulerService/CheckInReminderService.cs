using EasyNetQ;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LHSAPI.Persistence.DbContext;
using System.Linq;
using LHSAPI.Application.Interface;
using LHSAPI.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace LHSAPI.Application.SchedulerService
{
    public class CheckInReminderService : BackgroundService
    {
        private readonly ILogger<CheckInReminderService> _logger;
        IServiceScopeFactory _iServiceScopeFactory;
        private readonly IMessageService _iMessageService;
        private static IConfiguration _configuration;

        public CheckInReminderService(ILogger<CheckInReminderService> logger, IServiceScopeFactory iServiceScopeFactory, IMessageService iMessageService, IConfiguration configuraion)
        {
            _logger = logger;
            _iServiceScopeFactory = iServiceScopeFactory;
            _iMessageService = iMessageService;
            _configuration = configuraion;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" GracePeriod background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"GracePeriod task doing background work.");

                // This eShopOnContainers method is querying a database table
                // and publishing events into the Event Bus (RabbitMQ / ServiceBus)
                SendCheckInNotification();
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogDebug($"GracePeriod background task is stopping.");
        }

        private void SendCheckInNotification()
        {
            using (var scope = _iServiceScopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<LHSDbContext>();
                var shifts = (from shift in _dbContext.ShiftInfo
                              join empShift in _dbContext.EmployeeShiftInfo on shift.Id equals empShift.ShiftId
                              join empPrimaryInfo in _dbContext.EmployeePrimaryInfo on empShift.EmployeeId equals empPrimaryInfo.Id
                              join notification in _dbContext.ShiftEmailNotification on shift.Id equals notification.ShiftId into gj
                              from subpet in gj.DefaultIfEmpty()
                              where shift.IsActive == true && shift.IsDeleted == false && empShift.IsActive == true && empShift.IsDeleted == false
                              && empShift.IsAccepted == true
                               && (subpet == null || subpet != null && subpet.IsCheckInEmailSent == false)
                              && shift.StartUtcDate.Date == DateTime.UtcNow.Date
                              select new
                              {
                                  shift,
                                  empShift,
                                  empPrimaryInfo
                              }).ToList();
                int hour = Convert.ToInt16(_configuration.GetSection("UTC:Hour").Value);
                int min = Convert.ToInt32(_configuration.GetSection("UTC:Minutes").Value);
                var shiftWith15MinToStart = shifts.Where(x => x.shift.StartUtcDate.AddHours(hour).AddMinutes(min).Subtract(DateTime.UtcNow).TotalMinutes <= 15).ToList();
                foreach (var toDoDhift in shiftWith15MinToStart)
                {
                    var emailNotifications = (from notif in _dbContext.ShiftEmailNotification where notif.ShiftId == toDoDhift.shift.Id select new { notif }).FirstOrDefault();
                    if (emailNotifications == null)
                    {
                        // if (!emailNotifications.notif.IsEmailSent)
                        //{
                        ShiftEmailNotification notification = new ShiftEmailNotification();
                        notification.IsCheckInEmailSent = true;
                        notification.ShiftId = toDoDhift.shift.Id;
                        notification.CreatedById = 1;
                        notification.CreatedDate = DateTime.Now;
                        _dbContext.ShiftEmailNotification.Add(notification);
                        _dbContext.SaveChanges();

                        if (!string.IsNullOrEmpty(toDoDhift.empPrimaryInfo.EmailId))
                        {
                            string emailBody = _iMessageService.GetCheckInEmailTemplate();
                            string Subject = "Shift to be starting soon";
                            string Message = "Your Shift will be starting soon";
                            DateTime startDate = toDoDhift.shift.StartDate;
                            emailBody = emailBody.Replace("{Subject}", Subject);
                            emailBody = emailBody.Replace("{Message}", Message);
                            emailBody = emailBody.Replace("{ShiftStartDate}", startDate.ToString("dd/MM/yyyy"));
                            emailBody = emailBody.Replace("{ShiftStartTime}", startDate.Date.Add(toDoDhift.shift.StartTime).ToString(@"hh\:mm tt"));
                            _iMessageService.SendingEmails(toDoDhift.empPrimaryInfo.EmailId, Subject, emailBody);
                        }
                        // }
                    }
                }
            }

        }
    }
}
