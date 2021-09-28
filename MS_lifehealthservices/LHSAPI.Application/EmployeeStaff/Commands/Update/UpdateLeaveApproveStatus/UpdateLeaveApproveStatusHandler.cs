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

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateLeaveApproveStatus
{
    public class UpdateLeaveApproveStatusHandler : IRequestHandler<UpdateLeaveApproveStatusCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly INotification _Notification;
        private readonly ISessionService _ISessionService;
        private readonly IConfiguration _configuration;
        public UpdateLeaveApproveStatusHandler(LHSDbContext context, ISessionService ISessionService, INotification notification, IConfiguration configuration)
        {
            _context = context;
            _Notification = notification;
            _ISessionService = ISessionService;
            _configuration = configuration;
        }
        public async Task<ApiResponse> Handle(UpdateLeaveApproveStatusCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeLeaveInfo.FirstOrDefault(x => x.Id == request.Id && x.EmployeeId == request.EmployeeId && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        ExistEmp.IsRejected = false;
                        ExistEmp.IsApproved = true;
                        ExistEmp.ApprovedById = await _ISessionService.GetUserId();
                        ExistEmp.ApprovedDate = DateTime.Now; 
                        _context.EmployeeLeaveInfo.Update(ExistEmp);
                        _context.SaveChanges();
                        await _Notification.SaveNotification(new LHSAPI.Domain.Entities.Notification
                        {
                            Email = true,
                            EventName = "Employee Leave Accepted",
                            EmployeeId = request.EmpId,
                            Description = "Employee Leave Accepted",
                            IsAdminAlert = true,
                            IsEmailSent = true
                        }, Services.NotiFicationSaveMode.InBoth);
                        response.Update(ExistEmp);
                       
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
