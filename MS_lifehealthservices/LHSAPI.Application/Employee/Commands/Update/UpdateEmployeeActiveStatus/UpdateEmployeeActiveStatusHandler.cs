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

namespace LHSAPI.Application.Employee.Commands.Update.UpdateEmployeeActiveStatus
{
    public class UpdateEmployeeActiveStatusHandler : IRequestHandler<UpdateEmployeeActiveStatusCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;

        public UpdateEmployeeActiveStatusHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<ApiResponse> Handle(UpdateEmployeeActiveStatusCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeePrimaryInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                      
                        ExistEmp.Status = request.Status;
                        ExistEmp.IsActive = request.Status;
                        ExistEmp.UpdatedDate = DateTime.Now;
                        _context.EmployeePrimaryInfo.Update(ExistEmp);
                        _context.SaveChanges();
                        response.Update(ExistEmp);

                    }
                    else
                    {
                        response.NotFound();

                    }
                }
                else
                {
                    response.ValidationError();
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
