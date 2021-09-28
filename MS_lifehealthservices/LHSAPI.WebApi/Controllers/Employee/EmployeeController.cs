using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientDocument;
using LHSAPI.Application.Employee.Commands.Create.AddEmployee;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeAccidentInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeAppraisalDetails;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeAvailability;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeAwardInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeCommunicationInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeCompliancesDetails;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeDrivingLicenseInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeEducation;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeJobProfile;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeKinInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeLeaveDetails;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeMiscInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeOtherCompliancesInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeePicInfo;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeSignUp;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeStaffWarning;

using LHSAPI.Application.Employee.Commands.Create.AddEmployeeTraining;
using LHSAPI.Application.Employee.Commands.Create.AddEmployeeWorkExp;
using LHSAPI.Application.Employee.Commands.Create.AddPayRate;
using LHSAPI.Application.Employee.Commands.Create.Update.EditEmployee;
using LHSAPI.Application.Employee.Commands.Delete.DeleteAccidentIncidentInfo;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEducationDocument;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeAppraisal;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeAvailability;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeCommunication;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeCompliances;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeDrivingLicense;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeEducationInfo;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeExperienceInfo;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeLeave;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeListing;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeOtherDocument;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeRequireDocument;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeStaffWarning;
using LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeTraining;
using LHSAPI.Application.Employee.Commands.Delete.DeleteExperienceDocument;
using LHSAPI.Application.Employee.Commands.Delete.DeleteOtherEmployeeCompliances;
using LHSAPI.Application.Employee.Commands.Delete.DeleteTrainingDocument;
using LHSAPI.Application.Employee.Commands.Update.ApproveEmployeeLeave;
using LHSAPI.Application.Employee.Commands.Update.EditEmployeeAccidentDetails;
using LHSAPI.Application.Employee.Commands.Update.EditEmployeeAppraisalInfo;
using LHSAPI.Application.Employee.Commands.Update.EditEmployeeAvailability;
using LHSAPI.Application.Employee.Commands.Update.EditEmployeeCommunication;
using LHSAPI.Application.Employee.Commands.Update.EditEmployeeCompliances;
using LHSAPI.Application.Employee.Commands.Update.EditEmployeeLeave;
using LHSAPI.Application.Employee.Commands.Update.EditEmployeeStaffWarning;
using LHSAPI.Application.Employee.Commands.Update.EditOtherEmployeeCompliances;
using LHSAPI.Application.Employee.Commands.Update.UpdateEmployeeActiveStatus;
using LHSAPI.Application.Employee.Commands.Update.UpdateEmployeeTraining;
using LHSAPI.Application.Employee.Queries;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeAccidentList;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeAppraisalList;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeAvailableList;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeCommunicationList;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeLeaveList;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeOthComplianceList;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeReqComplianceList;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeShortInfo;
using LHSAPI.Application.Employee.Queries.GetAllEmployeeStaffwarningList;
using LHSAPI.Application.Employee.Queries.GetEmployeeAccidentDetail;
using LHSAPI.Application.Employee.Queries.GetEmployeeAccidentIncident;
using LHSAPI.Application.Employee.Queries.GetEmployeeAppraisaldetails;
using LHSAPI.Application.Employee.Queries.GetEmployeeAppraisalList;
using LHSAPI.Application.Employee.Queries.GetEmployeeAvailableInfo;
using LHSAPI.Application.Employee.Queries.GetEmployeeAvailableList;
using LHSAPI.Application.Employee.Queries.GetEmployeeCommunicationDetails;
using LHSAPI.Application.Employee.Queries.GetEmployeeCommunicationList;
using LHSAPI.Application.Employee.Queries.GetEmployeeComplianceList;
using LHSAPI.Application.Employee.Queries.GetEmployeeCompliancesDetails;
using LHSAPI.Application.Employee.Queries.GetEmployeeDriverLicense;
using LHSAPI.Application.Employee.Queries.GetEmployeeEducationList;
using LHSAPI.Application.Employee.Queries.GetEmployeeExperienceList;
using LHSAPI.Application.Employee.Queries.GetEmployeeLeaveDetail;
using LHSAPI.Application.Employee.Queries.GetEmployeeOtherComplianceList;
using LHSAPI.Application.Employee.Queries.GetEmployeeOtherCompliancesDetails;
using LHSAPI.Application.Employee.Queries.GetEmployeePasswordInfo;
using LHSAPI.Application.Employee.Queries.GetEmployeePrimaryInfo;
using LHSAPI.Application.Employee.Queries.GetEmployeeStaffWarningDetail;
using LHSAPI.Application.Employee.Queries.GetEmployeeStaffWarningList;
using LHSAPI.Application.Employee.Queries.GetEmployeeTodayShiftInfo;
using LHSAPI.Application.Employee.Queries.GetEmployeeToDoShiftInfo;
using LHSAPI.Application.Employee.Queries.GetEmployeeTrainingList;
using LHSAPI.Application.Employee.Queries.GetLeaveList;
using LHSAPI.Application.Master.Queries.GetEmployeeTotalWarning;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LHSAPI.Controllers.Employee
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LHSDbContext _dbContext;
        // GET: api/Employee
        public EmployeeController(UserManager<ApplicationUser> userManager, LHSDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Employee
        [HttpPost]
        // [Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeInfo")]
        public async Task<IActionResult> AddEmployeeInfo([FromBody] AddEmployeeInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        // POST: api/Employee
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeMiscInfo")]
        public async Task<IActionResult> AddEmployeeMiscInfo([FromBody] AddEmployeeMiscInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeePicInfo")]
        public async Task<IActionResult> AddEmployeePicInfo([FromForm] AddEmployeePicInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        // [Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeKinInfo")]
        public async Task<IActionResult> AddEmployeeKinInfo([FromBody] AddEmployeeKinInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeAwardInfo")]
        public async Task<IActionResult> AddEmployeeAwardInfo([FromBody] AddEmployeeAwardInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeDrivingLicenseInfo")]
        public async Task<IActionResult> AddEmployeeDrivingLicenseInfo([FromBody] AddEmployeeDrivingLicenseInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeEducation")]
        public async Task<IActionResult> AddEmployeeEducation([FromForm] AddEmployeeEducationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeJobProfile")]
        public async Task<IActionResult> AddEmployeeJobProfile([FromBody] AddEmployeeJobProfileCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddEmployeeWorkExp")]
        //[Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> AddEmployeeWorkExp([FromForm] AddEmployeeWorkExpCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeePayRate")]
        public async Task<IActionResult> AddEmployeePayRate([FromBody] AddPayRateCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdmin")]
        [Route("GetEmployeeList")]
        public async Task<IActionResult> GetEmployeeList([FromBody] GetEmployeeList model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeDetail")]
        public async Task<IActionResult> GetEmployeeDetail(int Id)
        {
            return Ok(await Mediator.Send(new GetEmployeeDetail { Id = Id }));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeInfo")]
        public async Task<IActionResult> EditEmployeeInfo([FromBody] EditEmployeeCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeePrimaryInfo")]
        public async Task<IActionResult> GetEmployeePrimaryInfo(int Id)
        {
            return Ok(await Mediator.Send(new GetEmployeePrimaryInfo { Id = Id }));
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #region "Accident/Incident"
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeAccidentInfo")]
        public async Task<IActionResult> AddEmployeeAccidentInfo([FromBody] AddEmployeeAccidentInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        //[Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireEndUser")]
        [Route("GetAccidentIncidentList")]
        public async Task<IActionResult> GetAccidentIncidentList([FromBody] GetEmployeeAccidentListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeAccidentDetail")]
        public async Task<IActionResult> GetEmployeeAccidentDetail([FromBody] GetEmployeeAccidentInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeAccidentInfo")]
        public async Task<IActionResult> EditEmployeeAccidentInfo([FromBody] EditEmployeeAccidentInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeAccidentInfo")]
        public async Task<IActionResult> DeleteEmployeeAccidentInfo([FromBody] DeleteAccidentInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        #endregion

        #region "Employee Leave"
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeLeaveDetail")]
        public async Task<IActionResult> AddEmployeeLeaveDetail([FromBody] AddEmployeeLeaveDetailsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetLeaveList")]
        public async Task<IActionResult> GetLeaveList([FromBody] GetLeaveListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeLeaveDetail")]
        public async Task<IActionResult> GetEmployeeLeaveDetail([FromBody] GetEmployeeLeaveDetailQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeLeaveInfo")]
        public async Task<IActionResult> EditEmployeeLeaveInfo([FromBody] EditEmployeeLeaveCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeLeave")]
        public async Task<IActionResult> DeleteEmployeeLeave([FromBody] DeleteEmployeeLeaveCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        #endregion

        #region "Availability"
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeAvailabilityDetail")]
        public async Task<IActionResult> AddEmployeeAvailabilityDetail([FromBody] AddEmployeeAvailabilityCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetAvailabilityList")]
        public async Task<IActionResult> GetAvailabilityList([FromBody] GetEmployeeAvailableListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeAvailabilityDetail")]
        public async Task<IActionResult> GetEmployeeAvailabilityDetail([FromBody] GetEmployeeAvailableInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeAvailabilityInfo")]
        public async Task<IActionResult> EditEmployeeAvailabilityInfo([FromBody] EditEmployeeAvailabilityCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeAvailability")]
        public async Task<IActionResult> DeleteEmployeeAvailability([FromBody] DeleteEmployeeAvailabilityCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        #endregion

        #region "Communication"
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeCommunicationInfo")]
        public async Task<IActionResult> AddEmployeeCommunicationInfo([FromBody] AddEmployeeCommunicationInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetEmployeeCommunicationList")]
        public async Task<IActionResult> GetEmployeeCommunicationList([FromBody] GetEmployeeCommunicationListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeCommunicationDetail")]
        public async Task<IActionResult> GetEmployeeCommunicationDetail([FromBody] GetEmployeeCommunicationDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeCommunicationInfo")]
        public async Task<IActionResult> EditEmployeeCommunicationInfo([FromBody] EditEmployeeCommunicationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeCommunication")]
        public async Task<IActionResult> DeleteEmployeeCommunication([FromBody] DeleteEmployeeCommunicationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        #endregion

        #region "Compliances"

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeCompliancesDetail")]
        public async Task<IActionResult> AddEmployeeCompliancesDetail([FromForm] AddEmployeeCompliancesDetailsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }



        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetRequireCompliancesList")]
        public async Task<IActionResult> GetRequireCompliancesList([FromBody] GetEmployeeComplianceListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetOtherCompliancesList")]
        public async Task<IActionResult> GetOtherCompliancesList([FromBody] GetEmployeeOtherComplianceListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeCompliancesDetail")]
        public async Task<IActionResult> GetEmployeeCompliancesDetail([FromBody] GetEmployeeCompliancesInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeCompliancesInfo")]
        public async Task<IActionResult> EditEmployeeCompliancesInfo([FromForm] EditEmployeeCompliancesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeCompliancesInfo")]
        public async Task<IActionResult> DeleteEmployeeCompliancesInfo([FromBody] DeleteEmployeeCompliancesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeOtherCompliancesDetail")]
        public async Task<IActionResult> AddEmployeeOtherCompliancesDetail([FromForm] AddEmployeeOtherCompliancesInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeOtherCompliancesInfo")]
        public async Task<IActionResult> EditEmployeeOtherCompliancesInfo([FromForm] EditOtherEmployeeCompliancesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeOtherCompliance")]
        public async Task<IActionResult> DeleteEmployeeOtherCompliance([FromBody] DeleteOtherEmployeeCompliancesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetOtherComplianceDetails")]
        public async Task<IActionResult> GetOtherComplianceDetails([FromBody] GetEmployeeOtherCompliancesDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        #endregion

        #region "Appraisal"
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeAppraisalDetail")]
        public async Task<IActionResult> AddEmployeeAppraisalDetail([FromBody] AddEmployeeAppraisalDetailsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetAllAppraisalList")]
        public async Task<IActionResult> GetAllAppraisalList([FromBody] GetEmployeeAppraisalListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeAppraisalInfo")]
        public async Task<IActionResult> DeleteEmployeeAppraisalInfo([FromBody] DeleteEmployeeAppraisalCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("UpdateEmployeeAppraisalDetail")]
        public async Task<IActionResult> UpdateEmployeeAppraisalDetail([FromBody] EditEmployeeAppraisalInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        #endregion

        #region "Staff Warning"
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddEmployeeStaffWarningDetail")]
        public async Task<IActionResult> AddEmployeeStaffWarningDetail([FromBody] AddEmployeeStaffWarningCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        // [Authorize(Policy = "RequireEndUser")]
        [Route("GetStaffWarningList")]
        public async Task<IActionResult> GetStaffWarningList([FromBody] GetEmployeeStaffWarningListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeStaffWarningDetail")]
        public async Task<IActionResult> GetEmployeeStaffWarningDetail([FromBody] GetEmployeeStaffWarningQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("EditEmployeeStaffWarningInfo")]
        public async Task<IActionResult> EditEmployeeStaffWarningInfo([FromBody] EditEmployeeStaffWarningCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("DeleteEmployeeStaffWarning")]
        public async Task<IActionResult> DeleteEmployeeStaffWarning([FromBody] DeleteEmployeeStaffWarningCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetEmployeeAppraisaldetails")]
        public async Task<IActionResult> GetEmployeeAppraisaldetails([FromBody] GetEmployeeAppraisaldetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("DeleteEmployeedetails")]
        public async Task<IActionResult> DeleteEmployeedetails([FromBody] DeleteEmployeeListingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("DeleteEmployeeEducationdetails")]
        public async Task<IActionResult> DeleteEmployeeEducationdetails([FromBody] DeleteEmployeeEducationInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteEmployeeExperiencedetails")]
        public async Task<IActionResult> DeleteEmployeeExperiencedetails([FromBody] DeleteEmployeeExperienceInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        #endregion

        #region "MainMenue Listing"
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeAccidentList")]
        public async Task<IActionResult> GetAllEmployeeAccidentList([FromBody] GetAllEmployeeAccidentListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeAppraisalList")]
        public async Task<IActionResult> GetAllEmployeeAppraisalList([FromBody] GetAllEmployeeAppraisalListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeAvailableList")]
        public async Task<IActionResult> GetAllEmployeeAvailableList([FromBody] GetAllEmployeeAvailableListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeCommunicationList")]
        public async Task<IActionResult> GetAllEmployeeCommunicationList([FromBody] GetAllEmployeeCommunicationListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeLeaveList")]
        public async Task<IActionResult> GetAllEmployeeLeaveList([FromBody] GetAllEmployeeLeaveListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeOthComplianceList")]
        public async Task<IActionResult> GetAllEmployeeOthComplianceList([FromBody] GetAllEmployeeOthComplianceListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeReqComplianceList")]
        public async Task<IActionResult> GetAllEmployeeReqComplianceList([FromBody] GetAllEmployeeReqComplianceListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("GetAllEmployeeStafwarning")]
        public async Task<IActionResult> GetAllEmployeeStafwarning([FromBody] GetAllEmployeeStaffwarningListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("DeleteEmployeeRequireDocument")]
        public async Task<IActionResult> DeleteEmployeeRequireDocument([FromBody] DeleteEmployeeRequireDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteEmployeeOtherDocument")]
        public async Task<IActionResult> DeleteEmployeeOtherDocument([FromBody] DeleteEmployeeOtherDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddEmployeeTraningInfo")]
        public async Task<IActionResult> AddEmployeeTraningInfo([FromForm] AddEmployeeTrainingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteEmployeeTraining")]
        public async Task<IActionResult> DeleteEmployeeTraining([FromBody] DeleteEmployeeTrainingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateEmployeeTraining")]
        public async Task<IActionResult> UpdateEmployeeTraining([FromForm] UpdateEmployeeTrainingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeTrainingList")]
        public async Task<IActionResult> GetEmployeeTrainingList([FromBody] GetEmployeeTrainingListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        #endregion
        [HttpGet]
        [Route("GetAllEmployeeShortInfo")]
        public async Task<IActionResult> GetAllEmployeeShortInfo()
        {
            return Ok(await Mediator.Send(new GetAllEmployeeShortInfoQuery { }));
        }

        [HttpPost]
        [Route("GetEmployeeEducationList")]
        public async Task<IActionResult> GetEmployeeEducationList([FromBody] GetEmployeeEducationListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeExperienceList")]
        public async Task<IActionResult> GetEmployeeExperienceList([FromBody] GetEmployeeExperienceListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteEmployeeEducationDocument")]
        public async Task<IActionResult> DeleteEmployeeEducationDocument([FromBody] DeleteEducationDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteEmployeeExperienceDocument")]
        public async Task<IActionResult> DeleteEmployeeExperienceDocument([FromBody] DeleteExperienceDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteTrainingDocument")]
        public async Task<IActionResult> DeleteTrainingDocument([FromBody] DeleteTrainingDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateEmployeeActiveStatus")]
        public async Task<IActionResult> UpdateEmployeeActiveStatus([FromBody] UpdateEmployeeActiveStatusCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeTodayShiftInfo")]
        public async Task<IActionResult> GetEmployeeTodayShiftInfo([FromBody] GetEmployeeTodayShiftInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeePasswordInfo")]
        public async Task<IActionResult> GetEmployeePasswordInfo([FromBody] GetEmployeePasswordInfo model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeToDoShiftInfo")]
        public async Task<IActionResult> GetEmployeeToDoShiftInfo([FromBody] GetEmployeeToDoShiftInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateEmployeePassword")]
        public async Task<IActionResult> UpdateEmployeePassword([FromBody] AddEmployeeSignUpCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetEmployeeDriverLicense")]
        public async Task<IActionResult> GetEmployeeDriverLicense([FromBody] GetEmployeeDriverLicenseListQuery model)
        {
            return Ok(await Mediator.Send(model));
        } 
        [HttpPost]
        [Route("DeleteEmployeeDrivingLicense")]
        public async Task<IActionResult> DeleteEmployeeDrivingLicense([FromBody] DeleteEmployeeDrivingLicenseCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("ApproveEmployeeLeaveById")]
        public async Task<IActionResult> ApproveEmployeeLeaveById([FromBody] ApproveEmployeeLeaveInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("RejectEmployeeLeaveById")]
        public async Task<IActionResult> RejectEmployeeLeaveById([FromBody] RejectEmployeeLeaveInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

    }
}
