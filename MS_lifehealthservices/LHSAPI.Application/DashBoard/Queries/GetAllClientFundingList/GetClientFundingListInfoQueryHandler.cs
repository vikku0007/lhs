
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

using System.Text.Json;
using LHSAPI.Application.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LHSAPI.Application.DashBoard.Queries.GetClientFundingList
{
    public class GetClientFundingListInfoQueryHandler : IRequestHandler<GetClientFundingListInfoQuery, ApiResponse>, IRequestHandler<GetAdminDashboardHours, ApiResponse>, IRequestHandler<GetAdminDashboardShiftTimePer, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly IConfiguration _configuration;
        //   readonly ILoggerManager _logger;
        public GetClientFundingListInfoQueryHandler(LHSDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            // _logger = logger;
        }

        public async Task<ApiResponse> Handle(GetClientFundingListInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientDetails _clientDetails = new ClientDetails();
                //List<ClientFunding> clientFundinglist = new List<ClientFunding>();
                DateTime Todaydate = DateTime.Now.Date;

                var fundinglist = (from fund in _dbContext.ClientFundingInfo
                                   where fund.IsDeleted == false && fund.IsActive == true
                                   select new
                                   {
                                       Id = fund.Id,
                                       Ammount = (fund.Amount),
                                       FundType = (fund.FundType),
                                       FundTypeName = _dbContext.StandardCode.Where(x => x.ID == fund.FundType).Select(x => x.CodeDescription).FirstOrDefault(),
                                       FullName = _dbContext.ClientPrimaryInfo.Where(x => x.Id == fund.ClientId).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                       ClientId = fund.ClientId,
                                       StartDate = fund.StartDate,
                                       EndDate = fund.EndDate,
                                       ReferenceNumber = fund.RefNumber,
                                       ExpiryDays = ((Todaydate) - Convert.ToDateTime(fund.EndDate.Value.Date).AddDays(1)).Days,
                                       CreatedDate = fund.CreatedDate
                                   }).OrderByDescending(x => x.Id).ToList();

                //test
                if (fundinglist != null && fundinglist.Any())
                {
                    var clientFundinglist = fundinglist.Where(x => x.ExpiryDays < 0).OrderByDescending(x => x.ExpiryDays).ToList();
                    if (clientFundinglist != null && clientFundinglist.Any())
                    {
                        var totalCount = clientFundinglist.Count();

                        switch (request.OrderBy)
                        {
                            case Common.Enums.Client.ClientAgreementOrderBy.ClientName:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.FullName).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.FullName).ToList();
                                }
                                break;
                            case Common.Enums.Client.ClientAgreementOrderBy.FundingSource:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.FundType).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.FundType).ToList();
                                }
                                break;
                            case Common.Enums.Client.ClientAgreementOrderBy.StartDate:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.StartDate).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.StartDate).ToList();
                                }
                                break;

                            case Common.Enums.Client.ClientAgreementOrderBy.EndDate:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.EndDate).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.EndDate).ToList();
                                }
                                break;
                            case Common.Enums.Client.ClientAgreementOrderBy.Expiry:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.ExpiryDays).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.ExpiryDays).ToList();
                                }
                                break;
                            case Common.Enums.Client.ClientAgreementOrderBy.Balance:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.Ammount).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.Ammount).ToList();
                                }
                                break;
                            default:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.ExpiryDays).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.ExpiryDays).ToList();
                                }

                                break;
                        }


                        //empList = empList.Skip<EmployeePrimaryInfo>((request.PageNo > 0 ? (request.PageNo - 1) : request.PageNo) * request.PageSize).Take<EmployeePrimaryInfo>(request.PageSize).ToList();
                        var clientlist = clientFundinglist.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                        response.Total = totalCount;
                        response.SuccessWithOutMessage(clientlist);



                    }
                    else
                    {
                        response = response.NotFound();
                    }

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
        public async Task<ApiResponse> Handle(GetAdminDashboardHours request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            AdminDashBoardHours ShiftHours = new AdminDashBoardHours();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetAdminDashboardEmployeeHours", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@startDate", request.StartDate);
                        cmd.Parameters.AddWithValue("@endDate", request.EndDate);

                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            ShiftHours.BookedHours = reader["BookedHours"] != System.DBNull.Value ? Convert.ToDouble(reader["BookedHours"]) : 0;
                            ShiftHours.CancelShifts = reader["CancelShifts"] != System.DBNull.Value ? Convert.ToInt64(reader["CancelShifts"]) : 0;
                            ShiftHours.PendingShifts = reader["PendingShifts"] != System.DBNull.Value ? Convert.ToInt64(reader["PendingShifts"]) : 0;
                            ShiftHours.TotalHours = reader["TotalHours"] != System.DBNull.Value ? Convert.ToDouble(reader["TotalHours"]) : 0;
                            ShiftHours.UnallocatedHours = reader["UnallocatedHours"] != System.DBNull.Value ? Convert.ToDouble(reader["UnallocatedHours"]) : 0;


                        }
                    }
                }
                response = response.Success(ShiftHours);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        public async Task<ApiResponse> Handle(GetAdminDashboardShiftTimePer request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            AdminDashBoardShiftTimeStatus ShiftTimeStatus = new AdminDashBoardShiftTimeStatus();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetAdminDashboardCheckinCheckoutPercentage", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@startDate", request.StartDate);
                        cmd.Parameters.AddWithValue("@endDate", request.EndDate);
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ShiftTimeStatus.ShiftOnTimePercentage = reader["ShiftOnTimePercentage"] != System.DBNull.Value ? Convert.ToSingle(reader["ShiftOnTimePercentage"]) : 0;
                            ShiftTimeStatus.ShiftOnTime = reader["ShiftOnTime"] != System.DBNull.Value ? Convert.ToSingle(reader["ShiftOnTime"]) : 0;
                            ShiftTimeStatus.ShiftLatePercentage = reader["ShiftLatePercentage"] != System.DBNull.Value ? Convert.ToSingle(reader["ShiftLatePercentage"]) : 0;
                            ShiftTimeStatus.ShiftLate = reader["ShiftLate"] != System.DBNull.Value ? Convert.ToSingle(reader["ShiftLate"]) : 0;
                        }
                    }
                }
                response = response.Success(ShiftTimeStatus);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("SqlConnection");
            return new SqlConnection(connectionString);
        }

    }
    public class AdminDashBoardHours
    {
        public double TotalHours { get; set; }
        public long PendingShifts { get; set; }
        public long CancelShifts { get; set; }
        public double BookedHours { get; set; }
        public double UnallocatedHours { get; set; }
    }
    public class AdminDashBoardShiftTimeStatus
    {
        public float ShiftOnTimePercentage { get; set; }
        public float ShiftOnTime { get; set; }
        public float ShiftLatePercentage { get; set; }
        public float ShiftLate { get; set; }

    }

}
