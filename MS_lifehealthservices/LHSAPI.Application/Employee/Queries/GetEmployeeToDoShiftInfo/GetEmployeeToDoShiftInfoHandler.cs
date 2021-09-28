
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using LHSAPI.Application.Employee.Models;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Application.Client.Models;
using AutoMapper;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeToDoShiftInfo
{
    public class GetEmployeeToDoShiftInfoHandler : IRequestHandler<GetEmployeeToDoShiftInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeToDoShiftInfoHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetEmployeeToDoShiftInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                EmployeeDetails _clientDetails = new EmployeeDetails();

               //_clientDetails.EmployeeToDoListShift = (from comminfo in _dbContext.EmployeeToDoListShift
               //                                        where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.EmployeeId == request.Id 
               //                                        && comminfo.ShiftId==request.ShiftId && comminfo.DateTime==request.DateTime
               //                                                       select new LHSAPI.Application.Employee.Models.EmployeeToDoListShift
               //                                                       {
               //                                                           Id = comminfo.Id,
               //                                                           EmployeeId = comminfo.EmployeeId,
               //                                                           ShiftId = comminfo.ShiftId,
               //                                                           DateTime = comminfo.DateTime,
               //                                                           IsInitials=comminfo.IsInitials,
               //                                                           Initials=comminfo.Initials,
               //                                                           IsReceived=comminfo.IsInitials,
               //                                                           Received=comminfo.Received,
               //                                                           DescriptionId=comminfo.DescriptionId
               //                                                       }).OrderByDescending(x => x.Id).ToList();
               
               
               if (_clientDetails.EmployeeToDoListShift == null) _clientDetails.EmployeeToDoListShift = new List<LHSAPI.Application.Employee.Models.EmployeeToDoListShift>();
                response.SuccessWithOutMessage(_clientDetails);
            }
            catch (Exception ex)
            {
                response.Status = (int)Number.Zero;
                response.Message = ResponseMessage.Error;
                response.StatusCode = HTTPStatusCode.INTERNAL_SERVER_ERROR;
            }
            return response;
        }
        #endregion
    }
}
