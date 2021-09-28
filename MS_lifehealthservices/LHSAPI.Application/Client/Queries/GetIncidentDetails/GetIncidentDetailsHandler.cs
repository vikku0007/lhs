
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

namespace LHSAPI.Application.Client.Queries.GetIncidentDetails
{
    public class GetIncidentDetailsHandler : IRequestHandler<GetIncidentDetailsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetIncidentDetailsHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetIncidentDetailsQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                AccidentIncidentmodel _clientDetails = new AccidentIncidentmodel();


                _clientDetails.ClientAccidentProviderInfo = (from client in _dbContext.ClientAccidentProviderInfo
                                                             where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id && client.ShiftId == request.ShiftId
                                                             select new LHSAPI.Application.Client.Models.ClientAccidentProviderInfo
                                                             {
                                                                 Id = client.Id,
                                                                 ClientId = client.ClientId,
                                                                 ReportCompletedBy = client.ReportCompletedBy,
                                                                 ProviderName = client.ProviderName,
                                                                 ProviderregistrationId = client.ProviderregistrationId,
                                                                 ProviderABN = client.ProviderABN,
                                                                 OutletName = client.OutletName,
                                                                 Registrationgroup = client.Registrationgroup,
                                                                 State = client.State,
                                                                 StateName = _dbContext.StandardCode.Where(x => x.ID == client.State).Select(x => x.CodeDescription).FirstOrDefault(),
                                                             }).FirstOrDefault();
                _clientDetails.ClientAccidentPrimaryContact = (from client in _dbContext.ClientAccidentPrimaryContact
                                                               where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id && client.ShiftId == request.ShiftId
                                                               select new LHSAPI.Application.Client.Models.ClientAccidentPrimaryContact
                                                               {
                                                                   Id = client.Id,
                                                                   ClientId = client.ClientId,
                                                                   Title = client.Title,
                                                                   FirstName = client.FirstName,
                                                                   MiddleName = client.MiddleName,
                                                                   LastName = client.LastName,
                                                                   ProviderPosition = client.ProviderPosition,
                                                                   PhoneNo = client.PhoneNo,
                                                                   Email = client.Email,
                                                                   ContactMetod = client.ContactMetod,
                                                                   FullName = client.FirstName + " " + (!string.IsNullOrEmpty(client.MiddleName) ? client.MiddleName : null) + " " + client.LastName
                                                               }).FirstOrDefault();
                _clientDetails.ClientIncidentCategory = (from accident in _dbContext.ClientIncidentCategory
                                                         where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                         select new LHSAPI.Application.Client.Models.ClientIncidentCategory
                                                         {
                                                             Id = accident.Id,
                                                             ClientId = accident.ClientId,
                                                             IsIncidentAnticipated = accident.IsIncidentAnticipated

                                                         }).FirstOrDefault();


                _clientDetails.ClientPrimaryIncidentCategory = (from comminfo in _dbContext.ClientPrimaryIncidentCategory
                                                                where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                                select new LHSAPI.Application.Client.Models.ClientPrimaryIncidentCategory
                                                                {
                                                                    Id = comminfo.Id,
                                                                    ClientId = comminfo.ClientId,
                                                                    PrimaryIncidentId = comminfo.PrimaryIncidentId,
                                                                    PrimaryIncidentName = _dbContext.StandardCode.Where(x => x.ID == comminfo.PrimaryIncidentId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.ClientSecondaryIncidentCategory = (from comminfo in _dbContext.ClientSecondaryIncidentCategory
                                                                  where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                                  select new LHSAPI.Application.Client.Models.ClientSecondaryIncidentCategory
                                                                  {
                                                                      Id = comminfo.Id,
                                                                      ClientId = comminfo.ClientId,
                                                                      SecondaryIncidentId = comminfo.SecondaryIncidentId,
                                                                      SecondaryIncidentName = _dbContext.StandardCode.Where(x => x.ID == comminfo.SecondaryIncidentId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                  }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.ClientIncidentDetails = (from accident in _dbContext.ClientIncidentDetails
                                                        where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                        select new LHSAPI.Application.Client.Models.ClientIncidentDetails
                                                        {
                                                            Id = accident.Id,
                                                            ClientId = accident.ClientId,
                                                            LocationId = accident.LocationId,
                                                            LocationType = accident.LocationType,
                                                            OtherLocation = accident.OtherLocation,
                                                            DateTime = accident.DateTime,
                                                            UnknowndateReason = accident.UnknowndateReason,
                                                            NdisProviderDate = accident.NdisProviderDate,
                                                            NdisProviderTime = accident.NdisProviderTime,
                                                            StartTimeString = accident.NdisProviderDate.Date.Add(accident.NdisProviderTime).ToString("hh:mm tt"),
                                                            AllegtionCircumstances = accident.AllegtionCircumstances,
                                                            IncidentAllegtion = accident.IncidentAllegtion,
                                                            LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == accident.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                            LocationName = _dbContext.StandardCode.Where(x => x.Value == accident.LocationId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                             Address=accident.Address
                                                        }).FirstOrDefault();

                _clientDetails.IncidentImmediateAction = (from accident in _dbContext.IncidentImmediateAction
                                                          where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                          select new LHSAPI.Application.Client.Models.IncidentImmediateAction
                                                          {
                                                              Id = accident.Id,
                                                              ClientId = accident.ClientId,
                                                              IsPoliceInformed = accident.IsPoliceInformed,
                                                              OfficerName = accident.OfficerName,
                                                              PoliceStation = accident.PoliceStation,
                                                              PoliceNo = accident.PoliceNo,
                                                              ProviderPosition = accident.ProviderPosition,
                                                              PhoneNo = accident.PhoneNo,
                                                              IsFamilyAware = accident.IsFamilyAware,
                                                              ContacttoFamily = accident.ContacttoFamily,
                                                              IsUnder18 = accident.IsUnder18,
                                                              ContactChildProtection = accident.ContactChildProtection,
                                                              DisabilityPerson = accident.DisabilityPerson,
                                                              SubjectWorkerAllegation = accident.SubjectWorkerAllegation,
                                                              SubjectDisabilityPerson = accident.SubjectDisabilityPerson

                                                          }).FirstOrDefault();
                _clientDetails.IncidentRiskAssesment = (from accident in _dbContext.IncidentRiskAssesment
                                                        where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                        select new LHSAPI.Application.Client.Models.IncidentRiskAssesment
                                                        {
                                                            Id = accident.Id,
                                                            ClientId = accident.ClientId,
                                                            IsRiskAssesment = accident.IsRiskAssesment,
                                                            RiskAssesmentDate = accident.RiskAssesmentDate,
                                                            RiskDetails = accident.RiskDetails,
                                                            NoRiskAssesmentInfo = accident.NoRiskAssesmentInfo,
                                                            InProgressRisk = accident.InProgressRisk,
                                                            TobeFinished = accident.TobeFinished
                                                        }).FirstOrDefault();
                _clientDetails.IncidentDeclaration = (from accident in _dbContext.IncidentDeclaration
                                                      where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                      select new LHSAPI.Application.Client.Models.IncidentDeclaration
                                                      {
                                                          Id = accident.Id,
                                                          ClientId = accident.ClientId,
                                                          Name = accident.Name,
                                                          PositionAtOrganisation = accident.PositionAtOrganisation,
                                                          Date = accident.Date,
                                                          IsDeclaration = accident.IsDeclaration,
                                                      }).FirstOrDefault();
                _clientDetails.IncidentImpactedPerson = (from accident in _dbContext.IncidentImpactedPerson
                                                         where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                         select new LHSAPI.Application.Client.Models.IncidentImpactedPerson
                                                         {
                                                             Id = accident.Id,
                                                             ClientId = accident.ClientId,
                                                             FirstName = accident.FirstName,
                                                             MiddleName = accident.MiddleName,
                                                             LastName = accident.LastName,
                                                             NdisParticipantNo = accident.NdisParticipantNo,
                                                             GenderId = accident.GenderId,
                                                             DateOfBirth = accident.DateOfBirth,
                                                             PhoneNo = accident.PhoneNo,
                                                             Email = accident.Email,
                                                             Title = accident.Title,
                                                             OtherDetail = accident.OtherDetail,
                                                             FullName = accident.FirstName + " " + (!string.IsNullOrEmpty(accident.MiddleName) ? accident.MiddleName : null) + " " + accident.LastName,
                                                             GenderName = _dbContext.StandardCode.Where(x => x.ID == accident.GenderId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                         }).FirstOrDefault();
                _clientDetails.IncidentPrimaryDisability = (from comminfo in _dbContext.IncidentPrimaryDisability
                                                            where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                            select new LHSAPI.Application.Client.Models.IncidentPrimaryDisability
                                                            {
                                                                Id = comminfo.Id,
                                                                ClientId = comminfo.ClientId,
                                                                PrimaryDisability = comminfo.PrimaryDisability,
                                                                PrimaryDisabilityName = _dbContext.StandardCode.Where(x => x.ID == comminfo.PrimaryDisability).Select(x => x.CodeDescription).FirstOrDefault(),
                                                            }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentOtherDisability = (from comminfo in _dbContext.IncidentOtherDisability
                                                          where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                          select new LHSAPI.Application.Client.Models.IncidentOtherDisability
                                                          {
                                                              Id = comminfo.Id,
                                                              ClientId = comminfo.ClientId,
                                                              OtherDisabilityId = comminfo.OtherDisability,
                                                              SecondaryDisabilityName = _dbContext.StandardCode.Where(x => x.ID == comminfo.OtherDisability).Select(x => x.CodeDescription).FirstOrDefault(),
                                                          }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentConcernBehaviour = (from comminfo in _dbContext.IncidentConcernBehaviour
                                                           where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                           select new LHSAPI.Application.Client.Models.IncidentConcernBehaviour
                                                           {
                                                               Id = comminfo.Id,
                                                               ClientId = comminfo.ClientId,
                                                               ConcerBehaviourId = comminfo.ConcerBehaviourId,
                                                               ConcernBehaviourName = _dbContext.StandardCode.Where(x => x.ID == comminfo.ConcerBehaviourId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                           }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.ClientIncidentCommunication = (from comminfo in _dbContext.ClientIncidentCommunication
                                                              where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                              select new LHSAPI.Application.Client.Models.ClientIncidentCommunication
                                                              {
                                                                  Id = comminfo.Id,
                                                                  ClientId = comminfo.ClientId,
                                                                  CommunicationId = comminfo.CommunicationId,
                                                                  CommunicationName = _dbContext.StandardCode.Where(x => x.ID == comminfo.CommunicationId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                              }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentWorkerAllegation = (from accident in _dbContext.IncidentWorkerAllegation
                                                           where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                           select new LHSAPI.Application.Client.Models.IncidentWorkerAllegation
                                                           {
                                                               Id = accident.Id,
                                                               ClientId = accident.ClientId,
                                                               FirstName = accident.FirstName,
                                                               LastName = accident.LastName,
                                                               Title = accident.Title,
                                                               Position = accident.Position,
                                                               Gender = accident.Gender,
                                                               DateOfBirth = accident.DateOfBirth,
                                                               PhoneNo = accident.PhoneNo,
                                                               Email = accident.Email,
                                                               IsSubjectAllegation = accident.IsSubjectAllegation,
                                                               SubjectFullName = accident.FirstName  + " " + accident.LastName,
                                                               GenderName = _dbContext.StandardCode.Where(x => x.ID == accident.Gender).Select(x => x.CodeDescription).FirstOrDefault(),

                                                           }).FirstOrDefault();
                _clientDetails.IncidentDisablePersonAllegation = (from accident in _dbContext.IncidentDisablePersonAllegation
                                                                  where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                                  select new LHSAPI.Application.Client.Models.IncidentDisablePersonAllegation
                                                                  {
                                                                      Id = accident.Id,
                                                                      ClientId = accident.ClientId,
                                                                      FirstName = accident.FirstName,
                                                                      LastName = accident.LastName,
                                                                      Title = accident.Title,
                                                                      NdisNumber = accident.NdisNumber,
                                                                      Gender = accident.Gender,
                                                                      DateOfBirth = accident.DateOfBirth,
                                                                      PhoneNo = accident.PhoneNo,
                                                                      Email = accident.Email,
                                                                      OtherDetail= accident.OtherDetail,
                                                                      DisableFullName = accident.FirstName + " " + accident.LastName,
                                                                      GenderName = _dbContext.StandardCode.Where(x => x.ID == accident.Gender).Select(x => x.CodeDescription).FirstOrDefault(),

                                                                  }).FirstOrDefault();
                _clientDetails.IncidentOtherAllegation = (from accident in _dbContext.IncidentOtherAllegation
                                                          where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                          select new LHSAPI.Application.Client.Models.IncidentOtherAllegation
                                                          {
                                                              Id = accident.Id,
                                                              ClientId = accident.ClientId,
                                                              FirstName = accident.FirstName,
                                                              LastName = accident.LastName,
                                                              Title = accident.Title,
                                                              Relationship = accident.Relationship,
                                                              Gender = accident.Gender,
                                                              DateOfBirth = accident.DateOfBirth,
                                                              PhoneNo = accident.PhoneNo,
                                                              Email = accident.Email,
                                                              OtherFullName = accident.FirstName + " " + accident.LastName,
                                                              GenderName = _dbContext.StandardCode.Where(x => x.ID == accident.Gender).Select(x => x.CodeDescription).FirstOrDefault(),

                                                          }).FirstOrDefault();
                _clientDetails.IncidentAllegationPrimaryDisability = (from comminfo in _dbContext.IncidentAllegationPrimaryDisability
                                                                      where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                                      select new LHSAPI.Application.Client.Models.IncidentAllegationPrimaryDisability
                                                                      {
                                                                          Id = comminfo.Id,
                                                                          ClientId = comminfo.ClientId,
                                                                          CodeId = comminfo.PrimaryDisability,
                                                                          CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.PrimaryDisability).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                      }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentAllegationOtherDisability = (from comminfo in _dbContext.IncidentAllegationOtherDisability
                                                                    where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                                    select new LHSAPI.Application.Client.Models.IncidentAllegationOtherDisability
                                                                    {
                                                                        Id = comminfo.Id,
                                                                        ClientId = comminfo.ClientId,
                                                                        CodeId = comminfo.OtherDisability,
                                                                        CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.OtherDisability).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                    }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentAllegationBehaviour = (from comminfo in _dbContext.IncidentAllegationBehaviour
                                                              where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                              select new LHSAPI.Application.Client.Models.IncidentAllegationBehaviour
                                                              {
                                                                  Id = comminfo.Id,
                                                                  ClientId = comminfo.ClientId,
                                                                  CodeId = comminfo.ConcerBehaviourId,
                                                                  CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.ConcerBehaviourId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                              }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentAllegationCommunication = (from comminfo in _dbContext.IncidentAllegationCommunication
                                                                  where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                                  select new LHSAPI.Application.Client.Models.IncidentAllegationCommunication
                                                                  {
                                                                      Id = comminfo.Id,
                                                                      ClientId = comminfo.ClientId,
                                                                      CodeId = comminfo.CommunicationId,
                                                                      CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.CommunicationId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                  }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentDocumentDetailModel = (from accident in _dbContext.IncidentDocumentDetails
                                                              where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                              select new LHSAPI.Application.Client.Models.IncidentDocumentDetailModel
                                                                  {
                                                                      Id = accident.Id,
                                                                      ClientId = accident.ClientId,
                                                                      DocumentName = accident.DocumentName,
                                                                      FileName=accident.FileName

                                                                  }).OrderByDescending(x => x.Id).ToList();
                if (_clientDetails.ClientAccidentProviderInfo == null) _clientDetails.ClientAccidentProviderInfo = new LHSAPI.Application.Client.Models.ClientAccidentProviderInfo();
                if (_clientDetails.ClientAccidentPrimaryContact == null) _clientDetails.ClientAccidentPrimaryContact = new LHSAPI.Application.Client.Models.ClientAccidentPrimaryContact();
                if (_clientDetails.ClientIncidentCategory == null) _clientDetails.ClientIncidentCategory = new LHSAPI.Application.Client.Models.ClientIncidentCategory();
                if (_clientDetails.ClientPrimaryIncidentCategory == null) _clientDetails.ClientPrimaryIncidentCategory = new List<LHSAPI.Application.Client.Models.ClientPrimaryIncidentCategory>();
                if (_clientDetails.ClientSecondaryIncidentCategory == null) _clientDetails.ClientSecondaryIncidentCategory = new List<LHSAPI.Application.Client.Models.ClientSecondaryIncidentCategory>();
                if (_clientDetails.ClientIncidentDetails == null) _clientDetails.ClientIncidentDetails = new LHSAPI.Application.Client.Models.ClientIncidentDetails();
                if (_clientDetails.IncidentImmediateAction == null) _clientDetails.IncidentImmediateAction = new LHSAPI.Application.Client.Models.IncidentImmediateAction();
                if (_clientDetails.IncidentRiskAssesment == null) _clientDetails.IncidentRiskAssesment = new LHSAPI.Application.Client.Models.IncidentRiskAssesment();
                if (_clientDetails.IncidentImpactedPerson == null) _clientDetails.IncidentImpactedPerson = new LHSAPI.Application.Client.Models.IncidentImpactedPerson();
                if (_clientDetails.IncidentDeclaration == null) _clientDetails.IncidentDeclaration = new LHSAPI.Application.Client.Models.IncidentDeclaration();
                if (_clientDetails.IncidentDocumentDetailModel == null) _clientDetails.IncidentDocumentDetailModel = new List<LHSAPI.Application.Client.Models.IncidentDocumentDetailModel>();
                if (_clientDetails.IncidentPrimaryDisability == null) _clientDetails.IncidentPrimaryDisability = new List<LHSAPI.Application.Client.Models.IncidentPrimaryDisability>();
                if (_clientDetails.IncidentOtherDisability == null) _clientDetails.IncidentOtherDisability = new List<LHSAPI.Application.Client.Models.IncidentOtherDisability>();
                if (_clientDetails.ClientIncidentCommunication == null) _clientDetails.ClientIncidentCommunication = new List<LHSAPI.Application.Client.Models.ClientIncidentCommunication>();
                if (_clientDetails.IncidentConcernBehaviour == null) _clientDetails.IncidentConcernBehaviour = new List<LHSAPI.Application.Client.Models.IncidentConcernBehaviour>();
                if (_clientDetails.IncidentWorkerAllegation == null) _clientDetails.IncidentWorkerAllegation = new LHSAPI.Application.Client.Models.IncidentWorkerAllegation();
                if (_clientDetails.IncidentDisablePersonAllegation == null) _clientDetails.IncidentDisablePersonAllegation = new LHSAPI.Application.Client.Models.IncidentDisablePersonAllegation();
                if (_clientDetails.IncidentOtherAllegation == null) _clientDetails.IncidentOtherAllegation = new LHSAPI.Application.Client.Models.IncidentOtherAllegation();
                if (_clientDetails.IncidentAllegationPrimaryDisability == null) _clientDetails.IncidentAllegationPrimaryDisability = new List<LHSAPI.Application.Client.Models.IncidentAllegationPrimaryDisability>();
                if (_clientDetails.IncidentAllegationOtherDisability == null) _clientDetails.IncidentAllegationOtherDisability = new List<LHSAPI.Application.Client.Models.IncidentAllegationOtherDisability>();
                if (_clientDetails.IncidentAllegationBehaviour == null) _clientDetails.IncidentAllegationBehaviour = new List<LHSAPI.Application.Client.Models.IncidentAllegationBehaviour>();
                if (_clientDetails.IncidentAllegationCommunication == null) _clientDetails.IncidentAllegationCommunication = new List<LHSAPI.Application.Client.Models.IncidentAllegationCommunication>();
                response.SuccessWithOutMessage(_clientDetails);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        #endregion
    }
}
