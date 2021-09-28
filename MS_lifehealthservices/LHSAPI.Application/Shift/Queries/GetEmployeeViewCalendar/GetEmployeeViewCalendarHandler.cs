using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LHSAPI.Domain.Entities;
using LHSAPI.Application.Shift.Models;
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Shift.Queries.GetEmployeeViewCalendar
{
    public class GetEmployeeViewCalendarHandler : IRequestHandler<GetEmployeeViewCalendarQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly IShiftService _IShiftService;
        //   readonly ILoggerManager _logger;
        public GetEmployeeViewCalendarHandler(LHSDbContext dbContext, IShiftService IShiftService)
        {
            _dbContext = dbContext;
            // _logger = logger;
            _IShiftService = IShiftService;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetEmployeeViewCalendarQuery request, CancellationToken cancellationToken)
        {

            return _IShiftService.GetEmployeeViewCalendar(request);
        }
        #endregion
    }
}
