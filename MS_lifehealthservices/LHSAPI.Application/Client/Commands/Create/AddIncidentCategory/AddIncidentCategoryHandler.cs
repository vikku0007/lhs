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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentCategory
{
    public class AddIncidentCategoryHandler : IRequestHandler<AddIncidentCategoryCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddIncidentCategoryHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentCategoryCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
               
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                     var ExistUser = _context.ClientIncidentCategory.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                   
                    if (ExistUser == null)
                    {
                        ClientIncidentCategory user = new ClientIncidentCategory();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId= request.EmployeeId;
                        user.IsIncidentAnticipated = request.IsIncidentAnticipated;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.ClientIncidentCategory.AddAsync(user);
                        _context.SaveChanges();

                        foreach (var id in request.PrimaryIncidentId)
                        {

                            ClientPrimaryIncidentCategory comm = new ClientPrimaryIncidentCategory();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.IncidentCategoryId = user.Id;
                            comm.PrimaryIncidentId = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.ClientPrimaryIncidentCategory.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        foreach (var id in request.SecondaryIncidentId)
                        {
                            ClientSecondaryIncidentCategory comm = new ClientSecondaryIncidentCategory();
                            comm.ClientId = request.ClientId;
                            comm.ShiftId = request.ShiftId;
                            comm.EmployeeId = request.EmployeeId;
                            comm.IncidentCategoryId = user.Id;
                            comm.SecondaryIncidentId = id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.ClientSecondaryIncidentCategory.AddAsync(comm);
                            _context.SaveChanges();
                        }
                        response.Success(user);
                    }

                    else
                    {
                        ExistUser.IsIncidentAnticipated = request.IsIncidentAnticipated;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientIncidentCategory.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        _context.ClientPrimaryIncidentCategory.RemoveRange(_context.ClientPrimaryIncidentCategory.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                        _context.ClientSecondaryIncidentCategory.RemoveRange(_context.ClientSecondaryIncidentCategory.Where(x => x.ClientId == request.ClientId && x.IsActive == true && x.ShiftId == request.ShiftId));
                        await _context.SaveChangesAsync();
                     
                            foreach (var id in request.PrimaryIncidentId)
                            {
                                ClientPrimaryIncidentCategory commuser = new ClientPrimaryIncidentCategory();
                                commuser.ClientId = request.ClientId;
                                commuser.PrimaryIncidentId = id;
                                commuser.ShiftId = request.ShiftId;
                                commuser.EmployeeId = request.EmployeeId;
                                commuser.IncidentCategoryId = ExistUser.Id;
                                commuser.CreatedById = await _ISessionService.GetUserId();
                                commuser.CreatedDate = DateTime.Now;
                                commuser.IsActive = true;
                                await _context.ClientPrimaryIncidentCategory.AddAsync(commuser);

                            }
                          
                      
                            foreach (var id in request.SecondaryIncidentId)
                            {
                                ClientSecondaryIncidentCategory comm1 = new ClientSecondaryIncidentCategory();
                                comm1.ClientId = request.ClientId;
                                comm1.ShiftId = request.ShiftId;
                                comm1.EmployeeId = request.EmployeeId;
                                comm1.SecondaryIncidentId = id;
                                comm1.IncidentCategoryId = ExistUser.Id;
                                comm1.CreatedById = await _ISessionService.GetUserId();
                                comm1.CreatedDate = DateTime.Now;
                                comm1.IsActive = true;
                                await _context.ClientSecondaryIncidentCategory.AddAsync(comm1);

                            }
                            _context.SaveChanges();
                       
                        response.Update(ExistUser);
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
