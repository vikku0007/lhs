using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Application.Employee.Models;
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeLeaveDetails
{
    public class AddEmployeeLeaveDetailsCommandHandler : IRequestHandler<AddEmployeeLeaveDetailsCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddEmployeeLeaveDetailsCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeLeaveDetailsCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeLeaveInfo.FirstOrDefault(x => x.EmployeeId == request.EmployeeId  && x.DateFrom == request.DateFrom && x.DateTo == request.DateTo && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        EmployeeLeaveInfo user = new EmployeeLeaveInfo();
                        user.EmployeeId = request.EmployeeId;
                        user.LeaveType = request.LeaveType;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.DateFrom = request.DateFrom;
                        user.IsActive = true;
                        user.DateTo = request.DateTo;
                        user.ReasonOfLeave = request.ReasonOfLeave;
                        await _context.EmployeeLeaveInfo.AddAsync(user);
                        _context.SaveChanges();
                        EmployeeLeaveModel model = new Models.EmployeeLeaveModel();
                        model.Id = user.Id;
                        model.EmployeeId = user.EmployeeId;
                        model.LeaveType = user.LeaveType;
                        model.DateFrom = user.DateFrom;
                        model.DateTo = user.DateTo;
                        model.ReasonOfLeave = user.ReasonOfLeave;
                        model.LeaveTypeName = _context.StandardCode.Where(x => x.ID == user.LeaveType).Select(x => x.CodeDescription).FirstOrDefault();
                        response.Success(user);

                    }
                    else
                    {
                        response.AlreadyExist();

                    }
                }
                else
                {


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
