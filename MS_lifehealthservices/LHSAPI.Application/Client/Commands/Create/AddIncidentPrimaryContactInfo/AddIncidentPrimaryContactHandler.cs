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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentPrimaryContact
{
    public class AddIncidentPrimaryContactHandler : IRequestHandler<AddIncidentPrimaryContactCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddIncidentPrimaryContactHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentPrimaryContactCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
              
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                      var  ExistUser = _context.ClientAccidentPrimaryContact.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                   
                    if (ExistUser == null)
                    {
                        ClientAccidentPrimaryContact user = new ClientAccidentPrimaryContact();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.Title = request.Title;
                        user.FirstName = request.FirstName;
                        user.MiddleName = request.MiddleName;
                        user.LastName = request.LastName;
                        user.ProviderPosition = request.ProviderPosition;
                        user.PhoneNo = request.PhoneNo;
                        user.Email = request.Email;
                        user.ContactMetod = request.ContactMetod;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.ClientAccidentPrimaryContact.AddAsync(user);
                        _context.SaveChanges();
                        response.Success(user);

                    }
                    else
                    {
                        ExistUser.EmployeeId = request.EmployeeId;
                        ExistUser.Title = request.Title;
                        ExistUser.FirstName = request.FirstName;
                        ExistUser.MiddleName = request.MiddleName;
                        ExistUser.LastName = request.LastName;
                        ExistUser.ProviderPosition = request.ProviderPosition;
                        ExistUser.PhoneNo = request.PhoneNo;
                        ExistUser.Email = request.Email;
                        ExistUser.ContactMetod = request.ContactMetod;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientAccidentPrimaryContact.Update(ExistUser);
                        await _context.SaveChangesAsync();
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
