using LHSAPI.Application.Master.Models;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LHSAPI.Application.Master.Queries.GetMedicalHistory
{
    public class GetMedicalHistoryQueryHandler : IRequestHandler<GetMedicalHistoryQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetMedicalHistoryQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse> Handle(GetMedicalHistoryQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                MedicalHistoryStaticModel medicalHistoryModel = new MedicalHistoryStaticModel();
                medicalHistoryModel.GenderList = new List<Gender>();
                medicalHistoryModel.SymptomType = new List<SymptomType>();
                medicalHistoryModel.ConditionType = new List<ConditionType>();
                // Gender
                var Genderlist = (from gender in _dbContext.StandardCode
                                  where gender.CodeData == Common.Enums.ResponseEnums.StandardCode.Gender.ToString() &&
                                     gender.IsActive == true
                                  select new Gender
                                  {
                                      Id = gender.ID,
                                      CodeDescription = gender.CodeDescription

                                  }).ToList();
                if (Genderlist != null && Genderlist.Any())
                {
                    medicalHistoryModel.GenderList = Genderlist;
                }
                // Condition Type
                var conditionlist = (from conditiontype in _dbContext.StandardCode
                                     where conditiontype.CodeData == Common.Enums.ResponseEnums.StandardCode.ConditionType.ToString() && conditiontype.IsActive == true
                                     select new ConditionType
                                     {
                                         Id = conditiontype.ID,
                                         CodeDescription = conditiontype.CodeDescription

                                     }).ToList();
                if (conditionlist != null && conditionlist.Any())
                {
                    medicalHistoryModel.ConditionType = conditionlist;
                }
                // Symptom List
                var symptomlist = (from symtype in _dbContext.StandardCode
                                   where symtype.CodeData == Common.Enums.ResponseEnums.StandardCode.SymptomsType.ToString() && symtype.IsActive == true
                                   select new SymptomType
                                   {
                                       Id = symtype.ID,
                                       CodeDescription = symtype.CodeDescription
                                   }).ToList();
                if (symptomlist != null && symptomlist.Any())
                {
                    medicalHistoryModel.SymptomType = symptomlist;
                }
                response.SuccessWithOutMessage(medicalHistoryModel);

            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
