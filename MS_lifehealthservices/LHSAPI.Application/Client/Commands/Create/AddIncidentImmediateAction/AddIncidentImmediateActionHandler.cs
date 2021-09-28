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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentImmediateAction
{
    public class AddIncidentImmediateActionHandler : IRequestHandler<AddIncidentImmediateActionCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddIncidentImmediateActionHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentImmediateActionCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
               
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                      var  ExistUser = _context.IncidentImmediateAction.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                    
                    if (ExistUser == null)
                    {
                        IncidentImmediateAction user = new IncidentImmediateAction();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.IsPoliceInformed = request.IsPoliceInformed;
                        user.OfficerName = request.OfficerName;
                        user.PoliceStation = request.PoliceStation;
                        user.PoliceNo = request.PoliceNo;
                        user.ProviderPosition = request.ProviderPosition;
                        user.IsFamilyAware = request.IsFamilyAware;
                        user.ContacttoFamily = request.ContacttoFamily;
                        user.IsUnder18 = request.IsUnder18;
                        user.ContactChildProtection = request.ContactChildProtection;
                        user.DisabilityPerson = request.DisabilityPerson;
                        user.SubjectWorkerAllegation = request.SubjectWorkerAllegation;
                        user.SubjectDisabilityPerson = request.SubjectDisabilityPerson;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.IncidentImmediateAction.AddAsync(user);
                        _context.SaveChanges();
                        response.Success(user);

                    }
                    else
                    {
                        ExistUser.IsPoliceInformed = request.IsPoliceInformed;
                        ExistUser.OfficerName = request.OfficerName;
                        ExistUser.PoliceStation = request.PoliceStation;
                        ExistUser.PoliceNo = request.PoliceNo;
                        ExistUser.ProviderPosition = request.ProviderPosition;
                        ExistUser.IsFamilyAware = request.IsFamilyAware;
                        ExistUser.ContacttoFamily = request.ContacttoFamily;
                        ExistUser.IsUnder18 = request.IsUnder18;
                        ExistUser.ContactChildProtection = request.ContactChildProtection;
                        ExistUser.DisabilityPerson = request.DisabilityPerson;
                        ExistUser.SubjectWorkerAllegation = request.SubjectWorkerAllegation;
                        ExistUser.SubjectDisabilityPerson = request.SubjectDisabilityPerson;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.IncidentImmediateAction.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);
                    }

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
