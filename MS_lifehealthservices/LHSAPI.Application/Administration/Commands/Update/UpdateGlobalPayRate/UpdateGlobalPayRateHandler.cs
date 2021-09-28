
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

namespace LHSAPI.Application.Administration.Commands.Update.UpdateGlobalPayRate
{
    public class UpdateGlobalPayRateHandler : IRequestHandler<UpdateGlobalPayRateCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public UpdateGlobalPayRateHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(UpdateGlobalPayRateCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistUser = _context.GlobalPayRate.FirstOrDefault(x => x.Id == request.Id & x.IsActive == true);
                    if (ExistUser != null)
                    {
                       
                        ExistUser.Holiday12To6AM = request.Holiday12To6AM;
                        ExistUser.Holiday6To12AM = request.Holiday6To12AM;
                        ExistUser.MonToFri12To6AM = request.MonToFri12To6AM;
                        ExistUser.MonToFri6To12AM = request.MonToFri6To12AM;
                        ExistUser.Sat12To6AM = request.Sat12To6AM;
                        ExistUser.Sat6To12AM = request.Sat6To12AM;
                        ExistUser.Sun6To12AM = request.Sun6To12AM;
                        ExistUser.Sun12To6AM = request.Sun12To6AM;
                        ExistUser.ActiveNightsAndSleep = request.ActiveNightsAndSleep;
                        ExistUser.HouseCleaning = request.HouseCleaning;
                        ExistUser.TransportPetrol = request.TransportPetrol;
                        ExistUser.Level = request.Level;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.GlobalPayRate.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);

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
