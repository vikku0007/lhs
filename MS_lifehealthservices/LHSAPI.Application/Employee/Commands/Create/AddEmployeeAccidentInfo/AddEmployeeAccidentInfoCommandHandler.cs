using LHSAPI.Application.Interface;
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAccidentInfo
{
    public class AddEmployeeAccidentInfoCommandHandler : IRequestHandler<AddEmployeeAccidentInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddEmployeeAccidentInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeAccidentInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeAccidentInfo.FirstOrDefault(x => x.EmployeeId == request.EmployeeId && x.LocationType == request.LocationType && x.AccidentDate==request.AccidentDate && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        EmployeeAccidentInfo user = new EmployeeAccidentInfo();
                        user.EmployeeId = request.EmployeeId;
                        user.AccidentDate = request.AccidentDate;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.EventType = request.EventType;
                        user.IsActive = true;
                        user.LocationId = request.LocationId;
                        user.RaisedBy = request.RaisedBy;
                        user.ReportedTo = request.ReportedTo;
                        user.BriefDescription = request.BriefDescription;
                        user.DetailedDescription = request.DetailedDescription;
                        user.LocationType = request.LocationType;
                        user.OtherLocation = request.OtherLocation;
                        user.ActionTaken = request.ActionTaken;
                        user.IncidentTime = TimeSpan.Parse(request.IncidentTime);
                        await _context.EmployeeAccidentInfo.AddAsync(user);
                        _context.SaveChanges();
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
