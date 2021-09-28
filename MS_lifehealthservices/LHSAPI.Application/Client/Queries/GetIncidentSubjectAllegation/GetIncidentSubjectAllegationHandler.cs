
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

namespace LHSAPI.Application.Client.Queries.GetIncidentSubjectAllegation
{
    public class GetIncidentSubjectAllegationHandler : IRequestHandler<GetIncidentSubjectAllegationQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetIncidentSubjectAllegationHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetIncidentSubjectAllegationQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientSubjectAllegation _clientDetails = new ClientSubjectAllegation();


               
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
                                                              where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id
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
