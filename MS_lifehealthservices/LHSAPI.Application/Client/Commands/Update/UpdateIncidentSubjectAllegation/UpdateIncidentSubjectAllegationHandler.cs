using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Client.Commands.Create.UpdateIncidentSubjectAllegation
{
    public class UpdateIncidentSubjectAllegationHandler : IRequestHandler<UpdateIncidentSubjectAllegationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public UpdateIncidentSubjectAllegationHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(UpdateIncidentSubjectAllegationCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {
                    var ExistUser = _context.IncidentWorkerAllegation.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser != null)
                    {
                        ExistUser.ClientId = request.ClientId;
                        ExistUser.FirstName = request.FirstName;
                        ExistUser.LastName = request.LastName;
                        ExistUser.Title = request.Title;
                        ExistUser.Gender = request.GenderId;
                        ExistUser.PhoneNo = request.PhoneNo;
                        ExistUser.Email = request.Email;
                        ExistUser.DateOfBirth = request.DateOfBirth;
                        ExistUser.IsSubjectAllegation = request.IsSubjectAllegation;
                        ExistUser.Position = request.Position;
                        ExistUser.DateOfBirth = request.DateOfBirth;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.IncidentWorkerAllegation.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        var PrimaryExistUser = _context.IncidentAllegationPrimaryDisability.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                        if (PrimaryExistUser != null)
                        {
                            foreach (var id in request.PrimaryDisability)
                            {

                                PrimaryExistUser.ClientId = request.ClientId;
                                PrimaryExistUser.PrimaryDisability = id;
                                PrimaryExistUser.UpdateById = await _ISessionService.GetUserId();
                                PrimaryExistUser.UpdatedDate = DateTime.Now;
                                PrimaryExistUser.IsActive = true;
                                _context.IncidentAllegationPrimaryDisability.Update(PrimaryExistUser);
                                await _context.SaveChangesAsync();
                            }
                        }
                        var secondExistUser = _context.IncidentAllegationOtherDisability.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                        if (secondExistUser != null)
                        {
                            foreach (var id in request.OtherDisability)
                            {

                                secondExistUser.ClientId = request.ClientId;
                                secondExistUser.OtherDisability = id;
                                secondExistUser.UpdateById = await _ISessionService.GetUserId();
                                secondExistUser.UpdatedDate = DateTime.Now;
                                secondExistUser.IsActive = true;
                                _context.IncidentAllegationOtherDisability.Update(secondExistUser);
                                await _context.SaveChangesAsync();
                            }
                        }
                        var concernExistUser = _context.IncidentAllegationBehaviour.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                        if (concernExistUser != null)
                        {
                            foreach (var id in request.ConcerBehaviourId)
                            {

                                concernExistUser.ClientId = request.ClientId;
                                concernExistUser.ConcerBehaviourId = id;
                                concernExistUser.UpdateById = await _ISessionService.GetUserId();
                                concernExistUser.UpdatedDate = DateTime.Now;
                                concernExistUser.IsActive = true;
                                _context.IncidentAllegationBehaviour.Update(concernExistUser);
                                await _context.SaveChangesAsync();
                            }
                        }
                        var commExistUser = _context.IncidentAllegationCommunication.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                        if (commExistUser != null)
                        {
                            foreach (var id in request.CommunicationId)
                            {

                                commExistUser.ClientId = request.ClientId;
                                commExistUser.CommunicationId = id;
                                commExistUser.UpdateById = await _ISessionService.GetUserId();
                                commExistUser.UpdatedDate = DateTime.Now;
                                commExistUser.IsActive = true;
                                _context.IncidentAllegationCommunication.Update(commExistUser);
                                await _context.SaveChangesAsync();
                            }
                        }

                        var ExistUserdis = _context.IncidentDisablePersonAllegation.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                        if (ExistUserdis != null)
                        {

                            ExistUserdis.ClientId = request.ClientId;
                            ExistUserdis.FirstName = request.DisableFirstName;
                            ExistUserdis.LastName = request.DisableLastName;
                            ExistUserdis.Title = request.DisableTitle;
                            ExistUserdis.Gender = request.DisableGender;
                            ExistUserdis.PhoneNo = request.DisablePhoneNo;
                            ExistUserdis.Email = request.DisableEmail;
                            ExistUserdis.DateOfBirth = request.DisableDateOfBirth;
                            ExistUserdis.NdisNumber = request.DisableNdisNumber;
                            ExistUserdis.OtherDetail = request.OtherDetail;
                            ExistUserdis.CreatedById = await _ISessionService.GetUserId();
                            ExistUserdis.CreatedDate = DateTime.Now;
                            ExistUserdis.IsActive = true;
                            await _context.IncidentDisablePersonAllegation.AddAsync(ExistUserdis);
                            _context.SaveChanges();
                        }
                        var ExistUserother = _context.IncidentOtherAllegation.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                        if (ExistUserother != null)
                        {

                            ExistUserother.ClientId = request.ClientId;
                            ExistUserother.FirstName = request.DisableFirstName;
                            ExistUserother.LastName = request.DisableLastName;
                            ExistUserother.Title = request.DisableTitle;
                            ExistUserother.Gender = request.DisableGender;
                            ExistUserother.PhoneNo = request.DisablePhoneNo;
                            ExistUserother.Email = request.DisableEmail;
                            ExistUserother.DateOfBirth = request.DisableDateOfBirth;
                            ExistUserother.Relationship = request.OtherRelationship;
                            ExistUserother.CreatedById = await _ISessionService.GetUserId();
                            ExistUserother.CreatedDate = DateTime.Now;
                            ExistUserother.IsActive = true;
                            await _context.IncidentOtherAllegation.AddAsync(ExistUserother);
                            _context.SaveChanges();
                        }
                    }

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
