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
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LHSAPI.Application.TimeSheet.Queries.GetEmployeeHourReport
{
    public class GetEmployeeHourReportHandler : IRequestHandler<GetEmployeeHourReport, ApiResponse>
  {
        private readonly LHSDbContext _dbContext;
        private readonly IConfiguration _configuration;
        //   readonly ILoggerManager _logger;
        public GetEmployeeHourReportHandler(LHSDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
      // _logger = logger;
    }
    #region Get Shift List

    public async Task<ApiResponse> Handle(GetEmployeeHourReport request, CancellationToken cancellationToken)
    {

      ApiResponse response = new ApiResponse();
      try
      {

        List<EmployeeHoursReport> EmployeeHours = new List<EmployeeHoursReport>();
        using (SqlConnection conn = GetConnection())
        {
          using (SqlCommand cmd = new SqlCommand("Usp_GetEmployeesHoursReport", conn))
          {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@employeeid", request.SearchByEmpId);
            cmd.Parameters.AddWithValue("@startDate", request.StartDate);
            cmd.Parameters.AddWithValue("@endDate", request.EndDate);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
              EmployeeHoursReport objEmployeeHours = new EmployeeHoursReport();
              objEmployeeHours.ShiftId = Convert.ToInt32(reader["id"]);
              objEmployeeHours.StartDate =  Convert.ToDateTime(reader["startdate"]).Date.Add(TimeSpan.Parse(reader["starttime"].ToString()));
              objEmployeeHours.EndDate = Convert.ToDateTime(reader["enddate"]).Date.Add(TimeSpan.Parse(reader["endtime"].ToString()));
              objEmployeeHours.StartTime = objEmployeeHours.StartDate.ToString(@"hh\:mm tt");
              objEmployeeHours.EndTime = objEmployeeHours.EndDate.ToString(@"hh\:mm tt");
              objEmployeeHours.EmployeeId = Convert.ToInt32(reader["employeeid"]);
              objEmployeeHours.IsAccepted = Convert.ToBoolean(reader["Isaccepted"]);
              objEmployeeHours.IsSleepover = Convert.ToBoolean(reader["issleepover"]);
              objEmployeeHours.IsHoliday = Convert.ToBoolean(reader["isholiday"]);
              objEmployeeHours.IsActiveNight = Convert.ToBoolean(reader["isactivenight"]);
              objEmployeeHours.Duration = Convert.ToDouble(reader["duration"]);
              EmployeeHours.Add(objEmployeeHours);
            }
          }
        }
        response.Total = EmployeeHours.Count;
        response.SuccessWithOutMessage(EmployeeHours);
      }
      catch (Exception ex)
      {
        response.Failed(ex.Message);
      }
      return response;
    }
    #endregion
   
    private SqlConnection GetConnection()
    {
      string connectionString = _configuration.GetConnectionString("SqlConnection");
      return new SqlConnection(connectionString);
    }
  }
  
  public class EmployeeHoursReport
  {
    public int ShiftId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int EmployeeId { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsSleepover { get; set; }
    public bool IsHoliday { get; set; }
    public bool IsActiveNight { get; set; }
    public double Duration { get; set; }

   
  }
}
