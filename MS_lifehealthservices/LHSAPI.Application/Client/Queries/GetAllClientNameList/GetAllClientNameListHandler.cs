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

namespace LHSAPI.Application.Client.Queries.GetAllClientNameList
{
    public class GetAllClientNameListHandler : IRequestHandler<GetAllClientNameListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllClientNameListHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Client List

        public async Task<ApiResponse> Handle(GetAllClientNameListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from Employeedata in _dbContext.ClientPrimaryInfo
                                  where Employeedata.IsDeleted == false && Employeedata.IsActive == true &&  Employeedata.IsDeleted == false
                                  select new
                                  {
                                      Id = Employeedata.Id,
                                      //LocationId = Employeedata.LocationId,
                                      //Name = _dbContext.Location.Where(x => x.LocationId == Employeedata.LocationId).Select(x => x.Name).FirstOrDefault(),
                                      //GenderName = _dbContext.StandardCode.Where(x => x.ID == Employeedata.Gender).Select(x => x.CodeDescription).FirstOrDefault(),                
                                      FullName = Employeedata.FirstName + " " + 
                                      (Employeedata.MiddleName == null ? "" : Employeedata.MiddleName) + " " + Employeedata.LastName

                                  }).OrderBy(x=>x.FullName).ToList();
                
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();
                    response.ResponseData = AvbempList.ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(AvbempList.ToList());

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
