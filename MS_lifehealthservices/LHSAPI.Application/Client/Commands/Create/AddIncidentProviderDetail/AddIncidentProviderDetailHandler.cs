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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentProviderDetail
{
    public class AddClientAccidentInfoCommandHandler : IRequestHandler<AddIncidentProviderDetailCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddClientAccidentInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentProviderDetailCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
             
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                       var ExistUser = _context.ClientAccidentProviderInfo.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                  
                    if (ExistUser == null)
                    {
                        ClientAccidentProviderInfo user = new ClientAccidentProviderInfo();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.ReportCompletedBy = request.ReportCompletedBy;
                        user.ProviderName = request.ProviderName;
                        user.ProviderregistrationId = request.ProviderregistrationId;
                        user.ProviderABN = request.ProviderABN;
                        user.OutletName = request.OutletName;
                        user.Registrationgroup = request.Registrationgroup;
                        user.State = request.State;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.ClientAccidentProviderInfo.AddAsync(user);
                        _context.SaveChanges();
                        response.Success(user);

                    }
                    else
                    {
                        ExistUser.EmployeeId = request.EmployeeId;
                        ExistUser.ReportCompletedBy = request.ReportCompletedBy;
                        ExistUser.ProviderName = request.ProviderName;
                        ExistUser.ProviderregistrationId = request.ProviderregistrationId;
                        ExistUser.ProviderABN = request.ProviderABN;
                        ExistUser.OutletName = request.OutletName;
                        ExistUser.Registrationgroup = request.Registrationgroup;
                        ExistUser.State = request.State;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientAccidentProviderInfo.Update(ExistUser);
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
