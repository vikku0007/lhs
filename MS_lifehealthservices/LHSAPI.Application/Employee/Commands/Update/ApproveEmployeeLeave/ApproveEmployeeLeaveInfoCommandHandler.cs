using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LHSAPI.Application.Employee.Commands.Update.ApproveEmployeeLeave
{
    public class ApproveEmployeeLeaveInfoCommandHandler : IRequestHandler<ApproveEmployeeLeaveInfoCommand, ApiResponse>, IRequestHandler<RejectEmployeeLeaveInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public ApproveEmployeeLeaveInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(ApproveEmployeeLeaveInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeLeaveInfo.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {

                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                        ExistEmp.ApprovedDate = DateTime.Now;
                        ExistEmp.IsApproved = true;
                        ExistEmp.ApprovedById = await _ISessionService.GetUserId();
                        _context.EmployeeLeaveInfo.Update(ExistEmp);
                        _context.SaveChanges();
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

        public async Task<ApiResponse> Handle(RejectEmployeeLeaveInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeLeaveInfo.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                        ExistEmp.RejectedDate = DateTime.Now;
                        ExistEmp.IsRejected = true;
                        ExistEmp.RejectedById = await _ISessionService.GetUserId();
                        ExistEmp.RejectRemark = request.Remark;
                        _context.EmployeeLeaveInfo.Update(ExistEmp);
                        _context.SaveChanges();
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
