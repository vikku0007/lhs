
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


namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeDashBoardInfoMobile
{
    public class GetEmployeeDashBoardDetailHandler : IRequestHandler<GetEmployeeDashBoardDetail, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public GetEmployeeDashBoardDetailHandler(LHSDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        private SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("SqlConnection");
            return new SqlConnection(connectionString);
        }
        public async Task<ApiResponse> Handle(GetEmployeeDashBoardDetail request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            EmployeeDashBoardDetail details = new EmployeeDashBoardDetail();
            try
            {
                var imageUrl = _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.Path).Any() ? _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.Path).FirstOrDefault() : null; ;

                details.EmployeeDetail = (from emp in _dbContext.EmployeePrimaryInfo
                                          where emp.IsActive == true && emp.IsDeleted == false && emp.Id == request.EmployeeId
                                          select new EmployeeDetail
                                          {
                                              Id = emp.Id,
                                              Saluation = emp.Saluation,
                                              FirstName = emp.FirstName,
                                              MiddleName = emp.MiddleName,
                                              LastName = emp.LastName,
                                              DateOfBirth = emp.DateOfBirth,
                                              MaritalStatus = emp.MaritalStatus,
                                              MobileNo = emp.MobileNo,
                                              Gender = emp.Gender,
                                              EmailId = emp.EmailId,
                                              EmployeeId = emp.EmployeeId,
                                              EmployeeLevel = emp.EmployeeLevel,
                                              Status = emp.Status,
                                              Address1 = emp.Address1,
                                              City = emp.City,
                                              State = emp.State,
                                              Country = emp.Country,
                                              Code = emp.Code,
                                              Role = emp.Role,
                                              EmpType = emp.EmpType,
                                              Language = emp.Language,
                                              GenderName = _dbContext.StandardCode.Where(x => x.ID == emp.Gender).Select(x => x.CodeDescription).FirstOrDefault(),
                                              SalutationName = _dbContext.StandardCode.Where(x => x.ID == emp.Saluation).Select(x => x.CodeDescription).FirstOrDefault(),
                                              RoleName = _dbContext.StandardCode.Where(x => x.ID == emp.Role).Select(x => x.CodeDescription).FirstOrDefault(),
                                              Age = DateTime.Now.Year - Convert.ToDateTime(emp.DateOfBirth).Year,
                                              MaritalStatusName = _dbContext.StandardCode.Where(x => x.ID == emp.MaritalStatus).Select(x => x.CodeDescription).FirstOrDefault(),
                                              HasVisa = emp.HasVisa,
                                              VisaNumber = emp.VisaNumber,
                                              PassportNumber = emp.PassportNumber,
                                              VisaType = emp.VisaType,
                                              VisaExpiryDate = emp.VisaExpiryDate,
                                              VisaTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.VisaType).Select(x => x.CodeDescription).FirstOrDefault(),
                                              CountryName = emp.Country,
                                              StateName = emp.State,
                                              ImageUrl = imageUrl,
                                              Religion = emp.Religion,
                                              OtherHobbies = emp.OtherHobbies,
                                              OtherReligion = emp.OtherReligion,
                                              PasswordExist = request.EmployeeId,
                                              //EmployeeLevelName = _dbContext.StandardCode.Where(x => x.ID == emp.EmployeeLevel).Select(x => x.CodeDescription).FirstOrDefault(),
                                              EmployeeLevelName = _dbContext.StandardCode.Where(x => x.Value == emp.EmployeeLevel && x.CodeData == Common.Enums.ResponseEnums.StandardCode.Level.ToString() && x.IsActive == true).Select(x => x.CodeDescription).FirstOrDefault(),
                                          }).FirstOrDefault();
                details.EmployeeDetail.EmployeeHobbiesModel = (from comminfo in _dbContext.EmployeeHobbies
                                                               where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.EmployeeId == request.EmployeeId
                                                               select new EmployeeHobbiesModel
                                                               {
                                                                   Id = comminfo.Id,
                                                                   EmployeeId = comminfo.EmployeeId,
                                                                   Hobbies = comminfo.Hobbies,
                                                                   HobbiesName = _dbContext.StandardCode.Where(x => x.ID == comminfo.Hobbies).Select(x => x.CodeDescription).FirstOrDefault(),
                                                               }).OrderByDescending(x => x.Id).ToList();

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
                            details.EmployeeDashBoardHours = new EmployeeDashBoardHours();
                            details.EmployeeDashBoardHours.ApprovedShifts = reader["ApprovedShifts"] != System.DBNull.Value ? Convert.ToInt64(reader["ApprovedShifts"]) : 0;
                            details.EmployeeDashBoardHours.CancelHours = reader["CancelHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["CancelHours"]), 2) : 0;
                            details.EmployeeDashBoardHours.PendingShifts = reader["PendingShifts"] != System.DBNull.Value ? Convert.ToInt64(reader["PendingShifts"]) : 0;
                            details.EmployeeDashBoardHours.TotalHours = reader["TotalHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["TotalHours"]), 2) : 0;
                            details.EmployeeDashBoardHours.CompletedHours = reader["CompletedHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["CompletedHours"]), 2) : 0;
                            details.EmployeeDashBoardHours.UnallocatedHours = reader["UnallocatedHours"] != System.DBNull.Value ? Math.Round(Convert.ToDouble(reader["UnallocatedHours"]), 2) : 0;
                        }
                    }
                }
                response = response.Success(details);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;


        }

        public class EmployeeDashBoardDetail
        {
            public EmployeeDetail EmployeeDetail { get; set; }
            public EmployeeDashBoardHours EmployeeDashBoardHours { get; set; }
        }
        public class EmployeeDetail
        {
            public int Id { get; set; }
            public int? Saluation { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }

            public string LastName { get; set; }

            public int? Role { get; set; }

            public DateTime? DateOfBirth { get; set; }

            public int? MaritalStatus { get; set; }
            public string MaritalStatusName { get; set; }

            public string MobileNo { get; set; }

            public int? Gender { get; set; }

            public string EmailId { get; set; }

            public int EmployeeId { get; set; }

            public int? EmployeeLevel { get; set; }
            public string EmployeeLevelName { get; set; }

            public bool Status { get; set; }

            public int? LocationId { get; set; }

            public string Address1 { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string Country { get; set; }

            public int? Code { get; set; }
            public string RoleName { get; set; }

            public int? Language { get; set; }
            public int? EmpType { get; set; }

            public string ImageUrl { get; set; }

            public string GenderName { get; set; }
            public string SalutationName { get; set; }
            public int Age { get; set; }
            public Nullable<DateTime> DateOfJoining { get; set; }
            public string FullName { get; set; }

            public bool HasVisa { get; set; }
            public string PassportNumber { get; set; }
            public string VisaNumber { get; set; }
            public int? VisaType { get; set; }
            public DateTime? VisaExpiryDate { get; set; }

            public string VisaTypeName { get; set; }
            public string StateName { get; set; }

            public string CountryName { get; set; }

            public Nullable<DateTime> CreatedDate { get; set; }
            public int? Religion { get; set; }
            public string OtherHobbies { get; set; }
            public string OtherReligion { get; set; }
            public string HobbiesName { get; set; }
            public int PasswordExist { get; set; }
            public List<EmployeeHobbiesModel> EmployeeHobbiesModel { get; set; }

        }

        public class EmployeeHobbiesModel
        {

            public int Id { get; set; }
            public int Hobbies { get; set; }

            public int EmployeeId { get; set; }
            public string HobbiesName { get; set; }
        }
        public class EmployeeDashBoardHours
        {
            public double TotalHours { get; set; }            
            public double CancelHours { get; set; }            
            public double UnallocatedHours { get; set; }
            public double CompletedHours { get; set; }
            public long ApprovedShifts { get; set; }
            public long PendingShifts { get; set; }
        }
    }

}
