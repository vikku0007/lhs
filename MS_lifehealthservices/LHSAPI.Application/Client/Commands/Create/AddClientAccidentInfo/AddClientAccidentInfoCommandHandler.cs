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

namespace LHSAPI.Application.Client.Commands.Create.AddClientAccidentInfo
{
    public class AddClientAccidentInfoCommandHandler : IRequestHandler<AddClientAccidentInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddClientAccidentInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;

        }

        public async Task<ApiResponse> Handle(AddClientAccidentInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientAccidentIncidentInfo.FirstOrDefault(x => x.ClientId == request.ClientId && x.LocationType == request.LocationType && x.AccidentDate == request.AccidentDate && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientAccidentIncidentInfo user = new ClientAccidentIncidentInfo();
                        user.ClientId = request.ClientId;
                        user.EmployeeId = request.EmployeeId;
                        user.DepartmentId = request.DepartmentId;
                        user.AccidentDate = request.AccidentDate;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IncidentType = request.IncidentType;
                        user.IsActive = true;
                        user.LocationId = request.LocationId;
                        user.ReportedBy = request.ReportedBy;
                        user.FollowUp = request.FollowUp;
                        user.PhoneNo = request.PhoneNo;
                        user.PoliceNotified = request.PoliceNotified;
                        user.IncidentCause = request.IncidentCause;
                        user.LocationType = request.LocationType;
                        user.OtherLocation = request.OtherLocation;
                        await _context.ClientAccidentIncidentInfo.AddAsync(user);
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
