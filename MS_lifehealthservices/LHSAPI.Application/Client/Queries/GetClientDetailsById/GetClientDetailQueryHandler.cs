
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

namespace LHSAPI.Application.Client.Queries.GetClientDetailsById
{
    public class GetClientDetailQueryHandler : IRequestHandler<GetClientDetailQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientDetailQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetClientDetailQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientDetails _clientDetails = new ClientDetails();

                var clientList = _dbContext.ClientPrimaryInfo.Where(x => x.IsDeleted == false &&  x.Id == request.Id).FirstOrDefault();
                if (clientList != null && clientList.Id > 0)
                {


                    var ClientBoadingNotes = _dbContext.ClientBoadingNotes.Where(x => x.IsDeleted == false && x.IsActive && x.ClientId == request.Id).ToList();
                    var ClientAdditionalNotes = _dbContext.ClientAdditionalNotes.Where(x => x.IsDeleted == false && x.IsActive && x.ClientId == request.Id).ToList();
                    var ClientFundingInfo = _dbContext.ClientFundingInfo.Where(x => x.IsDeleted == false && x.IsActive && x.ClientId == request.Id).ToList();
                    var ClientPrimaryCareInfo = _dbContext.ClientPrimaryCareInfo.Where(x => x.IsDeleted == false && x.IsActive && x.ClientId == request.Id).ToList();
                    var ClientprimaryInfo = _dbContext.ClientPrimaryInfo.Where(x => x.IsDeleted == false  && x.Id == request.Id).ToList();
                    var ClientSupportCoordinatorInfo = _dbContext.ClientSupportCoordinatorInfo.Where(x => x.IsDeleted == false && x.IsActive && x.ClientId == request.Id).ToList();
                    var imageUrl = _dbContext.ClientPicInfo.Where(x => x.ClientId == request.Id).Select(x => x.Path).Any() ? _dbContext.ClientPicInfo.Where(x => x.ClientId == request.Id).Select(x => x.Path).FirstOrDefault() : null; ;
                    var ClientGuardianInfo = _dbContext.ClientGuardianInfo.Where(x => x.IsDeleted == false && x.IsActive && x.ClientId == request.Id).ToList();
                    if (_clientDetails.ClientBoadingNotes == null) _clientDetails.ClientBoadingNotes = new Models.ClientBoadingNote();
                    if (_clientDetails.ClientAdditionalNote == null) _clientDetails.ClientAdditionalNote = new Models.ClientAdditionalNote();
                   // if (_clientDetails.ClientFundingInfo == null) _clientDetails.ClientFundingInfo = new Models.ClientFundingInfo();
                 //   if (_clientDetails.ClientPrimaryCareInfo == null) _clientDetails.ClientPrimaryCareInfo = new Models.ClientPrimaryCareInfo();
                    if (clientList != null)
                    {
                        foreach (var item in ClientprimaryInfo)
                        {
              ClientPrimaryInfo primaryCareInfo = new ClientPrimaryInfo
              {
                Id = item.ClientId,
                Salutation = item.Salutation,
                FirstName = item.FirstName,
                LastName = item.LastName,
                MiddleName = item.MiddleName,
                EmailId = item.EmailId,
                DateOfBirth = item.DateOfBirth,
                MaritalStatus = item.MaritalStatus,
                MobileNo = item.MobileNo,
                Address = item.Address,
                LocationId = item.LocationId,
                Gender = item.Gender,
                Age = DateTime.Today.Year - Convert.ToDateTime(item.DateOfBirth).Year,
                GenderName = _dbContext.StandardCode.Where(x => x.ID == item.Gender).Select(x => x.CodeDescription).FirstOrDefault(),
                SalutationName = _dbContext.StandardCode.Where(x => x.ID == item.Salutation).Select(x => x.CodeDescription).FirstOrDefault(),
                LocationName = item.OtherLocation != "" ? item.OtherLocation : _dbContext.Location.Where(x => x.LocationId == item.LocationId).Select(x => x.Name).FirstOrDefault(),
                ServiceTypeName = _dbContext.StandardCode.Where(x => x.ID == item.ServiceType).Select(x => x.CodeDescription).FirstOrDefault(),
                ServiceType = item.ServiceType,
                NDIS = item.NDIS,
                ClientId = item.ClientId,
                ImageUrl = imageUrl,
                LocationType = item.LocationType,
                LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == item.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                OtherLocation = item.OtherLocation,
                UserName = item.UserName
                            };
                            _clientDetails.ClientPrimaryInfo = primaryCareInfo;
                        }
                    }
                    _clientDetails.ClientPrimaryCareInfo = (from emp in _dbContext.ClientPrimaryCareInfo
                                                          where emp.IsActive == true && emp.IsDeleted == false && emp.ClientId == request.Id
                                                          select new LHSAPI.Application.Client.Models.ClientPrimaryCareInfo
                                                          {
                                                              Id = emp.Id,
                                                              ClientId = emp.ClientId,
                                                              Name = emp.FirstName + " " +
                                      (emp.MiddleName == null ? "" : emp.MiddleName) + " " + emp.LastName,
                                                              RelationShip = emp.RelationShip,
                                                              ContactNo = emp.ContactNo,
                                                              Email = emp.Email,
                                                              PhoneNo = emp.PhoneNo,
                                                              RelationShipName = _dbContext.StandardCode.Where(x => x.ID == emp.RelationShip).Select(x => x.CodeDescription).FirstOrDefault(),
                                                              MiddleName = emp.MiddleName,
                                                              LastName = emp.LastName,
                                                              FirstName = emp.FirstName,
                                                              Gender = emp.Gender
                                                          }).ToList();
                    //if (ClientPrimaryCareInfo != null)
                    //{
                    //    foreach (var item in ClientPrimaryCareInfo)
                    //    {
                    //        ClientPrimaryCareInfo primaryCareInfo = new ClientPrimaryCareInfo
                    //        {
                    //            ClientId = item.ClientId,
                    //            Name = item.FirstName + " " +
                    //                  (item.MiddleName == null ? "" : item.MiddleName) + " " + item.LastName,
                    //            RelationShip = item.RelationShip,
                    //            ContactNo = item.ContactNo,
                    //            Email = item.Email,
                    //            PhoneNo = item.PhoneNo,
                    //            RelationShipName = _dbContext.StandardCode.Where(x => x.ID == item.RelationShip).Select(x => x.CodeDescription).FirstOrDefault(),
                    //            MiddleName = item.MiddleName,
                    //            LastName = item.LastName,
                    //            FirstName = item.FirstName
                    //        };
                    //        _clientDetails.ClientPrimaryCareInfo = primaryCareInfo;
                    //    }
                    //}
                    if (ClientGuardianInfo != null)
                    {
                        foreach (var item in ClientGuardianInfo)
                        {
                            ClientGuardianModels primaryCareInfo = new ClientGuardianModels
                            {
                                ClientId = item.ClientId,
                                Name = item.FirstName + " " +
                                      (item.MiddleName == null ? "" : item.MiddleName) + " " + item.LastName,
                                RelationShip = item.RelationShip,
                                ContactNo = item.ContactNo,
                                Email = item.Email,
                                PhoneNo = item.PhoneNo,
                                MiddleName = item.MiddleName,
                                LastName = item.LastName,
                                FirstName = item.FirstName
                            };
                            _clientDetails.ClientGuardianModels = primaryCareInfo;
                        }
                    }

                    if (ClientBoadingNotes != null)
                    {
                        foreach (var item in ClientBoadingNotes)
                        {
                            ClientBoadingNote OnBoadingnote = new ClientBoadingNote
                            {
                                Id = item.Id,
                                CareNote = item.CareNote,
                               
                            };
                            _clientDetails.ClientBoadingNotes = OnBoadingnote;
                        }

                    }
                    if (ClientAdditionalNotes != null)
                    {
                        foreach (var item in ClientAdditionalNotes)
                        {
                            ClientAdditionalNote AdditionalNote = new ClientAdditionalNote
                            {
                                Id = item.ClientId,
                                PublicNote = item.PublicNote,
                                PrivateNote = item.PrivateNote
                            };
                            _clientDetails.ClientAdditionalNote = AdditionalNote;
                        }
                    }
                   
                    if (ClientSupportCoordinatorInfo != null)
                    {
                        foreach (var item in ClientSupportCoordinatorInfo)
                        {
                            ClientSupportCoordinatorModel SupportcoordinatorInfo = new ClientSupportCoordinatorModel
                            {
                                ClientId = item.ClientId,
                                FirstName=item.FirstName,
                                MiddleName=item.MiddleName,
                                LastName=item.LastName,
                                Name = item.FirstName + " " +
                                      (item.MiddleName == null ? "" : item.MiddleName) + " " + item.LastName,
                                Relationship =item.Relationship,
                                OtherRelation=item.OtherRelation,
                                Gender = item.Gender,
                                MobileNo = item.MobileNo,
                                EmailId = item.EmailId,
                                PhoneNo = item.PhoneNo,
                                Salutation=item.Salutation,
                                GenderName = _dbContext.StandardCode.Where(x => x.ID == item.Gender).Select(x => x.CodeDescription).FirstOrDefault(),
                                SalutationName = _dbContext.StandardCode.Where(x => x.ID == item.Salutation).Select(x => x.CodeDescription).FirstOrDefault(),
                                RelationShipName = _dbContext.StandardCode.Where(x => x.ID == item.Relationship).Select(x => x.CodeDescription).FirstOrDefault(),
                            };
                            _clientDetails.ClientSupportCoordinatorModel = SupportcoordinatorInfo;
                        }
                    }

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
