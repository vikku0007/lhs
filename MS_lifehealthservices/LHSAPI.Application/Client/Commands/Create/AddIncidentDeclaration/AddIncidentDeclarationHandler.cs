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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentDeclaration
{
    public class AddIncidentDeclarationHandler : IRequestHandler<AddIncidentDeclarationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddIncidentDeclarationHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentDeclarationCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
               
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                      var  ExistUser = _context.IncidentDeclaration.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                    
                    if (ExistUser == null)
                    {
                        IncidentDeclaration user = new IncidentDeclaration();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.Name = request.Name;
                        user.PositionAtOrganisation = request.PositionAtOrganisation;
                        user.Date = request.Date;
                        user.IsDeclaration = request.IsDeclaration;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.IncidentDeclaration.AddAsync(user);
                        _context.SaveChanges();
                        response.Success(user);

                    }
                    else
                    {
                        ExistUser.Name = request.Name;
                        ExistUser.PositionAtOrganisation = request.PositionAtOrganisation;
                        ExistUser.Date = request.Date;
                        ExistUser.IsDeclaration = request.IsDeclaration;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.IncidentDeclaration.Update(ExistUser);
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
