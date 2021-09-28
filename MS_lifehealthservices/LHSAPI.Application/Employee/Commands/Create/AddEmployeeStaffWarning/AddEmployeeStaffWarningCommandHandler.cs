using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;
using INotification = LHSAPI.Application.Interface.INotification;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeStaffWarning
{
    public class AddEmployeeStaffWarningCommandHandler : IRequestHandler<AddEmployeeStaffWarningCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private readonly IMessageService _IMessageService;
        private readonly IConfiguration _configuration;
        private readonly INotification _Notification;
        public AddEmployeeStaffWarningCommandHandler(LHSDbContext context, ISessionService ISessionService, INotification Notification, IMessageService IMessageService, IConfiguration configuration)
        {
            _context = context;
            _ISessionService = ISessionService;
            _IMessageService = IMessageService;
            _configuration = configuration;
            _Notification = Notification;
        }

        public async Task<ApiResponse> Handle(AddEmployeeStaffWarningCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeStaffWarning.FirstOrDefault(x => x.EmployeeId == request.EmployeeId && x.OffensesType == request.OffensesType && x.WarningType == request.WarningType && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        EmployeeStaffWarning user = new EmployeeStaffWarning();
                        user.EmployeeId = request.EmployeeId;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        user.WarningType = request.WarningType;
                        user.OffensesType = request.OffensesType;
                        user.Description = request.Description;
                        user.ImprovementPlan = request.ImprovementPlan;
                        user.OtherOffenses = request.OtherOffenses;
                        await _context.EmployeeStaffWarning.AddAsync(user);
                        _context.SaveChanges();
                        await _Notification.SaveNotification(new LHSAPI.Domain.Entities.Notification
                        {
                            Email = true,
                            EventName = "Employee warning added",
                            EmployeeId = request.EmployeeId,
                            Description = "Employee warning added",
                            IsAdminAlert = true,
                            IsEmailSent = true
                        }, Services.NotiFicationSaveMode.InBoth);
                        response.Success(user);
                        string EmailId = _context.EmployeePrimaryInfo.Where(x => x.Id == request.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmailId).FirstOrDefault();
                        string UserName = _context.EmployeePrimaryInfo.Where(x => x.Id == request.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();

                        if (!string.IsNullOrEmpty(EmailId))
                        {
                            string Warning = _context.StandardCode.Where(x => x.ID == user.WarningType).Select(x => x.CodeDescription).FirstOrDefault();
                            string OffenseTypeName = _context.StandardCode.Where(x => x.ID == user.OffensesType).Select(x => x.CodeDescription).FirstOrDefault();
                            string emailBody = _IMessageService.GetEmailTemplate();
                            string Message = "You received" + Warning + "for" + OffenseTypeName != "" ? OffenseTypeName : user.OtherOffenses;
                            string Subject = "Warning!";
                            emailBody = emailBody.Replace("{Message}", Message);
                            emailBody = emailBody.Replace("{Subject}", Subject);
                            emailBody = emailBody.Replace("{UserName}", UserName);
                            _IMessageService.SendingEmailsAsynWithCC(EmailId, Subject, emailBody, _configuration.GetSection("SMTP:CC_RegistrationEmail").Value, null);
                        }
                    }
                    else
                    {
                        response.AlreadyExist();

                    }
                }
                else
                {
                    response.ValidationError();

                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);

            }
            return response;

        }
    }
}
