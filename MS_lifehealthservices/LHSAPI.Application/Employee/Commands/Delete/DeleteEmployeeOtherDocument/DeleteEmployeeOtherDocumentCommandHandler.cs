﻿using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeOtherDocument
{

    public class DeleteEmployeeOtherDocumentCommandHandler : IRequestHandler<DeleteEmployeeOtherDocumentCommand, ApiResponse>
    { 

        private readonly LHSDbContext _context;

        public DeleteEmployeeOtherDocumentCommandHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<ApiResponse> Handle(DeleteEmployeeOtherDocumentCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var AvalResult = _context.EmployeeOtherComplianceDetails.FirstOrDefault(x => x.Id == request.Id &&  x.IsDeleted == false && x.IsActive == true);
                    if (AvalResult == null)
                    {
                        response.NotFound();
                    }
                    else
                    {
                        
                        AvalResult.OtherFileName = null;
                       _context.EmployeeOtherComplianceDetails.Update(AvalResult);
                        await _context.SaveChangesAsync();
                        response.Delete(AvalResult);

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
