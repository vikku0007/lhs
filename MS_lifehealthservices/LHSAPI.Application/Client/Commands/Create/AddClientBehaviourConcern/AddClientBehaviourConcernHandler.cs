
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientBehaviourConcern
{
    public class AddClientBehaviourConcernHandler : IRequestHandler<AddClientBehaviourConcernCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientBehaviourConcernHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientBehaviourConcernCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientSafetyBehavioursInfo.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientSafetyBehavioursInfo _Client = new ClientSafetyBehavioursInfo();
                        _Client.IsHistoryFalls = request.IsHistoryFalls;
                        _Client.HistoryFallsDetails = request.HistoryFallsDetails;
                        _Client.IsAbsconding = request.IsAbsconding;
                        _Client.AbscondingHistory = request.AbscondingHistory;
                        _Client.IsBSP = request.IsBSP;
                        _Client.BSPDetails = request.BSPDetails;
                        _Client.CreatedById = await _ISessionService.GetUserId();
                        _Client.CreatedDate = DateTime.Now;
                        _Client.IsActive = true;
                        _Client.IsDeleted = false;
                        _Client.ClientId = request.ClientId;
                        await _context.ClientSafetyBehavioursInfo.AddAsync(_Client);
                        _context.SaveChanges();
                        if (request.IsBehaviourConcern == true)
                        {
                            LHSAPI.Domain.Entities.ClientBehaviourofConcern _Clientconcern = new LHSAPI.Domain.Entities.ClientBehaviourofConcern();
                            _Clientconcern.IsBanging = request.IsBanging;
                            _Clientconcern.IsBehaviourConcern = request.IsBehaviourConcern;
                            _Clientconcern.IsBiting = request.IsBiting;
                            _Clientconcern.IsHeadButting = request.IsHeadButting;
                            _Clientconcern.IsHitting = request.IsHitting;
                            _Clientconcern.IsKicking = request.IsKicking;
                            _Clientconcern.IsObsessiveBehaviour = request.IsObsessiveBehaviour;
                            _Clientconcern.IsOther = request.IsOther;
                            _Clientconcern.IsPullingHair = request.IsPullingHair;
                            _Clientconcern.IsScreaming = request.IsScreaming;
                            _Clientconcern.IsSelfInjury = request.IsSelfInjury;
                            _Clientconcern.IsThrowing = request.IsThrowing;
                            _Clientconcern.IsPinching = request.IsPinching;
                            _Clientconcern.BehaviourConcernDetails = request.BehaviourConcernDetails;
                            _Clientconcern.CreatedById = await _ISessionService.GetUserId();
                            _Clientconcern.CreatedDate = DateTime.Now;
                            _Clientconcern.IsActive = true;
                            _Clientconcern.IsDeleted = false;
                            _Clientconcern.ClientId = request.ClientId;
                            await _context.ClientBehaviourofConcern.AddAsync(_Clientconcern);
                            _context.SaveChanges();
                        }
                        else
                        {

                        }
                        response.Success(_Client);

                    }
                    else
                    {
                        ExistUser.HistoryFallsDetails = request.HistoryFallsDetails;
                        ExistUser.IsHistoryFalls = request.IsHistoryFalls;
                        ExistUser.IsAbsconding = request.IsAbsconding;
                        ExistUser.AbscondingHistory = request.AbscondingHistory;
                        ExistUser.IsBSP = request.IsBSP;
                        ExistUser.BSPDetails = request.BSPDetails;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientSafetyBehavioursInfo.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);
                        var ExistUserinfo = _context.ClientBehaviourofConcern.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                        ExistUserinfo.IsBanging = request.IsBanging;
                        ExistUserinfo.IsBehaviourConcern = request.IsBehaviourConcern;
                        ExistUserinfo.IsBiting = request.IsBiting;
                        ExistUserinfo.IsHeadButting = request.IsHeadButting;
                        ExistUserinfo.IsHitting = request.IsHitting;
                        ExistUserinfo.IsKicking = request.IsKicking;
                        ExistUserinfo.IsObsessiveBehaviour = request.IsObsessiveBehaviour;
                        ExistUserinfo.IsOther = request.IsOther;
                        ExistUserinfo.IsPullingHair = request.IsPullingHair;
                        ExistUserinfo.IsScreaming = request.IsScreaming;
                        ExistUserinfo.IsSelfInjury = request.IsSelfInjury;
                        ExistUserinfo.IsThrowing = request.IsThrowing;
                        ExistUserinfo.IsPinching = request.IsPinching;
                        ExistUserinfo.BehaviourConcernDetails = request.BehaviourConcernDetails;
                        ExistUserinfo.UpdateById = await _ISessionService.GetUserId();
                        ExistUserinfo.UpdatedDate = DateTime.Now;
                        ExistUserinfo.IsActive = true;
                        _context.ClientBehaviourofConcern.Update(ExistUserinfo);
                        await _context.SaveChangesAsync();

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
