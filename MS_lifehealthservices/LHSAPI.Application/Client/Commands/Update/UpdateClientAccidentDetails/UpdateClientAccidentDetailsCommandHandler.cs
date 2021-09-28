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

namespace LHSAPI.Application.Client.Commands.Update.UpdateClientAccidentDetails
{
    public class UpdateClientAccidentDetailsCommandHandler : IRequestHandler<UpdateClientAccidentDetailsCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public UpdateClientAccidentDetailsCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(UpdateClientAccidentDetailsCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.ClientAccidentIncidentInfo.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {

                        ExistEmp.EmployeeId = request.EmployeeId;
                        ExistEmp.DepartmentId = request.DepartmentId;
                        ExistEmp.AccidentDate = request.AccidentDate;
                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                        ExistEmp.IncidentType = request.IncidentType;
                        ExistEmp.IsActive = true;
                        ExistEmp.LocationId = request.LocationId;
                        ExistEmp.ReportedBy = request.ReportedBy;
                        ExistEmp.FollowUp = request.FollowUp;
                        ExistEmp.PhoneNo = request.PhoneNo;
                        ExistEmp.PoliceNotified = request.PoliceNotified;
                        ExistEmp.IncidentCause = request.IncidentCause;
                        ExistEmp.LocationType = request.LocationType;
                        ExistEmp.OtherLocation = request.OtherLocation;
                        _context.ClientAccidentIncidentInfo.Update(ExistEmp);
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
