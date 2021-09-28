
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


namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeHours
{
    public class GetEmployeeHoursHandler : IRequestHandler<GetEmployeeHoursCommand, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public GetEmployeeHoursHandler(LHSDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("SqlConnection");
            return new SqlConnection(connectionString);
        }
        public async Task<ApiResponse> Handle(GetEmployeeHoursCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            EmployeeDashBoardHours ShiftHours = new EmployeeDashBoardHours();
            try
            {

                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetEmployeeHours_Employee_V1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employeeid", request.EmployeeId);
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            ShiftHours.ApprovedShifts = reader["ApprovedShifts"] != System.DBNull.Value ? Convert.ToInt64(reader["ApprovedShifts"]) : 0;
                            ShiftHours.CancelHours = reader["CancelHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["CancelHours"]), 2) : 0;
                            ShiftHours.PendingShifts = reader["PendingShifts"] != System.DBNull.Value ? Convert.ToInt64(reader["PendingShifts"]) : 0;
                            ShiftHours.TotalHours = reader["TotalHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["TotalHours"]), 2) : 0;
                            ShiftHours.CompletedHours = reader["CompletedHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["CompletedHours"]), 2) : 0;
                            ShiftHours.UnallocatedHours = reader["UnallocatedHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["UnallocatedHours"]), 2) : 0;
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

        public class EmployeeDashBoardHours
        {
            public double TotalHours { get; set; }
            public long PendingShifts { get; set; }
            public double CancelHours { get; set; }
            public long ApprovedShifts { get; set; }
            public double UnallocatedHours { get; set; }
            public double CompletedHours { get; set; }
        }
    }

}
