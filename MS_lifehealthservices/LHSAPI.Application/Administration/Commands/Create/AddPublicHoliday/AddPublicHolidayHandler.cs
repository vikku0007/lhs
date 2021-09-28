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
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Administration.Commands.Create.AddPublicHoliday
{
    public class AddPublicHolidayHandler : IRequestHandler<AddPublicHolidayCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddPublicHolidayHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;

        }

        public async Task<ApiResponse> Handle(AddPublicHolidayCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {

                var ExistUser = _context.PublicHoliday.FirstOrDefault(x => x.Holiday == request.Holiday && x.DateFrom == request.DateFrom && x.IsActive == true);
                if (ExistUser == null)
                {
                    LHSAPI.Domain.Entities.PublicHoliday holiday = new LHSAPI.Domain.Entities.PublicHoliday();

                    holiday.Holiday = request.Holiday;
                    holiday.DateFrom = request.DateFrom;
                    holiday.DateTo = request.DateTo;
                    holiday.Year = request.Year;
                    holiday.CreatedById = await _ISessionService.GetUserId();
                    holiday.CreatedDate = DateTime.Now;
                    holiday.IsActive = true;
                    await _context.PublicHoliday.AddAsync(holiday);
                    _context.SaveChanges();
                    response.Success(holiday);

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
