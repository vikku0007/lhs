
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientEatingNutrition
{
    public class AddClientEatingNutritionHandler : IRequestHandler<AddClientEatingNutritionCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddClientEatingNutritionHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientEatingNutritionCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientEatingNutrition.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientEatingNutrition _Client = new ClientEatingNutrition();
                        _Client.IsEatsIndependently = request.IsEatsIndependently;
                        _Client.EatingNutritionDetails = request.EatingNutritionDetails;
                        _Client.IsPreparingMeals = request.IsPreparingMeals;
                        _Client.MealsDetails = request.MealsDetails;
                        _Client.IsUsesUtensils = request.IsUsesUtensils;
                        _Client.UtensilsDetails = request.UtensilsDetails;
                        _Client.IsFluids = request.IsFluids;
                        _Client.FluidsDetails = request.FluidsDetails;
                        _Client.IsModifiedFood = request.IsModifiedFood;
                        _Client.IsPEG = request.IsPEG;
                        _Client.IsSwallowingImpairment = request.IsSwallowingImpairment;
                        _Client.IsDietPlan = request.IsDietPlan;
                        _Client.AllergiesDetails = request.AllergiesDetails;
                        _Client.HasChoking = request.HasChoking;
                        _Client.ChokingDetails = request.ChokingDetails;
                        _Client.FoodPreferences = request.FoodPreferences;
                        _Client.CreatedById = await _ISessionService.GetUserId();
                        _Client.CreatedDate = DateTime.Now;
                        _Client.IsActive = true;
                        _Client.IsDeleted = false;
                        _Client.ClientId = request.ClientId;
                        await _context.ClientEatingNutrition.AddAsync(_Client);
                        _context.SaveChanges();
                        response.Success(_Client);

                    }
                    else
                    {
                        ExistUser.IsEatsIndependently = request.IsEatsIndependently;
                        ExistUser.EatingNutritionDetails = request.EatingNutritionDetails;
                        ExistUser.IsPreparingMeals = request.IsPreparingMeals;
                        ExistUser.MealsDetails = request.MealsDetails;
                        ExistUser.IsUsesUtensils = request.IsUsesUtensils;
                        ExistUser.UtensilsDetails = request.UtensilsDetails;
                        ExistUser.IsFluids = request.IsFluids;
                        ExistUser.FluidsDetails = request.FluidsDetails;
                        ExistUser.IsModifiedFood = request.IsModifiedFood;
                        ExistUser.IsPEG = request.IsPEG;
                        ExistUser.IsSwallowingImpairment = request.IsSwallowingImpairment;
                        ExistUser.IsDietPlan = request.IsDietPlan;
                        ExistUser.AllergiesDetails = request.AllergiesDetails;
                        ExistUser.HasChoking = request.HasChoking;
                        ExistUser.ChokingDetails = request.ChokingDetails;
                        ExistUser.FoodPreferences = request.FoodPreferences;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientEatingNutrition.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);
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
