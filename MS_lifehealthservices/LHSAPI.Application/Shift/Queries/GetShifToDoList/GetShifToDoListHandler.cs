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

namespace LHSAPI.Application.Shift.Queries.GetShifToDoList
{
  public class GetShifToDoListHandler : IRequestHandler<GetShifToDoListQuery, ApiResponse>
  {
    private readonly LHSDbContext _dbContext;
    private readonly IShiftService _ShiftService;
    //   readonly ILoggerManager _logger;
    public GetShifToDoListHandler(LHSDbContext dbContext, IShiftService ShiftService)
    {
      _dbContext = dbContext;
      // _logger = logger;
      _ShiftService = ShiftService;
    }
    #region Get Shift List

    public async Task<ApiResponse> Handle(GetShifToDoListQuery request, CancellationToken cancellationToken)
    {
      //throw new NotImplementedException();
      ApiResponse response = new ApiResponse();
      try
      {

        var _shift = _ShiftService.GetShiftToDoList(request.ShiftId);

        if (_shift != null)
        {
          response.SuccessWithOutMessage(_shift);
        }
        else
        {
          response = response.NotFound();
        }


      }
      catch (Exception ex)
      {
        response.Failed(ex.Message);
      }
      return response;
    }
    #endregion
  }
}
