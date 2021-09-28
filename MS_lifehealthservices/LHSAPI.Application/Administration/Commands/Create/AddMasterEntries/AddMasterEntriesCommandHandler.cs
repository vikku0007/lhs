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
using LHSAPI.Application.Employee.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Administration.Commands.Create.AddMasterEntries
{
    public class AddMasterEntriesCommandHandler : IRequestHandler<AddMasterEntriesCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddMasterEntriesCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }


        public async Task<ApiResponse> Handle(AddMasterEntriesCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
               
               var ExistUser = _context.StandardCode.FirstOrDefault(x => x.CodeData == request.CodeData && x.CodeDescription == request.CodeDescription && x.IsActive == true);
                if (ExistUser == null)
                {
                    LHSAPI.Domain.Entities.StandardCode user = new LHSAPI.Domain.Entities.StandardCode();

                    user.CodeData = request.CodeData;
                    user.CreatedById = await _ISessionService.GetUserId();
                    user.CreatedDate = DateTime.Now;
                    user.CodeDescription = request.CodeDescription;
                    user.IsActive = true;
                    await _context.StandardCode.AddAsync(user);
                    _context.SaveChanges();
                    response.Success(user);
                   
                }
                else
                {
                    response.AlreadyExist();

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
