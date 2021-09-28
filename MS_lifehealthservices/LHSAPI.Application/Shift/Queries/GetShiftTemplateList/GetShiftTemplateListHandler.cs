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

namespace LHSAPI.Application.Shift.Queries.GetShiftTemplateList
{
    public class GetShiftTemplateListHandler : IRequestHandler<GetShiftTemplateListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly IShiftService _IShiftService;
        //   readonly ILoggerManager _logger;
        public GetShiftTemplateListHandler(LHSDbContext dbContext, IShiftService IShiftService)
        {
            _dbContext = dbContext;
            // _logger = logger;
            _IShiftService = IShiftService;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetShiftTemplateListQuery request, CancellationToken cancellationToken)
        {

            return _IShiftService.GetShiftTemplateList();
        }
        #endregion
    }
}
