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

namespace LHSAPI.Application.Client.Commands.Create.AddInciidentSubjectAllegation
{
    public class AddIncidentSubjectAllegationHandler : IRequestHandler<AddIncidentSubjectAllegationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddIncidentSubjectAllegationHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentSubjectAllegationCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
               
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                       var ExistUser = _context.IncidentWorkerAllegation.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                  
                    if (ExistUser == null)
                    {
                        IncidentWorkerAllegation user = new IncidentWorkerAllegation();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.IsSubjectAllegation = request.IsSubjectAllegation;
                        user.FirstName = request.FirstName;
                        user.LastName = request.LastName;
                        user.Title = request.Title;
                        user.Gender = request.GenderId;
                        user.PhoneNo = request.PhoneNo;
                        user.Email = request.Email;
                        user.DateOfBirth = request.DateOfBirth;
                        user.Position = request.Position;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.IncidentWorkerAllegation.AddAsync(user);
                        _context.SaveChanges();

                        IncidentDisablePersonAllegation userdis = new IncidentDisablePersonAllegation();
                        userdis.ClientId = request.ClientId;
                        userdis.ShiftId = request.ShiftId;
                        userdis.EmployeeId = request.EmployeeId;
                        userdis.FirstName = request.DisableFirstName;
                        userdis.LastName = request.DisableLastName;
                        userdis.Title = request.DisableTitle;
                        userdis.Gender = request.DisableGender;
                        userdis.PhoneNo = request.DisablePhoneNo;
                        userdis.Email = request.DisableEmail;
                        userdis.DateOfBirth = request.DisableDateOfBirth;
                        userdis.NdisNumber = request.DisableNdisNumber;
                        userdis.OtherDetail = request.OtherDetail;
                        userdis.CreatedById = await _ISessionService.GetUserId();
                        userdis.CreatedDate = DateTime.Now;
                        userdis.IsActive = true;
                        await _context.IncidentDisablePersonAllegation.AddAsync(userdis);
                        _context.SaveChanges();

                        IncidentOtherAllegation userother = new IncidentOtherAllegation();
                        userother.ClientId = request.ClientId;
                        userother.ShiftId = request.ShiftId;
                        userother.EmployeeId = request.EmployeeId;
                        userother.FirstName = request.OtherFirstName;
                        userother.LastName = request.OtherLastName;
                        userother.Title = request.OtherTitle;
                        userother.Gender = request.OtherGender;
                        userother.PhoneNo = request.OtherPhoneNo;
                        userother.Email = request.OtherEmail;
                        userother.DateOfBirth = request.OtherDateOfBirth;
                        userother.Relationship = request.OtherRelationship;
                        userother.CreatedById = await _ISessionService.GetUserId();
                        userother.CreatedDate = DateTime.Now;
                        userother.IsActive = true;
                        await _context.IncidentOtherAllegation.AddAsync(userother);
                        _context.SaveChanges();

                        foreach (var id in request.PrimaryDisability)
                        {
                            IncidentAllegationPrimaryDisability comm = new IncidentAllegationPrimaryDisability();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.DisableAllegationId = user.Id;
                            comm.PrimaryDisability = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.IncidentAllegationPrimaryDisability.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        foreach (var id in request.OtherDisability)
                        {
                            IncidentAllegationOtherDisability comm = new IncidentAllegationOtherDisability();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.DisableAllegationId = user.Id;
                            comm.OtherDisability = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.IncidentAllegationOtherDisability.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        foreach (var id in request.ConcerBehaviourId)
                        {
                            IncidentAllegationBehaviour comm = new IncidentAllegationBehaviour();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.DisableAllegationId = user.Id;
                            comm.ConcerBehaviourId = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.IncidentAllegationBehaviour.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        foreach (var id in request.CommunicationId)
                        {
                            IncidentAllegationCommunication comm = new IncidentAllegationCommunication();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.DisableAllegationId = user.Id;
                            comm.CommunicationId = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.IncidentAllegationCommunication.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        response.Success(user);
                    }

                    else
                    {
                        var ExistUseredit = _context.IncidentWorkerAllegation.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId);
                        if (ExistUseredit != null)
                        {
                            ExistUseredit.ClientId = request.ClientId;
                            ExistUseredit.FirstName = request.FirstName;
                            ExistUseredit.LastName = request.LastName;
                            ExistUseredit.Title = request.Title;
                            ExistUseredit.Gender = request.GenderId;
                            ExistUseredit.PhoneNo = request.PhoneNo;
                            ExistUseredit.Email = request.Email;
                            ExistUseredit.DateOfBirth = request.DateOfBirth;
                            ExistUseredit.IsSubjectAllegation = request.IsSubjectAllegation;
                            ExistUseredit.Position = request.Position;
                            ExistUseredit.DateOfBirth = request.DateOfBirth;
                            ExistUseredit.UpdateById = await _ISessionService.GetUserId();
                            ExistUseredit.UpdatedDate = DateTime.Now;
                            ExistUseredit.IsActive = true;
                            _context.IncidentWorkerAllegation.Update(ExistUseredit);
                            await _context.SaveChangesAsync();
                            var ExistUserdis = _context.IncidentDisablePersonAllegation.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId);
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
                                ExistUserdis.UpdateById = await _ISessionService.GetUserId();
                                ExistUserdis.UpdatedDate = DateTime.Now;
                                _context.IncidentDisablePersonAllegation.Update(ExistUserdis);
                                _context.SaveChanges();
                            }
                            _context.IncidentAllegationPrimaryDisability.RemoveRange(_context.IncidentAllegationPrimaryDisability.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                            _context.IncidentAllegationOtherDisability.RemoveRange(_context.IncidentAllegationOtherDisability.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                            _context.IncidentAllegationBehaviour.RemoveRange(_context.IncidentAllegationBehaviour.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                            _context.IncidentAllegationCommunication.RemoveRange(_context.IncidentAllegationCommunication.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                            await _context.SaveChangesAsync();
                           
                                foreach (var id in request.PrimaryDisability)
                                {
                                    IncidentAllegationPrimaryDisability comm = new IncidentAllegationPrimaryDisability();
                                    comm.ClientId = request.ClientId;
                                    comm.DisableAllegationId = ExistUserdis.Id;
                                    comm.PrimaryDisability = id;
                                    comm.ShiftId = request.ShiftId;
                                    comm.EmployeeId = request.EmployeeId;
                                    comm.CreatedById = await _ISessionService.GetUserId();
                                    comm.CreatedDate = DateTime.Now;
                                    comm.IsActive = true;
                                    await _context.IncidentAllegationPrimaryDisability.AddAsync(comm);

                                }
                             
                           
                                foreach (var id in request.OtherDisability)
                                {
                                    IncidentAllegationOtherDisability comm1 = new IncidentAllegationOtherDisability();
                                    comm1.ClientId = request.ClientId;
                                    comm1.DisableAllegationId = ExistUserdis.Id;
                                    comm1.OtherDisability = id;
                                    comm1.ShiftId = request.ShiftId;
                                    comm1.EmployeeId = request.EmployeeId;
                                    comm1.CreatedById = await _ISessionService.GetUserId();
                                    comm1.CreatedDate = DateTime.Now;
                                    comm1.IsActive = true;
                                    await _context.IncidentAllegationOtherDisability.AddAsync(comm1);

                                }
                             
                          
                                foreach (var id in request.ConcerBehaviourId)
                                {
                                    IncidentAllegationBehaviour comm2 = new IncidentAllegationBehaviour();
                                    comm2.ClientId = request.ClientId;
                                    comm2.DisableAllegationId = ExistUserdis.Id;
                                    comm2.ConcerBehaviourId = id;
                                    comm2.ShiftId = request.ShiftId;
                                    comm2.EmployeeId = request.EmployeeId;
                                    comm2.CreatedById = await _ISessionService.GetUserId();
                                    comm2.CreatedDate = DateTime.Now;
                                    comm2.IsActive = true;
                                    await _context.IncidentAllegationBehaviour.AddAsync(comm2);

                                }
                            
                                foreach (var id in request.CommunicationId)
                                {
                                    IncidentAllegationCommunication comm3 = new IncidentAllegationCommunication();
                                    comm3.ClientId = request.ClientId;
                                    comm3.DisableAllegationId = ExistUserdis.Id;
                                    comm3.CommunicationId = id;
                                    comm3.ShiftId = request.ShiftId;
                                    comm3.EmployeeId = request.EmployeeId;
                                    comm3.CreatedById = await _ISessionService.GetUserId();
                                    comm3.CreatedDate = DateTime.Now;
                                    comm3.IsActive = true;
                                    await _context.IncidentAllegationCommunication.AddAsync(comm3);

                                }
                            _context.SaveChanges();
                            var ExistUserother = _context.IncidentOtherAllegation.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId);
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
                                ExistUserother.UpdateById = await _ISessionService.GetUserId();
                                ExistUserother.UpdatedDate = DateTime.Now;
                                ExistUserother.IsActive = true;
                                _context.IncidentOtherAllegation.Update(ExistUserother);
                                _context.SaveChanges();
                            }
                            response.Update(ExistUseredit);
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
