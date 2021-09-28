
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using LHSAPI.Application.Employee.Models;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Application.Client.Models;
using AutoMapper;

namespace LHSAPI.Application.Client.Queries.GetClientProfileDetails
{
    public class GetClientProfileDetailsHandler : IRequestHandler<GetClientProfileDetailsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientProfileDetailsHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetClientProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientDetails _clientDetails = new ClientDetails();

                var clientList = _dbContext.ClientPrimaryInfo.Where(x => x.IsDeleted == false && x.Id == request.Id).FirstOrDefault();
                if (clientList != null && clientList.Id > 0)
                {
                    _clientDetails.ClientCultureNeedModel = (from client in _dbContext.ClientCultureNeed
                                                             where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                             select new LHSAPI.Application.Client.Models.ClientCultureNeed
                                                             {
                                                                 Id = client.Id,
                                                                 ClientId = client.ClientId,
                                                                 CultureNeed = client.CultureNeed

                                                             }).FirstOrDefault();

                    _clientDetails.ClientSafetyBehavioursInfoModel = (from client in _dbContext.ClientSafetyBehavioursInfo
                                                                      where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                                      select new LHSAPI.Application.Client.Models.ClientSafetyBehavioursInfo
                                                                      {
                                                                          Id = client.Id,
                                                                          ClientId = client.ClientId,
                                                                          IsHistoryFalls=client.IsHistoryFalls,
                                                                          HistoryFallsDetails = client.HistoryFallsDetails,
                                                                          IsAbsconding = client.IsAbsconding,
                                                                          AbscondingHistory = client.AbscondingHistory,
                                                                          IsBSP = client.IsBSP,
                                                                          BSPDetails = client.BSPDetails

                                                                      }).FirstOrDefault();
                    _clientDetails.ClientBehaviourofConcernModel = (from client in _dbContext.ClientBehaviourofConcern
                                                                    where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                                    select new LHSAPI.Application.Client.Models.ClientBehaviourofConcern
                                                                    {
                                                                        Id = client.Id,
                                                                        ClientId = client.ClientId,
                                                                        IsBehaviourConcern = client.IsBehaviourConcern,
                                                                        IsSelfInjury = client.IsSelfInjury,
                                                                        IsHitting = client.IsHitting,
                                                                        IsPullingHair = client.IsPullingHair,
                                                                        IsHeadButting = client.IsHeadButting,
                                                                        IsBanging = client.IsBanging,
                                                                        IsKicking = client.IsKicking,
                                                                        IsScreaming = client.IsScreaming,
                                                                        IsBiting = client.IsBiting,
                                                                        IsThrowing = client.IsThrowing,
                                                                        IsObsessiveBehaviour = client.IsObsessiveBehaviour,
                                                                        IsOther = client.IsOther,
                                                                        BehaviourConcernDetails = client.BehaviourConcernDetails,
                                                                        IsPinching= client.IsPinching
                                                                    }).FirstOrDefault();
                    _clientDetails.ClientEatingNutritionModel = (from client in _dbContext.ClientEatingNutrition
                                                                      where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                                      select new LHSAPI.Application.Client.Models.ClientEatingNutrition
                                                                      {
                                                                          Id = client.Id,
                                                                          ClientId = client.ClientId,
                                                                          IsEatsIndependently = client.IsEatsIndependently,
                                                                          EatingNutritionDetails = client.EatingNutritionDetails,
                                                                          IsPreparingMeals = client.IsPreparingMeals,
                                                                          MealsDetails = client.MealsDetails,
                                                                          IsUsesUtensils = client.IsUsesUtensils,
                                                                          UtensilsDetails = client.UtensilsDetails,
                                                                          IsFluids = client.IsFluids,
                                                                          FluidsDetails = client.FluidsDetails,
                                                                          IsModifiedFood = client.IsModifiedFood,
                                                                          IsPEG = client.IsPEG,
                                                                          IsSwallowingImpairment = client.IsSwallowingImpairment,
                                                                          IsDietPlan = client.IsDietPlan,
                                                                          AllergiesDetails = client.AllergiesDetails,
                                                                          HasChoking = client.HasChoking,
                                                                          ChokingDetails = client.ChokingDetails,
                                                                          FoodPreferences = client.FoodPreferences,

                                                                      }).FirstOrDefault();
                    _clientDetails.ClientLivingArrangementModel = (from client in _dbContext.ClientLivingArrangement
                                                                      where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                                      select new LHSAPI.Application.Client.Models.ClientLivingArrangement
                                                                      {
                                                                          Id = client.Id,
                                                                          ClientId = client.ClientId,
                                                                          LivingArrangement = client.LivingArrangement
                                                                         

                                                                      }).FirstOrDefault();
                    _clientDetails.ClientOtherInformtionModel = (from client in _dbContext.ClientOtherInformtion
                                                                   where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                                   select new LHSAPI.Application.Client.Models.ClientOtherInformtion
                                                                   {
                                                                       Id = client.Id,
                                                                       ClientId = client.ClientId,
                                                                       OtherInformation = client.OtherInformation,

                                                                   }).FirstOrDefault();
                    _clientDetails.ClientPersonalPreferencesModel = (from client in _dbContext.ClientPersonalPreferences
                                                                 where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                                 select new LHSAPI.Application.Client.Models.ClientPersonalPreferences
                                                                 {
                                                                     Id = client.Id,
                                                                     ClientId = client.ClientId,
                                                                     Interest = client.Interest,
                                                                     ClientImportance = client.ClientImportance,
                                                                     Charecteristics = client.Charecteristics,
                                                                     FearsandAnxities = client.FearsandAnxities,

                                                                 }).FirstOrDefault();
                    _clientDetails.ClientSocialConnectionsModel = (from client in _dbContext.ClientSocialConnections
                                                                     where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id
                                                                     select new LHSAPI.Application.Client.Models.ClientSocialConnections
                                                                     {
                                                                         Id = client.Id,
                                                                         ClientId = client.ClientId,
                                                                         SocialConnection = client.SocialConnection,
                                                                        

                                                                     }).FirstOrDefault();
                    if (_clientDetails.ClientCultureNeedModel == null) _clientDetails.ClientCultureNeedModel = new LHSAPI.Application.Client.Models.ClientCultureNeed();
                    if (_clientDetails.ClientSocialConnectionsModel == null) _clientDetails.ClientSocialConnectionsModel = new LHSAPI.Application.Client.Models.ClientSocialConnections();
                    if (_clientDetails.ClientSafetyBehavioursInfoModel == null) _clientDetails.ClientSafetyBehavioursInfoModel = new LHSAPI.Application.Client.Models.ClientSafetyBehavioursInfo();
                    if (_clientDetails.ClientBehaviourofConcernModel == null) _clientDetails.ClientBehaviourofConcernModel = new LHSAPI.Application.Client.Models.ClientBehaviourofConcern();
                    if (_clientDetails.ClientEatingNutritionModel == null) _clientDetails.ClientEatingNutritionModel = new LHSAPI.Application.Client.Models.ClientEatingNutrition();
                    if (_clientDetails.ClientLivingArrangementModel == null) _clientDetails.ClientLivingArrangementModel = new LHSAPI.Application.Client.Models.ClientLivingArrangement();
                    if (_clientDetails.ClientOtherInformtionModel == null) _clientDetails.ClientOtherInformtionModel = new LHSAPI.Application.Client.Models.ClientOtherInformtion();
                    if (_clientDetails.ClientPersonalPreferencesModel == null) _clientDetails.ClientPersonalPreferencesModel = new LHSAPI.Application.Client.Models.ClientPersonalPreferences();
                    response.SuccessWithOutMessage(_clientDetails);

                }

                else
                {
                    response.Status = (int)Number.Zero;
                    response.Message = ResponseMessage.NOTFOUND;
                    response.StatusCode = HTTPStatusCode.NO_DATA_FOUND;
                }

            }
            catch (Exception ex)
            {
                response.Status = (int)Number.Zero;
                response.Message = ResponseMessage.Error;
                response.StatusCode = HTTPStatusCode.INTERNAL_SERVER_ERROR;
            }
            return response;
        }
        #endregion
    }
}
