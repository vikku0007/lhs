
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
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Domain.Entities;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeAppraisaldetails
{
    public class GetEmployeeAppraisaldetailsQueryHandler : IRequestHandler<GetEmployeeAppraisaldetailsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeAppraisaldetailsQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeAppraisaldetailsQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

               

                var commList = (from staff in _dbContext.EmployeeAppraisalDetails
                                where staff.IsActive == true && staff.IsDeleted == false && staff.Id == request.Id
                                select new LHSAPI.Application.Employee.Models.EmployeeAppraisalTypemodel
                                {
                                    Id = staff.Id,
                                    EmployeeId = staff.Id,
                                    AppraisalDateFrom = staff.AppraisalDateFrom,
                                    AppraisalDateTo = staff.AppraisalDateTo,
                                    DepartmentName = staff.DepartmentName,
                                    AppraisalType = staff.AppraisalType,
                                    AppraisalTypeName = _dbContext.StandardCode.Where(x => x.ID == staff.AppraisalType).Select(x => x.CodeDescription).FirstOrDefault(),
                                   
                                }).OrderByDescending(x => x.Id).ToList();
              

                 if (commList != null && commList.Count > 0)
                {

                    response.SuccessWithOutMessage(commList.ToList());
                }
                else
                {
                    response.NotFound();
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
