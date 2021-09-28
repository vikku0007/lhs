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

namespace LHSAPI.Application.Master.Queries.GetAccidentIncident
{
    public class GetAccidentIncidentQueryHandler : IRequestHandler<GetAccidentIncidentQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        public GetAccidentIncidentQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse> Handle(GetAccidentIncidentQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                AccidentIncidentStaticModel accidentIncidentModel = new AccidentIncidentStaticModel();
                accidentIncidentModel.StateList = new List<State>();
                accidentIncidentModel.PrimaryCategoryList = new List<PrimaryCategory>();
                accidentIncidentModel.SecondaryCategoryList = new List<SecondaryCategory>();
                accidentIncidentModel.LocationTypeList = new List<LocationType>();
                accidentIncidentModel.PrimaryDisabilityList = new List<PrimaryDisability>();
                accidentIncidentModel.SecondaryDisabilityList = new List<SecondaryDisability>();
                accidentIncidentModel.CommunicationTypeList = new List<CommunicationType>();
                accidentIncidentModel.ConcernBehaviourList = new List<ConcernBehaviour>();
                accidentIncidentModel.GenderList = new List<Gender>();

                // State List
                var statelist = (from state in _dbContext.StandardCode
                                 where state.CodeData == Common.Enums.ResponseEnums.StandardCode.State.ToString() && state.IsActive == true
                                 select new State
                                 {
                                     Id = state.ID,
                                     CodeDescription = state.CodeDescription

                                 }).ToList();
                if (statelist != null && statelist.Any())
                {
                    accidentIncidentModel.StateList = statelist;
                }
                // Primary Category List
                var primaryCategorylist = (from appraisaltype in _dbContext.StandardCode
                                           where appraisaltype.CodeData == Common.Enums.ResponseEnums.StandardCode.PrimaryCategory.ToString() && appraisaltype.IsActive == true
                                           select new PrimaryCategory
                                           {
                                               Id = appraisaltype.ID,
                                               CodeDescription = appraisaltype.CodeDescription

                                           }).ToList();
                if (primaryCategorylist != null && primaryCategorylist.Any())
                {
                    accidentIncidentModel.PrimaryCategoryList = primaryCategorylist;
                }
                // Secondary Category List
                var secondaryCategorylist = (from appraisaltype in _dbContext.StandardCode
                                             where appraisaltype.CodeData == Common.Enums.ResponseEnums.StandardCode.SecondaryCategory.ToString() && appraisaltype.IsActive == true
                                             select new SecondaryCategory
                                             {
                                                 Id = appraisaltype.ID,
                                                 CodeDescription = appraisaltype.CodeDescription
                                             }).ToList();
                if (secondaryCategorylist != null && secondaryCategorylist.Any())
                {
                    accidentIncidentModel.SecondaryCategoryList = secondaryCategorylist;
                }
                // Location Type
                var locationTypelist = (from coursetype in _dbContext.StandardCode
                                        where coursetype.CodeData == Common.Enums.ResponseEnums.StandardCode.LocationType.ToString() && coursetype.IsActive == true
                                        select new LocationType
                                        {
                                            Id = coursetype.Value,
                                            CodeDescription = coursetype.CodeDescription
                                        }).ToList();
                if (locationTypelist != null && locationTypelist.Any())
                {
                    accidentIncidentModel.LocationTypeList = locationTypelist;
                }
                // Primary Disability
                var primaryDisabilitylist = (from appraisaltype in _dbContext.StandardCode
                                             where appraisaltype.CodeData == Common.Enums.ResponseEnums.StandardCode.PrimaryDisability.ToString() && appraisaltype.IsActive == true
                                             select new PrimaryDisability
                                             {
                                                 Id = appraisaltype.ID,
                                                 CodeDescription = appraisaltype.CodeDescription
                                             }).ToList();
                if (primaryDisabilitylist != null && primaryDisabilitylist.Any())
                {
                    accidentIncidentModel.PrimaryDisabilityList = primaryDisabilitylist;
                }
                // Secondary Disability
                var secondaryDisabilityList = (from appraisaltype in _dbContext.StandardCode
                                               where appraisaltype.CodeData == Common.Enums.ResponseEnums.StandardCode.SecondaryDisability.ToString() && appraisaltype.IsActive == true
                                               select new SecondaryDisability
                                               {
                                                   Id = appraisaltype.ID,
                                                   CodeDescription = appraisaltype.CodeDescription

                                               }).ToList();
                if (secondaryDisabilityList != null && secondaryDisabilityList.Any())
                {
                    accidentIncidentModel.SecondaryDisabilityList = secondaryDisabilityList;
                }
                // Communication Type
                var communicationTypelist = (from appraisaltype in _dbContext.StandardCode
                                             where appraisaltype.CodeData == Common.Enums.ResponseEnums.StandardCode.CommunicationType.ToString() && appraisaltype.IsActive == true
                                             select new CommunicationType
                                             {
                                                 Id = appraisaltype.ID,
                                                 CodeDescription = appraisaltype.CodeDescription
                                             }).ToList();
                if (communicationTypelist != null && communicationTypelist.Any())
                {
                    accidentIncidentModel.CommunicationTypeList = communicationTypelist;
                }
                // Concern Behaviour
                var concernBehaviourlist = (from appraisaltype in _dbContext.StandardCode
                                            where appraisaltype.CodeData == Common.Enums.ResponseEnums.StandardCode.ConcernBehaviour.ToString() && appraisaltype.IsActive == true
                                            select new ConcernBehaviour
                                            {
                                                Id = appraisaltype.ID,
                                                CodeDescription = appraisaltype.CodeDescription
                                            }).ToList();
                if (concernBehaviourlist != null && concernBehaviourlist.Any())
                {
                    accidentIncidentModel.ConcernBehaviourList = concernBehaviourlist;
                }
                // Gender
                var genderlist = (from gender in _dbContext.StandardCode
                                  where gender.CodeData == Common.Enums.ResponseEnums.StandardCode.Gender.ToString() &&
                                     gender.IsActive == true
                                  select new Gender
                                  {
                                      Id = gender.ID,
                                      CodeDescription = gender.CodeDescription

                                  }).ToList();
                if (genderlist != null && genderlist.Any())
                {
                    accidentIncidentModel.GenderList = genderlist;
                }
                var locationlist = (from location in _dbContext.Location
                                    where location.IsActive == true && location.IsDeleted == false
                                    select new Location
                                    {
                                        Id = location.LocationId,
                                        Address = location.Address,
                                        CodeDescription = location.Name
                                    }).ToList();
                if (locationlist != null && locationlist.Any())
                {
                    accidentIncidentModel.LocationList = locationlist;
                }                
                response.SuccessWithOutMessage(accidentIncidentModel);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
