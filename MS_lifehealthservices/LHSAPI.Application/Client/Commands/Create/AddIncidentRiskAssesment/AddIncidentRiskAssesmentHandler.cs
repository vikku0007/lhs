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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentRiskAssesment
{
    public class AddIncidentRiskAssesmentHandler : IRequestHandler<AddIncidentRiskAssesmentCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddIncidentRiskAssesmentHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentRiskAssesmentCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
              
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                      var  ExistUser = _context.IncidentRiskAssesment.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                 
                    if (ExistUser == null)
                    {
                        IncidentRiskAssesment user = new IncidentRiskAssesment();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.IsRiskAssesment = request.IsRiskAssesment;
                        user.RiskAssesmentDate = request.RiskAssesmentDate;
                        user.RiskDetails = request.RiskDetails;
                        user.NoRiskAssesmentInfo = request.NoRiskAssesmentInfo;
                        user.InProgressRisk = request.InProgressRisk;
                        user.TobeFinished = request.TobeFinished;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.IncidentRiskAssesment.AddAsync(user);
                        _context.SaveChanges();
                        response.Success(user);

                    }
                    else
                    {
                        ExistUser.IsRiskAssesment = request.IsRiskAssesment;
                        ExistUser.RiskAssesmentDate = request.RiskAssesmentDate;
                        ExistUser.RiskDetails = request.RiskDetails;
                        ExistUser.NoRiskAssesmentInfo = request.NoRiskAssesmentInfo;
                        ExistUser.InProgressRisk = request.InProgressRisk;
                        ExistUser.TobeFinished = request.TobeFinished;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.IncidentRiskAssesment.Update(ExistUser);
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
