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

namespace LHSAPI.Application.Employee.Commands.Update.UpdateMasterActiveInActive
{
    public class UpdateMasterActiveInActiveHandler : IRequestHandler<UpdateMasterActiveInActiveCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;

        public UpdateMasterActiveInActiveHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<ApiResponse> Handle(UpdateMasterActiveInActiveCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.StandardCode.FirstOrDefault(x => x.ID == request.Id && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                      
                        ExistEmp.IsActive = request.Status;
                        ExistEmp.UpdatedDate = DateTime.Now;
                        _context.StandardCode.Update(ExistEmp);
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
