﻿using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeExperienceInfo
{
    public class DeleteEmployeeExperienceInfoHandler : IRequestHandler<DeleteEmployeeExperienceInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteEmployeeExperienceInfoHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteEmployeeExperienceInfoCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistEmp = _context.EmployeeWorkExp.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                    if (ExistEmp == null)
                    {
                        response.NotFound();
                    }
                    else
                    {

                        ExistEmp.IsDeleted = true;
                        ExistEmp.IsActive = false;
                        ExistEmp.DeletedDate = DateTime.UtcNow;
                        ExistEmp.DeletedById = await _ISessionService.GetUserId();
                        _context.EmployeeWorkExp.Update(ExistEmp);
                        await _context.SaveChangesAsync();
                        response.Delete(ExistEmp);

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
