using LHSAPI.Application.Interface;
using LHSAPI.Persistence.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LHSAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace LHSAPI.Application.SchedulerService
{
    public class CheckOutReminderService : BackgroundService
    {
        private readonly ILogger<CheckInReminderService> _logger;
        IServiceScopeFactory _iServiceScopeFactory;
        private readonly IMessageService _iMessageService;
        private IConfiguration _configuration;

        public CheckOutReminderService(ILogger<CheckInReminderService> logger, IServiceScopeFactory iServiceScopeFactory, IMessageService iMessageService, IConfiguration configuration)
        {
            _logger = logger;
            _iServiceScopeFactory = iServiceScopeFactory;
            _iMessageService = iMessageService;
            _configuration = configuration;
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
                SendCheckOutNotification();
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogDebug($"GracePeriod background task is stopping.");
        }

        private void SendCheckOutNotification()
        {
            try
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
                                  //  && (subpet == null || subpet != null && subpet.IsCheckOutEmailSent == false)
                                  && shift.EndUtcDate.Date == DateTime.UtcNow.Date
                                  select new
                                  {
                                      shift,
                                      empShift,
                                      empPrimaryInfo
                                  }).ToList();
                    int hour = Convert.ToInt16(_configuration.GetSection("UTC:Hour").Value);
                    int min = Convert.ToInt32(_configuration.GetSection("UTC:Minutes").Value);
                    var shiftWith15MinToStart = shifts.Where(x => x.shift.EndUtcDate.AddHours(hour).AddMinutes(min).Subtract(DateTime.UtcNow).TotalMinutes <= 15).ToList();
                    foreach (var toDoDhift in shiftWith15MinToStart)
                    {
                        var emailNotifications = _dbContext.ShiftEmailNotification.Where(x => x.ShiftId == toDoDhift.shift.Id).FirstOrDefault();
                        if (emailNotifications != null)
                        {
                            if (!emailNotifications.IsCheckOutEmailSent)
                            {
                                emailNotifications.IsCheckOutEmailSent = true;
                                _dbContext.ShiftEmailNotification.Update(emailNotifications);
                                _dbContext.SaveChanges();
                                // Send Email                            
                                if (!string.IsNullOrEmpty(toDoDhift.empPrimaryInfo.EmailId))
                                {
                                    string emailBody = _iMessageService.GetCheckInEmailTemplate();
                                    string Subject = "Notification - Shift Finishing";
                                    string Message = "Your shift will be ending soon";
                                    DateTime endDate = toDoDhift.shift.EndDate;
                                    emailBody = emailBody.Replace("{Subject}", Subject);
                                    emailBody = emailBody.Replace("{Message}", Message);
                                    emailBody = emailBody.Replace("{ShiftStartDate}", endDate.ToString("dd/MM/yyyy"));
                                    emailBody = emailBody.Replace("{ShiftStartTime}", endDate.Date.Add(toDoDhift.shift.EndTime).ToString(@"hh\:mm tt"));
                                    _iMessageService.SendingEmails(toDoDhift.empPrimaryInfo.EmailId, Subject, emailBody);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
