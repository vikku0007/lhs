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

namespace LHSAPI.Application.Administration.Commands.Update.UpdatePublicHoliday
{
    public class UpdatePublicHolidayHandler : IRequestHandler<UpdatePublicHolidayCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;

        public UpdatePublicHolidayHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<ApiResponse> Handle(UpdatePublicHolidayCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.PublicHoliday.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {

                        ExistEmp.Holiday = request.Holiday;
                        ExistEmp.DateFrom = request.DateFrom;
                        ExistEmp.DateTo = request.DateTo;
                        ExistEmp.Year = request.Year;
                        ExistEmp.UpdateById = 1;
                        ExistEmp.UpdatedDate = DateTime.Now;
                        _context.PublicHoliday.Update(ExistEmp);
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
