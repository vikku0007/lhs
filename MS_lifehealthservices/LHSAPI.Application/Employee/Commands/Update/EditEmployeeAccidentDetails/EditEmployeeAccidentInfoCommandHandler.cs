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

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeAccidentDetails
{
    public class EditEmployeeAccidentInfoCommandHandler : IRequestHandler<EditEmployeeAccidentInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public EditEmployeeAccidentInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(EditEmployeeAccidentInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeAccidentInfo.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {

                        ExistEmp.AccidentDate = request.AccidentDate;
                        ExistEmp.EventType = request.EventType;
                        ExistEmp.IsActive = true;
                        ExistEmp.LocationId = request.LocationId;
                        ExistEmp.RaisedBy = request.RaisedBy;
                        ExistEmp.ReportedTo = request.ReportedTo;
                        ExistEmp.BriefDescription = request.BriefDescription;
                        ExistEmp.DetailedDescription = request.DetailedDescription;
                        ExistEmp.OtherLocation = request.OtherLocation;
                        ExistEmp.LocationType = request.LocationType;
                        ExistEmp.ActionTaken = request.ActionTaken;
                        ExistEmp.IncidentTime = TimeSpan.Parse(request.IncidentTime);
                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                        _context.EmployeeAccidentInfo.Update(ExistEmp);
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
