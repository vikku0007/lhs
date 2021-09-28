using LHSAPI.Application.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LHSAPI.Persistence.DbContext;

namespace LHSAPI.Application.SchedulerService
{
    public class EmployeeDocumentReminderService : BackgroundService
    {
        private readonly ILogger<EmployeeDocumentReminderService> _logger;
        IServiceScopeFactory _iServiceScopeFactory;
        private readonly IMessageService _iMessageService;
        //private readonly INotification _Notification;
        public EmployeeDocumentReminderService(ILogger<EmployeeDocumentReminderService> logger, IServiceScopeFactory iServiceScopeFactory, IMessageService iMessageService
            )
        {
            _logger = logger;
            _iServiceScopeFactory = iServiceScopeFactory;
            _iMessageService = iMessageService;
            //_Notification = Notification;
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
                SendEmployeeDocumentExpiryNotificationAsync();
                SendMandatoryComplianceDocumentAsync();
                TimeSpan timeSpan = new TimeSpan(24, 0, 0);
                await Task.Delay(timeSpan, stoppingToken);
            }

            _logger.LogDebug($"GracePeriod background task is stopping.");
        }

        private void SendEmployeeDocumentExpiryNotificationAsync()
        {
            try
            {
                using (var scope = _iServiceScopeFactory.CreateScope())
                {
                    var _dbContext = scope.ServiceProvider.GetRequiredService<LHSDbContext>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotification>();
                    var licenseList = (from empPrimary in _dbContext.EmployeePrimaryInfo
                                       join license in _dbContext.EmployeeDrivingLicenseInfo
                                       on empPrimary.Id equals license.EmployeeId
                                       where empPrimary.IsActive == true && empPrimary.IsDeleted == false
                                       && license.IsActive == true && license.IsDeleted == false
                                       select new
                                       {
                                           empPrimary,
                                           license
                                       }).ToList();
                    var licenseToExpire = licenseList.Where(x => x.license.LicenseExpiryDate.Value.Subtract(DateTime.Now).TotalDays <= 10);
                    foreach (var license in licenseToExpire)
                    {
                        var notification = _dbContext.Notification.Where(x => x.EventName.ToLower() == "driving license expiration" &&
                          x.IsActive == true && x.IsDeleted == false && x.EmployeeId == license.empPrimary.Id).FirstOrDefault();
                        if (notification == null)
                        {
                            notificationService.SaveNotification(new LHSAPI.Domain.Entities.Notification
                            {
                                Email = true,
                                EventName = "Driving License Expiration",
                                EmployeeId = license.empPrimary.Id,
                                Description = "Driving license expiring soon",
                                IsAdminAlert = true,
                                IsEmailSent = true
                            }, Services.NotiFicationSaveMode.InBoth);

                            if (!string.IsNullOrEmpty(license.empPrimary.EmailId))
                            {
                                string emailBody = _iMessageService.GetEmployeeDocumentTemplate();
                                string Subject = "License expiring soon";
                                string Message = "Hey! your license is expiring soon.";
                                Nullable<DateTime> expiryDate = license.license.LicenseExpiryDate;
                                emailBody = emailBody.Replace("{Subject}", Subject);
                                emailBody = emailBody.Replace("{Message}", Message);
                                emailBody = emailBody.Replace("{LicenseExpiryDate}", expiryDate.Value.ToString("dd/MM/yyyy"));
                                _iMessageService.SendingEmails(license.empPrimary.EmailId, Subject, emailBody);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SendMandatoryComplianceDocumentAsync()
        {
            try
            {
                using (var scope = _iServiceScopeFactory.CreateScope())
                {
                    var _dbContext = scope.ServiceProvider.GetRequiredService<LHSDbContext>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotification>();
                    var documentList = (from empPrimary in _dbContext.EmployeePrimaryInfo
                                        join document in _dbContext.EmployeeCompliancesDetails
                                        on empPrimary.Id equals document.EmployeeId
                                        join standard in _dbContext.StandardCode on document.DocumentName equals standard.ID
                                        where empPrimary.IsActive == true && empPrimary.IsDeleted == false
                                        && document.IsActive == true && document.IsDeleted == false
                                        && standard.IsActive == true && standard.IsDeleted == false
                                        && standard.CodeData == Common.Enums.ResponseEnums.StandardCode.DocumentName.ToString()
                                        select new
                                        {
                                            empPrimary,
                                            document
                                        }).ToList();
                    var documentToExpire = documentList.Where(x => x.document.ExpiryDate.Value.Subtract(DateTime.Now).TotalDays <= 10);
                    foreach (var license in documentToExpire)
                    {
                        var notification = _dbContext.Notification.Where(x => x.EventName.ToLower() == "compliance document expiration" &&
                          x.IsActive == true && x.IsDeleted == false && x.EmployeeId == license.empPrimary.Id).FirstOrDefault();
                        if (notification == null)
                        {
                            notificationService.SaveNotification(new LHSAPI.Domain.Entities.Notification
                            {
                                Email = true,
                                EventName = "Compliance Document Expiration",
                                EmployeeId = license.empPrimary.Id,
                                Description = "Compliance Document expiring soon",
                                IsAdminAlert = true,
                                IsEmailSent = true
                            }, Services.NotiFicationSaveMode.InBoth);

                            if (!string.IsNullOrEmpty(license.empPrimary.EmailId))
                            {
                                string emailBody = _iMessageService.GetEmployeeDocumentTemplate();
                                string Subject = "Compliance document expiring soon";
                                string Message = "Hey! your license is expiring soon.";
                                Nullable<DateTime> expiryDate = license.document.ExpiryDate;
                                emailBody = emailBody.Replace("{Subject}", Subject);
                                emailBody = emailBody.Replace("{Message}", Message);
                                emailBody = emailBody.Replace("{LicenseExpiryDate}", expiryDate.Value.ToString("dd/MM/yyyy"));
                                _iMessageService.SendingEmails(license.empPrimary.EmailId, Subject, emailBody);
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
