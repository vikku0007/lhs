using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LHSAPI.Application.Master.Queries.GetGender;
using LHSAPI.Application.Master.Queries.GetLanguage;
using LHSAPI.Application.Master.Queries.GetDepartment;
using LHSAPI.Application.Master.Queries.GetEmployeeType;
using LHSAPI.Application.Master.Queries.GetLevel;
using LHSAPI.Application.Master.Queries.GetMaritalStatus;
using LHSAPI.Application.Master.Queries.GetRelationship;
using LHSAPI.Application.Master.Queries.GetRoles;
using LHSAPI.Application.Master.Queries.GetSourceOfHire;
using LHSAPI.Application.Master.Queries.GetEventType;
using LHSAPI.Application.Master.Queries.GetLocation;
using LHSAPI.Application.Master.Queries.GetWarningType;
using LHSAPI.Application.Master.Queries.GetOffenseType;
using LHSAPI.Application.Master.Queries.GetRepotedTo;
using LHSAPI.Application.Master.Queries.GetRaisedBy;
using LHSAPI.Application.Master.Queries.GetAppraisalType;
using LHSAPI.Application.Master.Queries.GetAwardGroup;
using LHSAPI.Application.Master.Queries.GetGlobalPayRate;
using LHSAPI.Application.Master.Queries.GetFundType;
using LHSAPI.Application.Master.Queries.GetDocument;
using LHSAPI.Application.Master.Queries.GetSalutation;
using LHSAPI.Application.Master.Queries.GetServiceType;
using LHSAPI.Application.Master.Queries.GetDocumentName;
using LHSAPI.Application.Master.Queries.GetOtherDocumentName;
using LHSAPI.Application.Master.Queries.GetDepartmentByEmployee;
using LHSAPI.Application.Master.Queries.GetReportedByEmployee;
using LHSAPI.Application.Master.Queries.GetShiftStatus;
using LHSAPI.Application.Master.Queries.GetLeaveType;
using LHSAPI.Application.Master.Queries.GetVisaType;
using LHSAPI.Application.Master.Queries.GetConditionType;
using LHSAPI.Application.Master.Queries.GetSymptomsType;
using LHSAPI.Application.Master.Queries.GetEthnicity;
using LHSAPI.Application.Master.Queries.GetReligion;
using LHSAPI.Application.Master.Queries.GetState;
using LHSAPI.Application.Master.Queries.GetCountry;
using LHSAPI.Application.Master.Queries.GetCourseType;
using LHSAPI.Application.Master.Queries.GetMandatoryTraining;
using LHSAPI.Application.Master.Queries.GetOptionalTraining;
using LHSAPI.Application.Master.Queries.GetTrainingType;
using LHSAPI.Application.Master.Queries.GetServiceInfo;
using LHSAPI.Application.Master.Queries.GetServiceRate;
using LHSAPI.Application.Master.Queries.GetLicenseType;
using LHSAPI.Application.Master.Queries.GetCodeData;
using LHSAPI.Application.Master.Queries.GetLocationType;
using LHSAPI.Application.Master.Queries.GetHobbies;
using LHSAPI.Application.Master.Queries.GetQualificationType;
using LHSAPI.Application.Master.Queries.GetAppraisalCrieteria;
using LHSAPI.Application.Master.Queries.GetEmployeeTotalWarning;
using LHSAPI.Application.Master.Queries.GetPaymentTerm;
using LHSAPI.Application.Master.Queries.GetPayers;
using LHSAPI.Application.Master.Queries.GetCommunicationType;
using LHSAPI.Application.Master.Queries.GetConcernBehaviour;
using LHSAPI.Application.Master.Queries.GetPrimaryCategory;
using LHSAPI.Application.Master.Queries.GetSecondaryCategory;
using LHSAPI.Application.Master.Queries.GetPrimaryDisability;
using LHSAPI.Application.Master.Queries.GetSecondaryDisability;
using LHSAPI.Application.Master.Queries.GetAMShift;
using LHSAPI.Application.Master.Queries.GetShiftTime;
using LHSAPI.Application.Master.Queries.GetPMShift;
using LHSAPI.Application.Master.Queries.GetActiveNightShift;
using LHSAPI.Application.Master.Queries.GetClientDocuments;
using LHSAPI.Application.Master.Queries.GetOptionalDocument;
using LHSAPI.Application.Master.Queries.GetYear;
using LHSAPI.Application.Master.Queries.GetCheckListDocument;
using LHSAPI.Application.Master.Queries.GetEmployeeLevel;
using LHSAPI.Application.Master.Queries.GetImageDetail;
using LHSAPI.Application.Master.Queries.GetAccidentIncident;
using LHSAPI.Application.Master.Queries.GetMedicalHistory;
using LHSAPI.Application.Master.Queries.GetLicense;
using LHSAPI.Application.Master.Queries.GetPayRateDetails;
using LHSAPI.Application.Master.Queries.GetComplianceType;
using LHSAPI.Application.Master.Queries.GetReferenceNumber;
using LHSAPI.Application.Master.Queries.GetEmployeeCheckListdoc;

namespace LHSAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
    [ApiController]
    public class MasterController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LHSDbContext _dbContext;
        // GET: api/Employee
        public MasterController(UserManager<ApplicationUser> userManager, LHSDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }


        [HttpGet]
        [Route("GetGender")]
        public async Task<IActionResult> GetGender()
        {
            GetGenderQuery model = new GetGenderQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetLanguage")]
        public async Task<IActionResult> GetLanguage()
        {
            GetLanguageQuery model = new GetLanguageQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetDepartment")]
        public async Task<IActionResult> GetDepartment()
        {
            GetDepartmentQuery model = new GetDepartmentQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetEmployeeType")]
        public async Task<IActionResult> GetEmployeeType()
        {
            GetEmployeeTypeQuery model = new GetEmployeeTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetLevel")]
        public async Task<IActionResult> GetLevel()
        {
            GetLevelQuery model = new GetLevelQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetMaritalStatus")]
        public async Task<IActionResult> GetMaritalStatus()
        {
            GetMaritalStatusQuery model = new GetMaritalStatusQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetRelationship")]
        public async Task<IActionResult> GetRelationship()
        {
            GetRelationshipQuery model = new GetRelationshipQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            GetRolesQuery model = new GetRolesQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetSalutation")]
        public async Task<IActionResult> GetSalutation()
        {
            GetSalutationQuery model = new GetSalutationQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetSourceOfHire")]
        public async Task<IActionResult> GetSourceOfHire()
        {
            GetSourceOfHireQuery model = new GetSourceOfHireQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetEventType")]
        public async Task<IActionResult> GetEventType()
        {
            GetEventTypeQuery model = new GetEventTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetLocation")]
        public async Task<IActionResult> GetLocation()
        {
            GetLocationInfoQuery model = new GetLocationInfoQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetWarningType")]
        public async Task<IActionResult> GetWarningType()
        {
            GetWarningTypeQuery model = new GetWarningTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetOffenseType")]
        public async Task<IActionResult> GetOffenseType()
        {
            GetOffenseTypeQuery model = new GetOffenseTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetReportedTo")]
        public async Task<IActionResult> GetReportedTo()
        {
            GetRepotedToQuery model = new GetRepotedToQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetRaisedBy")]
        public async Task<IActionResult> GetRaisedBy([FromBody] GetRaisedByQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetAppraisalType")]
        public async Task<IActionResult> GetAppraisalType()
        {
            GetAppraisalTypeQuery model = new GetAppraisalTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetAwardGroup")]
        public async Task<IActionResult> GetAwardGroup()
        {
            GetAwardGroupQuery model = new GetAwardGroupQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetFundType")]

        public async Task<IActionResult> GetFundType()
        {
            GetFundTypeQuery model = new GetFundTypeQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetDocumentType")]
        public async Task<IActionResult> GetDocumentType()
        {
            GetDocumentQuery model = new GetDocumentQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetGlobalPayRate")]
        public async Task<IActionResult> GetGlobalPayRate(int level)
        {
            return Ok(await Mediator.Send(new GetGlobalPayRate { Level = level }));
        }
        [HttpGet]
        [Route("GetServiceType")]
        public async Task<IActionResult> GetServiceType()
        {
            GetServiceTypeQuery model = new GetServiceTypeQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetDocumentName")]
        public async Task<IActionResult> GetDocumentName()
        {
            GetDocumentNameQuery model = new GetDocumentNameQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetOtherDocumentName")]
        public async Task<IActionResult> GetOtherDocumentName()
        {
            GetOtherDocumentNameQuery model = new GetOtherDocumentNameQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetDepartmentByEmployee")]
        public async Task<IActionResult> GetDepartmentByEmployee(int employeeId)
        {
            return Ok(await Mediator.Send(new GetDepartmentByEmployeeQuery { EmployeeId = employeeId }));
        }
        [HttpGet]
        [Route("GetReportedByEmployee")]
        public async Task<IActionResult> GetReportedByEmployee(int employeeId)
        {
            return Ok(await Mediator.Send(new GetReportedByEmployeeQuery { EmployeeId = employeeId }));
        }
        [HttpGet]
        [Route("GetShiftStatusList")]
        public async Task<IActionResult> GetShiftStatusList()
        {
            return Ok(await Mediator.Send(new GetShiftStatusQuery { }));
        }

        [HttpGet]
        [Route("GetLeaveType")]
        public async Task<IActionResult> GetLeaveType()
        {
            GetLeaveTypeQuery model = new GetLeaveTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetVisaType")]
        public async Task<IActionResult> GetVisaType()
        {
            GetVisaTypeQuery model = new GetVisaTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetConditionType")]
        public async Task<IActionResult> GetConditionType()
        {
            GetConditionTypeQuery model = new GetConditionTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetSymptomsType")]
        public async Task<IActionResult> GetSymptomsType()
        {
            GetSymptomsTypeQuery model = new GetSymptomsTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetReligion")]
        public async Task<IActionResult> GetReligion()
        {
            GetReligionQuery model = new GetReligionQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetEthnicityType")]
        public async Task<IActionResult> GetEthnicityType()
        {
            GetEthnicityQuery model = new GetEthnicityQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetState")]
        public async Task<IActionResult> GetState()
        {
            GetStateQuery model = new GetStateQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            GetCountryQuery model = new GetCountryQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetCourseType")]
        public async Task<IActionResult> GetCourseType()
        {
            GetCourseTypeQuery model = new GetCourseTypeQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetMandatoryTraining")]
        public async Task<IActionResult> GetMandatoryTraining()
        {
            GetMandatoryTrainingQuery model = new GetMandatoryTrainingQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetOptionalTraining")]
        public async Task<IActionResult> GetOptionalTraining()
        {
            GetOptionalTrainingQuery model = new GetOptionalTrainingQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetTrainingType")]
        public async Task<IActionResult> GetTrainingType()
        {
            GetTrainingTypeQuery model = new GetTrainingTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetService")]
        public async Task<IActionResult> GetService()
        {
            GetServiceInfoQuery model = new GetServiceInfoQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetServiceRate")]
        public async Task<IActionResult> GetServiceRate(int Id)
        {
            return Ok(await Mediator.Send(new GetServiceRateQuery { Id = Id }));
        }

        [HttpGet]
        [Route("GetLicenseType")]
        public async Task<IActionResult> GetLicenseType()
        {
            GetLicenseTypeQuery model = new GetLicenseTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetCodeData")]
        public async Task<IActionResult> GetCodeData()
        {
            GetCodeDataQuery model = new GetCodeDataQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetLocationType")]
        public async Task<IActionResult> GetLocationType()
        {
            GetLocationTypeQuery model = new GetLocationTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetHobbies")]
        public async Task<IActionResult> GetHobbies()
        {
            GetHobbiesQuery model = new GetHobbiesQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetQualificationType")]
        public async Task<IActionResult> GetQualificationType()
        {
            GetQualificationTypeQuery model = new GetQualificationTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetAppraisalCrieteria")]
        public async Task<IActionResult> GetAppraisalCrieteria()
        {
            GetAppraisalCrieteriaQuery model = new GetAppraisalCrieteriaQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeTotalWarning")]
        public async Task<IActionResult> GetEmployeeTotalWarning([FromBody] GetEmployeeTotalWarningQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetPaymentTerm")]
        public async Task<IActionResult> GetPaymentTerm()
        {
            GetPaymentTermQuery model = new GetPaymentTermQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetPayers")]
        public async Task<IActionResult> GetPayers()
        {
            GetPayersQuery model = new GetPayersQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetClientDocuments")]
        public async Task<IActionResult> GetClientDocuments()
        {
            GetClientDocumentsQuery model = new GetClientDocumentsQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetCommunicationType")]
        public async Task<IActionResult> GetCommunicationType()
        {
            GetCommunicationTypeQuery model = new GetCommunicationTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetConcernBehaviour")]
        public async Task<IActionResult> GetConcernBehaviour()
        {
            GetConcernBehaviourQuery model = new GetConcernBehaviourQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetPrimaryCategory")]
        public async Task<IActionResult> GetPrimaryCategory()
        {
            GetPrimaryCategoryQuery model = new GetPrimaryCategoryQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetSecondaryCategory")]
        public async Task<IActionResult> GetSecondaryCategory()
        {
            GetSecondaryCategoryQuery model = new GetSecondaryCategoryQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetPrimaryDisability")]
        public async Task<IActionResult> GetPrimaryDisability()
        {
            GetPrimaryDisabilityQuery model = new GetPrimaryDisabilityQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetSecondaryDisability")]
        public async Task<IActionResult> GetSecondaryDisability()
        {
            GetSecondaryDisabilityQuery model = new GetSecondaryDisabilityQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetShiftItemList")]
        public async Task<IActionResult> GetShiftItemList([FromBody] GetAMShiftQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetShiftTime")]
        public async Task<IActionResult> GetShiftTime()
        {
            GetShiftTimeQuery model = new GetShiftTimeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetPMShift")]
        public async Task<IActionResult> GetPMShift()
        {
            GetPMShiftQuery model = new GetPMShiftQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetActiveNightShift")]
        public async Task<IActionResult> GetActiveNightShift()
        {
            GetActiveNightShiftQuery model = new GetActiveNightShiftQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetClientOptionalDocument")]
        public async Task<IActionResult> GetClientOptionalDocuments()
        {
            GetOptionalDocumentQuery model = new GetOptionalDocumentQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetYear")]
        public async Task<IActionResult> GetYear()
        {
            GetYearQuery model = new GetYearQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetCheckListDocument")]
        public async Task<IActionResult> GetCheckListDocument()
        {
            GetCheckListDocumentQuery model = new GetCheckListDocumentQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetEmployeeLevel")]
        public async Task<IActionResult> GetEmployeeLevel(int EmployeeId)
        {
            return Ok(await Mediator.Send(new GetEmployeeLevelQuery { EmployeeId = EmployeeId }));
        }
        [HttpPost]
        [Route("GetImageDetail")]
        public async Task<IActionResult> GetImageDetail([FromBody] GetImageDetailQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetAccidentIncidentStaticListing")]
        public async Task<IActionResult> GetAccidentIncidentStaticListing()
        {
            GetAccidentIncidentQuery model = new GetAccidentIncidentQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        [Route("GetMedicalHistoryStaticListing")]
        public async Task<IActionResult> GetMedicalHistoryStaticListing()
        {
            GetMedicalHistoryQuery model = new GetMedicalHistoryQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetLicense")]
        public async Task<IActionResult> GetLicense()
        {
            GetLicenseQuery model = new GetLicenseQuery();
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetPayRateDetails")]
        public async Task<IActionResult> GetPayRateDetails([FromBody] GetPayRateDetails model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetComplianceType")]
        public async Task<IActionResult> GetComplianceType()
        {
            GetComplianceTypeQuery model = new GetComplianceTypeQuery();
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetReferenceNumber")]
        public async Task<IActionResult> GetReferenceNumbe([FromBody] GetReferenceNumberQuery model)
        {
            return Ok(await Mediator.Send(model));
        } 
        [HttpGet]
        [Route("GetEmployeeCheckListdoc")]
        public async Task<IActionResult> GetEmployeeCheckListdoc()
        {
            GetEmployeeCheckListdocQuery model = new GetEmployeeCheckListdocQuery();
            return Ok(await Mediator.Send(model));
        }
    }
}
