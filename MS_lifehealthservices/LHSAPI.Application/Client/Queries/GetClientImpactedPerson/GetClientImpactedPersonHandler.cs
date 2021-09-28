
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

namespace LHSAPI.Application.Client.Queries.GetClientImpactedPerson
{
    public class GetClientImpactedPersonHandler : IRequestHandler<GetClientImpactedPersonQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientImpactedPersonHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetClientImpactedPersonQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientntImpactedPersonDisability _clientDetails = new ClientntImpactedPersonDisability();
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
                                                            select new LHSAPI.Application.Client.Models.PrimaryDisabilityModel
                                                            {
                                                                Id = comminfo.Id,
                                                                ClientId = comminfo.ClientId,
                                                                CodeId = comminfo.PrimaryDisability,
                                                                CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.PrimaryDisability).Select(x => x.CodeDescription).FirstOrDefault(),
                                                            }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentOtherDisability = (from comminfo in _dbContext.IncidentOtherDisability
                                                          where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                          select new LHSAPI.Application.Client.Models.PrimaryDisabilityModel
                                                          {
                                                              Id = comminfo.Id,
                                                              ClientId = comminfo.ClientId,
                                                              CodeId = comminfo.OtherDisability,
                                                              CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.OtherDisability).Select(x => x.CodeDescription).FirstOrDefault(),
                                                          }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.IncidentConcernBehaviour = (from comminfo in _dbContext.IncidentConcernBehaviour
                                                           where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                           select new LHSAPI.Application.Client.Models.PrimaryDisabilityModel
                                                           {
                                                               Id = comminfo.Id,
                                                               ClientId = comminfo.ClientId,
                                                               CodeId = comminfo.ConcerBehaviourId,
                                                               CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.ConcerBehaviourId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                           }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.ClientIncidentCommunication = (from comminfo in _dbContext.ClientIncidentCommunication
                                                              where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                              select new LHSAPI.Application.Client.Models.PrimaryDisabilityModel
                                                              {
                                                                  Id = comminfo.Id,
                                                                  ClientId = comminfo.ClientId,
                                                                  CodeId = comminfo.CommunicationId,
                                                                  CodeName = _dbContext.StandardCode.Where(x => x.ID == comminfo.CommunicationId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                              }).OrderByDescending(x => x.Id).ToList();

                if (_clientDetails.IncidentImpactedPerson == null) _clientDetails.IncidentImpactedPerson = new LHSAPI.Application.Client.Models.IncidentImpactedPerson();
                if (_clientDetails.IncidentPrimaryDisability == null) _clientDetails.IncidentPrimaryDisability = new List<LHSAPI.Application.Client.Models.PrimaryDisabilityModel>();
                if (_clientDetails.IncidentOtherDisability == null) _clientDetails.IncidentOtherDisability = new List<LHSAPI.Application.Client.Models.PrimaryDisabilityModel>();
                if (_clientDetails.ClientIncidentCommunication == null) _clientDetails.ClientIncidentCommunication = new List<LHSAPI.Application.Client.Models.PrimaryDisabilityModel>();
                if (_clientDetails.IncidentConcernBehaviour == null) _clientDetails.IncidentConcernBehaviour = new List<LHSAPI.Application.Client.Models.PrimaryDisabilityModel>();
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
