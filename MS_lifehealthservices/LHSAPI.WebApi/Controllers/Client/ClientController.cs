using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHSAPI.Application.Client.Commands.Create.AddClientAdditionalNotes;
using LHSAPI.Application.Client.Commands.Create.AddClientBoardingNotes;
using LHSAPI.Application.Client.Commands.Create.AddClientFunding;
using LHSAPI.Application.Client.Commands.Create.AddClientFundingInfo;
using LHSAPI.Application.Client.Commands.Create.AddClientMedicalHistory;
using LHSAPI.Application.Client.Commands.Create.AddClientPrimaryCareInfo;
using LHSAPI.Application.Client.Commands.Create.AddClientPrimaryInfo;
using LHSAPI.Application.Client.Commands.Create.AddClientProgressNotes;
using LHSAPI.Application.Client.Commands.Create.AddClientProgressNotesItem;
using LHSAPI.Application.Client.Commands.Create.UpdateClientPrimaryInfo;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientListing;
using LHSAPI.Application.Client.Commands.Update.UpdateClientFundingInfo;
using LHSAPI.Application.Client.Commands.Update.UpdateClientProgressNotesItem;
using LHSAPI.Application.Client.Queries.GetAllClientList;
//using LHSAPI.Application.Client.Queries.GetClienProgressNotes;
using LHSAPI.Application.Client.Queries.GetClientDetailsById;
using LHSAPI.Application.Client.Queries.GetClientFundingTypeList;
using LHSAPI.Application.Client.Queries.GetClientPrimaryInfo;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientFunding;
using LHSAPI.Application.Client.Queries.GetClientProgressNotesList;
//using LHSAPI.Application.Employee.Commands.Delete.DeleteClientFunding;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using LHSAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LHSAPI.Application.Client.Queries.GetAllClientMedicalHistoryList;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientMedicalHistory;
using LHSAPI.Application.Client.Queries.GetClientProgressNotes;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientProgressNotes;
using LHSAPI.Application.Client.Queries.GetClientProgressSingleNote;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientProgressNotesItem;
using LHSAPI.Application.Client.Queries.GetClientMedicalHistoryInfo;
using LHSAPI.Application.Client.Commands.Create.AddClientPicInfo;
using LHSAPI.Application.Client.Commands.Create.AddClientSupportCoordinator;
using LHSAPI.Application.Client.Commands.Create.AddClientCompliancesDetails;
using LHSAPI.Application.Client.Queries.GetAllComplianceDetailsList;
using LHSAPI.Application.Client.Commands.Update.EditClientCompliances;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientCompliances;
using LHSAPI.Application.Client.Queries.GetAllClientDocumentsList;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientDocument;
using LHSAPI.Application.Client.Queries.GetAllClientNameList;
using LHSAPI.Application.Client.Queries.GetClientAccidentIncidentList;
using LHSAPI.Application.Client.Commands.Create.AddClientAccidentInfo;
using LHSAPI.Application.Client.Commands.Update.UpdateClientAccidentDetails;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientAccidentIIncdentnfo;
using LHSAPI.Application.Client.Queries.GetAllClientAccidentList;
using LHSAPI.Application.Client.Queries.GetClientAccidentDetails;
using LHSAPI.Application.Employee.Queries.GetClientCompliancesDetails;
using LHSAPI.Application.Client.Queries.GetClientMedicalInfo;
using LHSAPI.Application.Client.Queries.GetProgressNotesDetails;
using LHSAPI.Application.Client.Commands.Create.AddClientGuardianInfo;
using LHSAPI.Application.Client.Queries.GetOtherContactPersonInfo;
using LHSAPI.Application.Client.Commands.Delete.DeleteOtherContactPerson;
using LHSAPI.Application.Client.Commands.Update.UpdateClientActiveStatus;
using LHSAPI.Application.Client.Commands.Create.AddClientCultureNeed;
using LHSAPI.Application.Client.Commands.Create.AddClientBehaviourConcern;
using LHSAPI.Application.Client.Commands.Create.AddClientEatingNutrition;
using LHSAPI.Application.Client.Commands.Create.AddClientLivingArrangement;
using LHSAPI.Application.Client.Commands.Create.AddClientOtherInformtion;
using LHSAPI.Application.Client.Commands.Create.AddClientPreferences;
using LHSAPI.Application.Client.Commands.Create.AddClientSocialConnections;
using LHSAPI.Application.Client.Queries.GetClientProfileDetails;
using LHSAPI.Application.Client.Commands.Create.AddIncidentCategory;
using LHSAPI.Application.Client.Commands.Create.AddIncidentDeclaration;
using LHSAPI.Application.Client.Commands.Create.AddIncidentDetails;
using LHSAPI.Application.Client.Commands.Create.AddIncidentImmediateAction;
using LHSAPI.Application.Client.Commands.Create.AddIncidentImpactedPerson;
using LHSAPI.Application.Client.Commands.Create.AddIncidentPrimaryContact;
using LHSAPI.Application.Client.Commands.Create.AddIncidentProviderDetail;
using LHSAPI.Application.Client.Commands.Create.AddIncidentRiskAssesment;
using LHSAPI.Application.Client.Commands.Create.AddInciidentSubjectAllegation;
using LHSAPI.Application.Client.Commands.Create.AddIncidentDocuments;
using LHSAPI.Application.Client.Commands.Create.UpdateIncidentSubjectAllegation;
using LHSAPI.Application.Client.Queries.GetIncidentDetails;
using LHSAPI.Application.Client.Commands.Update.UpdateIncidentDocument;
using LHSAPI.Application.Client.Commands.Delete.DeleteIncidentDocument;
using LHSAPI.Application.Client.Queries.GetIncidentDocumentDetail;
using LHSAPI.Application.Client.Queries.GetIncidentImpactedPerson;
using LHSAPI.Application.Client.Queries.GetIncidentSubjectAllegation;
using LHSAPI.Application.Client.Queries.GetClientIncidentCategory;
using LHSAPI.Application.Client.Queries.GetIncidentProviderInfo;
using LHSAPI.Application.Client.Queries.GetIncidentPrimaryContact;
using LHSAPI.Application.Client.Queries.GetAccidentIncidentInfo;
using LHSAPI.Application.Client.Queries.GetIncidentImmediateAction;
using LHSAPI.Application.Client.Queries.GetRiskAssesment;
using LHSAPI.Application.Client.Queries.GetIncidentDeclaration;
using LHSAPI.Application.Client.Queries.GetClientCurrentShifts;
using LHSAPI.Application.Employee.Commands.Create.AddClientSignUp;
using LHSAPI.Application.Client.Queries.GetClientShiftList;
using LHSAPI.Application.Client.Queries.GetClientImpactedPerson;
using LHSAPI.Application.Client.Commands.Delete.DeleteClientAgreement;
using LHSAPI.Application.Client.Queries.GetClientAgreementInfo;

namespace LHSAPI.Controllers.Client
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
    [ApiController]
    public class ClientController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LHSDbContext _dbContext;
        public ClientController(UserManager<ApplicationUser> userManager, LHSDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }


        [HttpPost]
        [Route("AddClientPrimaryInfo")]
        public async Task<IActionResult> AddClientPrimaryInfo([FromBody] AddClientPrimaryInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateClientPrimaryInfo")]
        public async Task<IActionResult> UpdateClientPrimaryInfo([FromBody] UpdateClientPrimaryInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientList")]
        public async Task<IActionResult> GetClientList([FromBody] GetAllClientListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("DeleteClientListing")]
        public async Task<IActionResult> DeleteClientListing([FromBody] DeleteClientListingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddClientPrimaryCareInfo")]
        public async Task<IActionResult> AddClientPrimaryCareInfo([FromBody] AddClientPrimaryCareInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]

        [Route("AddClientBoardingNotes")]
        public async Task<IActionResult> AddClientBoardingNotes([FromBody] AddClientBoardingNotesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]

        [Route("AddClientAdditionalNotes")]
        public async Task<IActionResult> AddClientAdditionalNotes([FromBody] AddClientAdditionalNotesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]

        [Route("AddClientFunding")]
        public async Task<IActionResult> AddClientFunding([FromBody] AddClientFundingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]

        [Route("DeleteClientFunding")]
        public async Task<IActionResult> DeleteClientFunding([FromBody] DeleteClientFundingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]

        [Route("UpdateClientFunding")]
        public async Task<IActionResult> UpdateClientFunding([FromBody] UpdateClientFundingCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]

        [Route("GetClientFundingtypeList")]
        public async Task<IActionResult> GetClientFundingtypeList([FromBody] GetClientFundingTypeListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]

        [Route("AddClientFundingInfo")]

        public async Task<IActionResult> AddClientFundingInfo([FromBody] AddClientFundingInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientDetails")]
        public async Task<IActionResult> GetClientDetails([FromBody] GetClientDetailQuery model)
        {
            return Ok(await Mediator.Send(model));
        }



        [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
        [HttpPost]
        [Route("GetClientPrimaryInfo")]
        public async Task<IActionResult> GetClientPrimaryInfo([FromBody] GetClientPrimaryInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddClientMedicalHistory")]
        public async Task<IActionResult> AddClientMedicalHistory([FromBody] AddClientMedicalHistoryCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetAllClientMedicalHistory")]
        public async Task<IActionResult> GetAllClientMedicalHistory([FromBody] GetClientAllMedicalHistoryQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientMedicalHistory")]
        public async Task<IActionResult> DeleteClientMedicalHistory([FromBody] DeleteClientMedicalHistoryCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetAllClientProgressNotes")]
        public async Task<IActionResult> GetAllClientProgressNotes([FromBody] GetClientProgressNotesQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientProgressNotes")]
        public async Task<IActionResult> DeleteClientProgressNotes([FromBody] DeleteClientProgressNotesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientProgressNotesItem")]
        public async Task<IActionResult> DeleteClientProgressNotesItem([FromBody] DeleteClientProgressNotesItemCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientProgressNotes")]
        public async Task<IActionResult> AddClientProgressNotes([FromBody] AddClientProgressNotesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientProgressNotesItem")]
        public async Task<IActionResult> AddClientProgressNotesItem([FromBody] AddClientProgressNotesItemCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateClientProgressNotesItem")]
        public async Task<IActionResult> UpdateClientProgressNotesItem([FromBody] UpdateClientProgressNotesItemCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetClientProgressNotes")]
        public async Task<IActionResult> GetClientProgressNotes([FromBody] GetClientProgressNotesQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
        [HttpPost]
        [Route("GetClientProgressNotesList")]
        public async Task<IActionResult> GetClientProgressNotesList([FromBody] GetClientProgressNotesListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientProgressSingleNote")]
        public async Task<IActionResult> GetClientProgressSingleNote([FromBody] GetClientProgressSingleNoteQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetClientMedicalDetails")]
        public async Task<IActionResult> GetClientMedicalDetails([FromBody] GetClientMedicalHistoryQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        //[Authorize(Policy = "RequireAdmin")]
        [Route("AddClientPicInfo")]
        public async Task<IActionResult> AddEmployeePicInfo([FromForm] AddClientPicInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddClientSupportCoordinator")]
        public async Task<IActionResult> AddClientSupportCoordinator([FromBody] AddClientSupportCoordinatorCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientCompliancesInfo")]
        public async Task<IActionResult> AddClientCompliancesInfo([FromForm] AddClientCompliancesDetailsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientCompliancesList")]
        public async Task<IActionResult> GetClientCompliancesList([FromBody] GetClientComplianceDetailsListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateClientCompliancesInfo")]
        public async Task<IActionResult> UpdateClientCompliancesInfo([FromForm] EditClientCompliancesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientComplianceInfo")]
        public async Task<IActionResult> DeleteClientComplianceInfo([FromBody] DeleteClientCompliancesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetAllClientDocumentsList")]
        public async Task<IActionResult> GetAllClientDocumentsList([FromBody] GetAllClientDocumentsListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientDocument")]
        public async Task<IActionResult> DeleteClientDocument([FromBody] DeleteClientDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetClientAccidentIncidentList")]
        public async Task<IActionResult> GetClientAccidentIncidentList([FromBody] GetClientAccidentIncidentListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientAccidentIncidentInfo")]
        public async Task<IActionResult> AddClientAccidentIncidentInfo([FromBody] AddClientAccidentInfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateClientAccidentInfo")]
        public async Task<IActionResult> UpdateClientAccidentInfo([FromBody] UpdateClientAccidentDetailsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientAccidentInfo")]
        public async Task<IActionResult> DeleteClientAccidentInfo([FromBody] DeleteClientAccidentIIncdentnfoCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetAllClientAccidentList")]
        public async Task<IActionResult> GetAllClientAccidentList([FromBody] GetAllClientAccidentListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("GetAllClientNameList")]
        public async Task<IActionResult> GetAllClientNameList()
        {
            return Ok(await Mediator.Send(new GetAllClientNameListQuery() { }));
        }
        [HttpPost]
        [Route("GetClientAccidentDetails")]
        public async Task<IActionResult> GetClientAccidentDetails([FromBody] GetClientAccidentDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientCompliancesDetails")]
        public async Task<IActionResult> GetClientCompliancesDetails([FromBody] GetClientCompliancesDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientMedicalInfo")]
        public async Task<IActionResult> GetClientMedicalInfo([FromBody] GetClientMedicalInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetProgressNotesDetails")]
        public async Task<IActionResult> GetProgressNotesDetails([FromBody] GetProgressNotesDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddGuardianInfo")]
        public async Task<IActionResult> AddGuardianInfo([FromBody] AddClientGuardianCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetOtherContactPerson")]
        public async Task<IActionResult> GetOtherContactPerson([FromBody] GetOtherContactPersonInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientContactOtherPerson")]
        public async Task<IActionResult> DeleteClientContactOtherPerson([FromBody] DeleteOtherContactPersonCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateClientActiveStatus")]
        public async Task<IActionResult> UpdateClientActiveStatus([FromBody] UpdateClientActiveStatusCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddClientCultureNeed")]
        public async Task<IActionResult> AddClientCultureNeed([FromBody] AddClientCultureNeedCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientBehaviourConcern")]
        public async Task<IActionResult> AddClientBehaviourConcern([FromBody] AddClientBehaviourConcernCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientEatingNutrition")]
        public async Task<IActionResult> AddClientEatingNutrition([FromBody] AddClientEatingNutritionCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientLivingArrangement")]
        public async Task<IActionResult> AddClientLivingArrangement([FromBody] AddClientLivingArrangementCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientOtherInformtion")]
        public async Task<IActionResult> AddClientOtherInformtion([FromBody] AddClientOtherInformtionCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientSocialConnections")]
        public async Task<IActionResult> AddClientSocialConnections([FromBody] AddClientSocialConnectionsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddClientPreferences")]
        public async Task<IActionResult> AddClientPreferences([FromBody] AddClientPreferencesCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientProfileDetails")]
        public async Task<IActionResult> GetClientProfileDetails([FromBody] GetClientProfileDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentCategory")]
        public async Task<IActionResult> AddIncidentCategory([FromBody] AddIncidentCategoryCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentDetails")]
        public async Task<IActionResult> AddIncidentDetails([FromBody] AddIncidentDetailsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentDeclaration")]
        public async Task<IActionResult> AddIncidentDeclaration([FromBody] AddIncidentDeclarationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentImmediateAction")]
        public async Task<IActionResult> AddIncidentImmediateAction([FromBody] AddIncidentImmediateActionCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("AddIncidentImpactedPerson")]
        public async Task<IActionResult> AddIncidentImpactedPerson([FromBody] AddIncidentImpactedPersonCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentPrimaryContact")]
        public async Task<IActionResult> AddIncidentPrimaryContact([FromBody] AddIncidentPrimaryContactCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentProviderDetail")]
        public async Task<IActionResult> AddIncidentProviderDetail([FromBody] AddIncidentProviderDetailCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentRiskAssesment")]
        public async Task<IActionResult> AddIncidentRiskAssesment([FromBody] AddIncidentRiskAssesmentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddInciidentSubjectAllegation")]
        public async Task<IActionResult> AddInciidentSubjectAllegation([FromBody] AddIncidentSubjectAllegationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("AddIncidentDocuments")]
        public async Task<IActionResult> AddIncidentDocuments([FromForm] AddIncidentDocumentsCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateIncidentSubjectAllegation")]
        public async Task<IActionResult> UpdateIncidentSubjectAllegation([FromBody] UpdateIncidentSubjectAllegationCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetIncidentDetails")]
        public async Task<IActionResult> GetIncidentDetails([FromBody] GetIncidentDetailsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("UpdateIncidentDocument")]
        public async Task<IActionResult> UpdateIncidentDocument([FromForm] UpdateIncidentDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteIncidentDocument")]
        public async Task<IActionResult> DeleteIncidentDocument([FromBody] DeleteIncidentDocumentCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetIncidentDocumentDetail")]
        public async Task<IActionResult> GetIncidentDocumentDetail([FromBody] GetIncidentDocumentDetailQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
        [HttpPost]
        [Route("GetIncidentImpactedPerson")]
        public async Task<IActionResult> GetIncidentImpactedPerson([FromBody] GetIncidentImpactedPersonQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetIncidentSubjectAllegation")]
        public async Task<IActionResult> GetIncidentSubjectAllegation([FromBody] GetIncidentSubjectAllegationQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
        [HttpPost]
        [Route("GetClientIncidentCategory")]
        public async Task<IActionResult> GetClientIncidentCategory([FromBody] GetClientIncidentCategoryQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
        [HttpPost]
        [Route("GetIncidentProviderInfo")]
        public async Task<IActionResult> GetIncidentProviderInfo([FromBody] GetIncidentProviderInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
        [HttpPost]
        [Route("GetIncidentPrimaryContact")]
        public async Task<IActionResult> GetIncidentPrimaryContact([FromBody] GetIncidentPrimaryContactQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "RequireAdminOrEmployee")]
        [HttpPost]
        [Route("GetAccidentIncidentInfo")]
        public async Task<IActionResult> GetAccidentIncidentInfo([FromBody] GetAccidentIncidentInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetIncidentImmediateAction")]
        public async Task<IActionResult> GetIncidentImmediateAction([FromBody] GetImmediateActionQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetRiskAssesment")]
        public async Task<IActionResult> GetRiskAssesment([FromBody] GetRiskAssesmentQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetIncidentDeclaration")]
        public async Task<IActionResult> GetIncidentDeclaration([FromBody] GetIncidentDeclarationQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientShiftList")]
        public async Task<IActionResult> GetClientShiftList([FromBody] GetClientShiftListQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        //  --------------------Client Module Api's------------------------.

        [HttpPost]
        [Route("GetClientCurrentShift")]
        public async Task<IActionResult> GetClientCurrentShift([FromBody] GetClientCurrentShiftsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetClientAssignedShift")]
        public async Task<IActionResult> GetClientAssignedShift([FromBody] GetClientAssignedShiftsQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("CancelAssignedShift")]
        public async Task<IActionResult> CancelAssignedShift([FromBody] UpdateClientShiftCancelQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        [Route("GetClientCalendarShifts")]
        public async Task<IActionResult> GetClientCalendarShifts([FromBody] GetClientCalendarShifts model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GenerateUserAndPassword")]
        public async Task<IActionResult> GenerateUserAndPassword([FromBody] AddClientSignUpCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("ClientResetPassword")]
        public async Task<IActionResult> ClientResetPassword([FromBody] ClientResetPasswordCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientImpactedPerson")]
        public async Task<IActionResult> GetClientImpactedPerson([FromBody] GetClientImpactedPersonQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("DeleteClientAgreement")]
        public async Task<IActionResult> DeleteClientAgreement([FromBody] DeleteClientAgreementCommand model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("GetClientAgreementInfo")]
        public async Task<IActionResult> GetClientAgreementInfo([FromBody] GetClientAgreementInfoQuery model)
        {
            return Ok(await Mediator.Send(model));
        }

    }

}



