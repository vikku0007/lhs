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

namespace LHSAPI.Application.PayRoll.Queries.GetEmployesHours
{
    public class GetEmployesHoursQueryHandler : IRequestHandler<GetEmployesHoursQuery, ApiResponse>, IRequestHandler<GetEmployeeHoursDetail, ApiResponse>, IRequestHandler<UpdateRejectedShift, ApiResponse>, IRequestHandler<GetEmployeeMyObHoursDetail, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly IConfiguration _configuration;
        //   readonly ILoggerManager _logger;
        public GetEmployesHoursQueryHandler(LHSDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            // _logger = logger;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetEmployesHoursQuery request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {

                List<EmployeeHours> EmployeeHours = new List<EmployeeHours>();
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetEmployeesHour", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employeeid", request.SearchByEmpName);
                        cmd.Parameters.AddWithValue("@startDate", request.StartDate);
                        cmd.Parameters.AddWithValue("@endDate", request.EndDate);
                        cmd.Parameters.AddWithValue("@IsOnTime", request.IsOnTime);
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            EmployeeHours objEmployeeHours = new EmployeeHours();
                            objEmployeeHours.WeekDay = reader["Weekly"] != System.DBNull.Value ? Math.Round(Convert.ToSingle(reader["Weekly"]), 2) : 0;
                            objEmployeeHours.Sat = reader["Sat"] != System.DBNull.Value ? Math.Round(Convert.ToSingle(reader["Sat"]), 2) : 0;
                            objEmployeeHours.Sun = reader["Sun"] != System.DBNull.Value ? Math.Round(Convert.ToSingle(reader["Sun"]), 2) : 0;
                            objEmployeeHours.SleepOver = Math.Round(Convert.ToSingle(reader["SleepOver"]), 2);
                            objEmployeeHours.SleepOver = GetSleepOverCount(objEmployeeHours.SleepOver);
                            objEmployeeHours.ActiveNightWeekDay = Math.Round(Convert.ToSingle(reader["WeeklyActiveNight"]), 2);
                            objEmployeeHours.ActiveNightSat = Math.Round(Convert.ToSingle(reader["SatActiveNight"]), 2);
                            objEmployeeHours.ActiveNightSun = Math.Round(Convert.ToSingle(reader["SunActiveNight"]), 2);
                            objEmployeeHours.Holiday = Math.Round(Convert.ToSingle(reader["PublicHoliday"]), 2);
                            objEmployeeHours.ActiveNightHoliday = Math.Round(Convert.ToSingle(reader["PublicHolidayActiveNight"]), 2);
                            objEmployeeHours.EmployeeId = reader["EmployeeId"] != System.DBNull.Value ? Convert.ToInt32(reader["EmployeeId"]) : 0;
                            objEmployeeHours.FirstName = Convert.ToString(reader["FirstName"]);
                            objEmployeeHours.LastName = Convert.ToString(reader["LastName"]);
                            objEmployeeHours.Admin = Convert.ToSingle(reader["Admin"]);
                            objEmployeeHours.Cleaning = Convert.ToSingle(reader["Cleaning"]);
                            objEmployeeHours.Total = Math.Round(Convert.ToSingle(reader["Total"]), 2);
                            EmployeeHours.Add(objEmployeeHours);
                        }  
                    }
                }

                response = response.Success(EmployeeHours);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }

        #endregion
        public async Task<ApiResponse> Handle(GetEmployeeHoursDetail request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                var s = Math.Round(Convert.ToSingle(-11), 2);
                List<EmployeeHoursDetail> EmployeeHours = new List<EmployeeHoursDetail>();
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetEmployeesHourDetail_V1", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employeeid", request.SearchByEmpName);
                        cmd.Parameters.AddWithValue("@startDate", request.StartDate);
                        cmd.Parameters.AddWithValue("@endDate", request.EndDate);
                        cmd.Parameters.AddWithValue("@IsOnTime", request.IsOnTime);
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            EmployeeHoursDetail objEmployeeHours = new EmployeeHoursDetail();
                            objEmployeeHours.ShiftTrackerId = Convert.ToInt32(reader["ShiftTrackerId"]);
                            objEmployeeHours.WeekDay = reader["Weekly"] != System.DBNull.Value ? Math.Round(Convert.ToSingle(reader["Weekly"]), 2) : 0;
                            objEmployeeHours.Sat = reader["Sat"] != System.DBNull.Value ? Math.Round(Convert.ToSingle(reader["Sat"]), 2) : 0;
                            objEmployeeHours.Sun = reader["Sun"] != System.DBNull.Value ? Math.Round(Convert.ToSingle(reader["Sun"]), 2) : 0;
                            objEmployeeHours.SleepOver = Math.Round(Convert.ToSingle(reader["SleepOver"]), 2);
                            objEmployeeHours.SleepOver = GetSleepOverCount(objEmployeeHours.SleepOver);
                            objEmployeeHours.ActiveNightWeekDay = Math.Round(Convert.ToSingle(reader["WeeklyActiveNight"]), 2);
                            objEmployeeHours.ActiveNightSat = Math.Round(Convert.ToSingle(reader["SatActiveNight"]), 2);
                            objEmployeeHours.ActiveNightSun = Math.Round(Convert.ToSingle(reader["SunActiveNight"]), 2);
                            objEmployeeHours.Holiday = Math.Round(Convert.ToSingle(reader["PublicHoliday"]), 2);
                            objEmployeeHours.ActiveNightHoliday = Math.Round(Convert.ToSingle(reader["PublicHolidayActiveNight"]), 2);
                            objEmployeeHours.EmployeeId = reader["EmployeeId"] != System.DBNull.Value ? Convert.ToInt32(reader["EmployeeId"]) : 0;
                            objEmployeeHours.FirstName = Convert.ToString(reader["FirstName"]);
                            objEmployeeHours.LastName = Convert.ToString(reader["LastName"]);
                            objEmployeeHours.Total = Math.Round(Convert.ToSingle(reader["Total"]), 2);
                            objEmployeeHours.ClientFirstName = Convert.ToString(reader["ClientFirstName"]);
                            objEmployeeHours.ClientLastName = Convert.ToString(reader["ClientLastName"]);
                            objEmployeeHours.StartDate = Convert.ToDateTime(reader["StartDate"]);
                            objEmployeeHours.EndDate = Convert.ToDateTime(reader["EndDate"]);
                            objEmployeeHours.Admin = Convert.ToSingle(reader["Admin"]);
                            objEmployeeHours.Cleaning = Convert.ToSingle(reader["Cleaning"]);
                            objEmployeeHours.IsShiftOnTime = Convert.ToBoolean(reader["IsShiftOnTime"]);
                            objEmployeeHours.IsApproveByAccounts = Convert.ToBoolean(reader["IsApproveByAccounts"]);
                            objEmployeeHours.ShiftId = Convert.ToInt32(reader["Id"]);
                            objEmployeeHours.EndTime = TimeSpan.Parse(reader["endtime"].ToString());
                            objEmployeeHours.StartTime = TimeSpan.Parse(reader["starttime"].ToString());
                            objEmployeeHours.StartTimeString = objEmployeeHours.StartDate.Date.Add(objEmployeeHours.StartTime).ToString(@"hh\:mm tt");
                            objEmployeeHours.EndTimeString = objEmployeeHours.EndDate.Date.Add(objEmployeeHours.EndTime).ToString(@"hh\:mm tt");
                            // Added on 28 october
                            objEmployeeHours.actualEndTime = TimeSpan.Parse(reader["actualEndTime"].ToString());
                            objEmployeeHours.actualStartTime = TimeSpan.Parse(reader["actualStartTime"].ToString());
                            objEmployeeHours.actualStartDate = Convert.ToDateTime(reader["actualStartDate"]);
                            objEmployeeHours.actualEndDate = Convert.ToDateTime(reader["actualEndDate"]);
                            objEmployeeHours.actualStartTimeString = objEmployeeHours.actualStartDate.Date.Add(objEmployeeHours.actualStartTime).ToString(@"hh\:mm tt");
                            objEmployeeHours.actualEndTimeString = objEmployeeHours.actualEndDate.Date.Add(objEmployeeHours.actualEndTime).ToString(@"hh\:mm tt");
                            EmployeeHours.Add(objEmployeeHours);
                        }
                        foreach (var item in EmployeeHours)
                        {
                            DateTime actualStartDateTime = item.actualStartDate.Date.Add(item.actualStartTime);
                            DateTime actualEndDateTime = item.actualEndDate.Date.Add(item.actualEndTime);
                            DateTime checkInDateTime = item.StartDate.Date.Add(item.StartTime);
                            DateTime checkOutDateTime = item.EndDate.Date.Add(item.EndTime);
                            var differenceCheckIn = checkInDateTime.Subtract(actualStartDateTime).TotalMinutes;
                            var differenceCheckOut = checkOutDateTime.Subtract(actualEndDateTime).TotalMinutes;
                            if (differenceCheckIn <= 15 && differenceCheckIn >= -15)
                            {
                                item.StartTimeString = item.actualStartTimeString;
                                var actualDuration = (actualEndDateTime.Subtract(actualStartDateTime).TotalHours);
                                var durationDiff = actualDuration - item.Total;

                                //start added by Deepak Bisht
                                if (item.Sat != 0 || item.Sun != 0)
                                {
                                    item.WeekDay = 0;
                                }
                                else
                                {
                                    item.WeekDay = item.WeekDay + durationDiff;
                                    item.WeekDay = Math.Round(item.WeekDay, 2);
                                }
                                // ended by deepakbisht
                            }
                            if (differenceCheckOut <= 15 && differenceCheckOut >= -15)
                            {
                                item.EndTimeString = item.actualEndTimeString;
                            }
                        }
                        //Added by deepak Bisht
                        var ids = new List<string>();
                        for (int i=0; i < EmployeeHours.Count-1; i++)
                        {
                           int CountClient = 1;
                           var indexes = new List<int>() { i };
                            if (ids.Contains(EmployeeHours[i].EmployeeId.ToString() + EmployeeHours[i].ShiftId.ToString()))
                                continue;

                            for (int j = i + 1; j < EmployeeHours.Count; j++)
                            {
                                if (EmployeeHours[i].ShiftId == EmployeeHours[j].ShiftId &&
                                       EmployeeHours[i].EmployeeId == EmployeeHours[j].EmployeeId)
                                {
                                    CountClient = CountClient + 1;
                                    indexes.Add(j);
                                }
                            }
                            foreach (int index in indexes)
                            {
                                EmployeeHours[index].WeekDay = EmployeeHours[index].WeekDay / CountClient;
                                EmployeeHours[index].Sat = EmployeeHours[index].Sat / CountClient;
                                EmployeeHours[index].Sun = EmployeeHours[index].Sun / CountClient;
                                EmployeeHours[index].ActiveNightWeekDay = EmployeeHours[index].ActiveNightWeekDay / CountClient;
                                EmployeeHours[index].ActiveNightSat = EmployeeHours[index].ActiveNightSat / CountClient;
                                EmployeeHours[index].ActiveNightSun = EmployeeHours[index].ActiveNightSun / CountClient;
                                EmployeeHours[index].SleepOver = EmployeeHours[index].SleepOver / CountClient;

                            }
                            ids.Add(EmployeeHours[i].EmployeeId.ToString() + EmployeeHours[i].ShiftId.ToString());
                        }
                        //end
                    }
                }

                response = response.Success(EmployeeHours);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        public async Task<ApiResponse> Handle(GetEmployeeMyObHoursDetail request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                List<EmployeeHoursDetail> EmployeeHours = new List<EmployeeHoursDetail>();
                using (SqlConnection conn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_GetEmployeesHourDetail_V2", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@employeeid", request.SearchByEmpName);
                        cmd.Parameters.AddWithValue("@startDate", request.StartDate);
                        cmd.Parameters.AddWithValue("@endDate", request.EndDate);
                        cmd.Parameters.AddWithValue("@IsOnTime", request.IsOnTime);
                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            EmployeeHoursDetail objEmployeeHours = new EmployeeHoursDetail();
                            objEmployeeHours.ShiftTrackerId = Convert.ToInt32(reader["ShiftTrackerId"]);
                            objEmployeeHours.EmployeeId = reader["EmployeeId"] != System.DBNull.Value ? Convert.ToInt32(reader["EmployeeId"]) : 0;
                            objEmployeeHours.FirstName = Convert.ToString(reader["FirstName"]);
                            objEmployeeHours.LastName = Convert.ToString(reader["LastName"]);
                            objEmployeeHours.Total = Math.Round(Convert.ToSingle(reader["Unit"]), 2);
                            objEmployeeHours.StartDate = Convert.ToDateTime(reader["StartDate"]);
                            objEmployeeHours.EndDate = Convert.ToDateTime(reader["EndDate"]);
                            objEmployeeHours.PayrollCategory = Convert.ToString(reader["PayrollCategory"]);
                            objEmployeeHours.IsShiftOnTime = Convert.ToBoolean(reader["IsShiftOnTime"]);
                            objEmployeeHours.IsApproveByAccounts = Convert.ToBoolean(reader["IsApproveByAccounts"]);
                            objEmployeeHours.ShiftId = Convert.ToInt32(reader["Id"]);
                            // Added on 23 Feb 2021
                            objEmployeeHours.ClientFirstName = Convert.ToString(reader["ClientFirstName"]);
                            objEmployeeHours.ClientLastName = Convert.ToString(reader["ClientLastName"]);
                            objEmployeeHours.CodeDescription = Convert.ToString(reader["CodeDescription"]);
                            objEmployeeHours.JobCode = Convert.ToString(reader["JobCode"]);
                            // End added
                            objEmployeeHours.EndTime = TimeSpan.Parse(reader["endtime"].ToString());
                            objEmployeeHours.StartTime = TimeSpan.Parse(reader["starttime"].ToString());
                            objEmployeeHours.StartTimeString = objEmployeeHours.StartDate.Date.Add(objEmployeeHours.StartTime).ToString(@"hh\:mm tt");
                            objEmployeeHours.EndTimeString = objEmployeeHours.EndDate.Date.Add(objEmployeeHours.EndTime).ToString(@"hh\:mm tt");
                            // Added on 28 october
                            objEmployeeHours.actualEndTime = TimeSpan.Parse(reader["actualEndTime"].ToString());
                            objEmployeeHours.actualStartTime = TimeSpan.Parse(reader["actualStartTime"].ToString());
                            objEmployeeHours.actualStartDate = Convert.ToDateTime(reader["actualStartDate"]);
                            objEmployeeHours.actualEndDate = Convert.ToDateTime(reader["actualEndDate"]);                            
                            objEmployeeHours.actualStartTimeString = objEmployeeHours.actualStartDate.Date.Add(objEmployeeHours.actualStartTime).ToString(@"hh\:mm tt");
                            objEmployeeHours.actualEndTimeString = objEmployeeHours.actualEndDate.Date.Add(objEmployeeHours.actualEndTime).ToString(@"hh\:mm tt");
                            EmployeeHours.Add(objEmployeeHours);
                        }
                    }
                }

                response = response.Success(EmployeeHours);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        private int GetSleepOverCount(double sleepOver)
        {
            int value = 0;
            if (sleepOver > 0 && sleepOver <= 8)
            {
                value = 1;
            }
            else if (sleepOver > 8 && sleepOver <= 16)
            {
                value = 2;
            }
            else
            {
                value = 0;
            }
            return value;
        }

        public async Task<ApiResponse> Handle(UpdateRejectedShift request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var tracker = (from shiftdata in _dbContext.ShiftInfo
                               join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                               join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                               join emShiftTracker in _dbContext.EmployeeShiftTracker on shiftdata.Id equals emShiftTracker.ShiftId
                               where shiftdata.IsDeleted == false && shiftdata.IsActive == true && emInfo.IsActive == true && emInfo.IsDeleted == false && emInfo.Status == true
                               && emShiftTracker.Id == request.EmployeeTrackerId
                               select new
                               {
                                   emShiftTracker
                               }
                 ).FirstOrDefault();

                if (tracker != null)
                {
                    tracker.emShiftTracker.IsApproveByAccounts = true;
                    _dbContext.EmployeeShiftTracker.Update(tracker.emShiftTracker);
                    await _dbContext.SaveChangesAsync();
                    response.Success();
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
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("SqlConnection");
            return new SqlConnection(connectionString);
        }
    }
    public class EmployeeHours
    {
        public float Admin { get; set; }
        public float Cleaning { get; set; }

        public double WeekDay { get; set; }
        public double Sat { get; set; }
        public double Sun { get; set; }
        public double Holiday { get; set; }
        public double ActiveNightHoliday { get; set; }

        public double SleepOver { get; set; }
        public double ActiveNightWeekDay { get; set; }
        public double ActiveNightSat { get; set; }
        public double ActiveNightSun { get; set; }
        public double Total { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class EmployeeHoursDetail
    {
        public bool IsApproveByAccounts { get; set; }
        public bool IsShiftOnTime { get; set; }
        public int ShiftTrackerId { get; set; }
        public float Admin { get; set; }
        public float Cleaning { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public double WeekDay { get; set; }
        public double Sat { get; set; }
        public double Sun { get; set; }
        public double Holiday { get; set; }
        public double ActiveNight { get; set; }

        public double SleepOver { get; set; }
        public double ActiveNightWeekDay { get; set; }
        public double ActiveNightSat { get; set; }
        public double ActiveNightSun { get; set; }
        public double ActiveNightHoliday { get; set; }
        public double Total { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ShiftId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public TimeSpan actualStartTime { get; set; }
        public TimeSpan actualEndTime { get; set; }
        public string actualStartTimeString { get; set; }
        public string actualEndTimeString { get; set; }
        public DateTime actualStartDate { get; set; }
        public DateTime actualEndDate { get; set; }
        public string PayrollCategory { get; set; }
        public string JobCode { get; set; }
        public string CodeDescription { get; set; }
    }
}
