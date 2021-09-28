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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentImpactedPerson
{
    public class AddIncidentImpactedPersonHandler : IRequestHandler<AddIncidentImpactedPersonCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddIncidentImpactedPersonHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentImpactedPersonCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
               
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                      var ExistUser = _context.IncidentImpactedPerson.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                    
                    if (ExistUser == null)
                    {
                        IncidentImpactedPerson user = new IncidentImpactedPerson();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.FirstName = request.FirstName;
                        user.MiddleName = request.MiddleName;
                        user.LastName = request.LastName;
                        user.NdisParticipantNo = request.NdisParticipantNo;
                        user.GenderId = request.GenderId;
                        user.PhoneNo = request.PhoneNo;
                        user.Email = request.Email;
                        user.DateOfBirth = request.DateOfBirth;
                        user.OtherDetail = request.OtherDetail;
                        user.Title = request.Title;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.IncidentImpactedPerson.AddAsync(user);
                        _context.SaveChanges();

                        foreach (var id in request.PrimaryDisability)
                        {
                            IncidentPrimaryDisability comm = new IncidentPrimaryDisability();
                            comm.ClientId = request.ClientId;
                            comm.EmployeeId = request.EmployeeId;

                            comm.ShiftId = request.ShiftId; 
                            comm.ImpactPersonId = user.Id;
                            comm.PrimaryDisability = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.IncidentPrimaryDisability.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        foreach (var id in request.OtherDisability)
                        {
                            IncidentOtherDisability comm = new IncidentOtherDisability();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.ImpactPersonId = user.Id;
                            comm.OtherDisability = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.IncidentOtherDisability.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        foreach (var id in request.ConcerBehaviourId)
                        {
                            IncidentConcernBehaviour comm = new IncidentConcernBehaviour();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.ImpactPersonId = user.Id;
                            comm.ConcerBehaviourId = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.IncidentConcernBehaviour.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        foreach (var id in request.CommunicationId)
                        {
                            ClientIncidentCommunication comm = new ClientIncidentCommunication();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.ImpactPersonId = user.Id;
                            comm.CommunicationId = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.ClientIncidentCommunication.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        response.Success(user);
                    }

                    else
                    {
                        ExistUser.FirstName = request.FirstName;
                        ExistUser.MiddleName = request.MiddleName;
                        ExistUser.LastName = request.LastName;
                        ExistUser.NdisParticipantNo = request.NdisParticipantNo;
                        ExistUser.GenderId = request.GenderId;
                        ExistUser.PhoneNo = request.PhoneNo;
                        ExistUser.Email = request.Email;
                        ExistUser.DateOfBirth = request.DateOfBirth;
                        ExistUser.OtherDetail = request.OtherDetail;
                        ExistUser.Title = request.Title;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.IncidentImpactedPerson.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        _context.IncidentPrimaryDisability.RemoveRange(_context.IncidentPrimaryDisability.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                        _context.IncidentOtherDisability.RemoveRange(_context.IncidentOtherDisability.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                        _context.IncidentConcernBehaviour.RemoveRange(_context.IncidentConcernBehaviour.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                        _context.ClientIncidentCommunication.RemoveRange(_context.ClientIncidentCommunication.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                        await _context.SaveChangesAsync();
                        foreach (var id in request.PrimaryDisability)
                            {
                                IncidentPrimaryDisability comm = new IncidentPrimaryDisability();
                                comm.ClientId = request.ClientId;
                                comm.PrimaryDisability = id;
                                comm.ShiftId = request.ShiftId;
                                comm.EmployeeId = request.EmployeeId;
                                comm.ImpactPersonId = ExistUser.Id;
                                comm.CreatedById = await _ISessionService.GetUserId();
                                comm.CreatedDate = DateTime.Now;
                                comm.IsActive = true;
                                await _context.IncidentPrimaryDisability.AddAsync(comm);

                            }
                           
                            foreach (var id in request.OtherDisability)
                            {
                                IncidentOtherDisability comm1 = new IncidentOtherDisability();
                                comm1.ClientId = request.ClientId;
                                comm1.ShiftId = request.ShiftId;
                                comm1.EmployeeId = request.EmployeeId;
                                comm1.OtherDisability = id;
                                comm1.ImpactPersonId = ExistUser.Id;
                                comm1.CreatedById = await _ISessionService.GetUserId();
                                comm1.CreatedDate = DateTime.Now;
                                comm1.IsActive = true;
                                await _context.IncidentOtherDisability.AddAsync(comm1);

                            }
                           
                       
                            foreach (var id in request.ConcerBehaviourId)
                            {
                                IncidentConcernBehaviour comm2 = new IncidentConcernBehaviour();
                                comm2.ClientId = request.ClientId;
                                comm2.ConcerBehaviourId = id;
                                comm2.ShiftId = request.ShiftId;
                                comm2.EmployeeId = request.EmployeeId;
                                comm2.ImpactPersonId = ExistUser.Id;
                                comm2.CreatedById = await _ISessionService.GetUserId();
                                comm2.CreatedDate = DateTime.Now;
                                comm2.IsActive = true;
                                await _context.IncidentConcernBehaviour.AddAsync(comm2);

                            }
                           
                     
                            foreach (var id in request.CommunicationId)
                            {
                                ClientIncidentCommunication comm3 = new ClientIncidentCommunication();
                                comm3.ClientId = request.ClientId;
                                comm3.CommunicationId = id;
                                comm3.ImpactPersonId = ExistUser.Id;
                                comm3.ShiftId = request.ShiftId;
                                comm3.EmployeeId = request.EmployeeId;
                                comm3.CreatedById = await _ISessionService.GetUserId();
                                comm3.CreatedDate = DateTime.Now;
                                comm3.IsActive = true;
                                await _context.ClientIncidentCommunication.AddAsync(comm3);

                            }
                        _context.SaveChanges();
                    }
                    response.Update(ExistUser);
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
