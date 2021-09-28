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
using INotification = LHSAPI.Application.Interface.INotification;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateShiftRejectStatus
{
    public class UpdateShiftRejectStatusCommandHandler : IRequestHandler<UpdateShiftRejectStatusCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly INotification _Notification;
        private readonly IMessageService _IMessageService;
        private readonly IConfiguration _configuration;
        public UpdateShiftRejectStatusCommandHandler(LHSDbContext context, IMessageService IMessageService, INotification notification, IConfiguration configuration)
        {
            _context = context;
            _Notification = notification;
            _IMessageService = IMessageService;
            _configuration = configuration;
        }
        public async Task<ApiResponse> Handle(UpdateShiftRejectStatusCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeShiftInfo.FirstOrDefault(x => x.ShiftId == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        ExistEmp.IsAccepted = false;
                        ExistEmp.IsRejected = true;
                        ExistEmp.Remark = request.Remark;
                        ExistEmp.StatusId = _context.StandardCode.Where(x => x.CodeData == "ShiftStatus" && x.CodeDescription == "Cancelled").FirstOrDefault().ID;
                        _context.EmployeeShiftInfo.Update(ExistEmp);
                        _context.SaveChanges();
                        string EmailId = _context.EmployeePrimaryInfo.Where(x => x.Id == ExistEmp.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmailId).FirstOrDefault();
                        string UserName = _context.EmployeePrimaryInfo.Where(x => x.Id == ExistEmp.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();
                        var _ShiftInfo = _context.ShiftInfo.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);

                        await _Notification.SaveNotification(new LHSAPI.Domain.Entities.Notification
                        {
                            Email = true,
                            EventName = "Employee Rejected Shift",
                            EmployeeId = ExistEmp.EmployeeId,
                            Description = "Employee Rejected Shift",
                            IsAdminAlert = true,
                            IsEmailSent = true
                        }, Services.NotiFicationSaveMode.InBoth);
                        response.Update(ExistEmp);
                        if (!string.IsNullOrEmpty(EmailId))
                        {
                            string emailBody = _IMessageService.GetShiftTemplate();
                            string Message = "Shift has been Rejected for " + _ShiftInfo.Description;
                            string Date = " Start Date :" + _ShiftInfo.StartDate.ToShortDateString() + "and End Date :" + _ShiftInfo.EndDate.ToShortDateString();
                            string Time = " Start Time: " + _ShiftInfo.StartDate.Add(_ShiftInfo.StartTime).ToString(@"hh\:mm tt") + "- End Time: " + _ShiftInfo.StartDate.Add(_ShiftInfo.EndTime).ToString(@"hh\:mm tt");
                            string Subject = "Shift Rejected";
                            emailBody = emailBody.Replace("{Message}", Message);
                            emailBody = emailBody.Replace("{Subject}", Subject);
                            emailBody = emailBody.Replace("{UserName}", UserName);
                            emailBody = emailBody.Replace("{Date}", Date);
                            emailBody = emailBody.Replace("{Time}", Time);
                            _IMessageService.SendingEmailsAsynWithCC(EmailId, Subject, emailBody, _configuration.GetSection("SMTP:CC_RegistrationEmail").Value, null);
                        }
                    }
                    else
                    {
                        response.NotFound();

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
