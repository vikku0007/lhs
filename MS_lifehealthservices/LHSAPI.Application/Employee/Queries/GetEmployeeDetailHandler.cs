
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
using System.Security.Cryptography;
using LHSAPI.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LHSAPI.Application.Employee.Queries
{
    public class GetEmployeeDetailHandler : IRequestHandler<GetEmployeeDetail, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;

        //   readonly ILoggerManager _logger;
        public GetEmployeeDetailHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

            // _logger = logger;
        }
        #region Employee
        /// <summary>
        /// Get Particular Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Handle(GetEmployeeDetail request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                EmployeeDetails _EmployeeDetails = new EmployeeDetails();
                List<LHSAPI.Application.Employee.Models.EmployeePrimaryInfoViewModel> Employeelist = new List<LHSAPI.Application.Employee.Models.EmployeePrimaryInfoViewModel>();
                var imageUrl = _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == request.Id).Select(x => x.Path).Any() ? _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == request.Id).Select(x => x.Path).FirstOrDefault() : null; 
                // var empList = _dbContext.EmployeePrimaryInfo.Where(x => x.IsDeleted == false && x.IsActive && x.Id == request.Id).FirstOrDefault();
              
                var empList = (from emp in _dbContext.EmployeePrimaryInfo
                               where emp.IsActive == true && emp.IsDeleted == false && emp.Id == request.Id
                               select new EmployeePrimaryInfoViewModel
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
                                   PasswordExist = request.Id,
                                   OtherLanguage=emp.OtherLanguage,
                                   IsAustralian = emp.IsAustralian
                               }).ToList();
                foreach (var item in empList)
                {
                    LHSAPI.Application.Employee.Models.EmployeePrimaryInfoViewModel comm = new LHSAPI.Application.Employee.Models.EmployeePrimaryInfoViewModel()
                    {
                        Id = item.Id,
                        Saluation = item.Saluation,
                        FirstName = item.FirstName,
                        MiddleName = item.MiddleName,
                        LastName = item.LastName,
                        DateOfBirth = item.DateOfBirth,
                        MaritalStatus = item.MaritalStatus,
                        MobileNo = item.MobileNo,
                        Gender = item.Gender,
                        EmailId = item.EmailId,
                        EmployeeId = item.EmployeeId,
                        EmployeeLevel = item.EmployeeLevel,
                        Status = item.Status,
                        Address1 = item.Address1,
                        City = item.City,
                        State = item.State,
                        Country = item.Country,
                        Code = item.Code,
                        Role = item.Role,
                        EmpType = item.EmpType,
                        Language = item.Language,
                        GenderName = _dbContext.StandardCode.Where(x => x.ID == item.Gender).Select(x => x.CodeDescription).FirstOrDefault(),
                        SalutationName = _dbContext.StandardCode.Where(x => x.ID == item.Saluation).Select(x => x.CodeDescription).FirstOrDefault(),
                        RoleName = _dbContext.StandardCode.Where(x => x.ID == item.Role).Select(x => x.CodeDescription).FirstOrDefault(),
                        Age = DateTime.Now.Year - Convert.ToDateTime(item.DateOfBirth).Year,
                        MaritalStatusName = _dbContext.StandardCode.Where(x => x.ID == item.MaritalStatus).Select(x => x.CodeDescription).FirstOrDefault(),
                        HasVisa = item.HasVisa,
                        VisaNumber = item.VisaNumber,
                        PassportNumber = item.PassportNumber,
                        VisaType = item.VisaType,
                        VisaExpiryDate = item.VisaExpiryDate,
                        VisaTypeName = _dbContext.StandardCode.Where(x => x.ID == item.VisaType).Select(x => x.CodeDescription).FirstOrDefault(),
                        CountryName = item.Country,
                        StateName = item.State,
                        ImageUrl = imageUrl,
                        Religion = item.Religion,
                        OtherHobbies = item.OtherHobbies,
                        OtherReligion=item.OtherReligion,
                        PasswordExist = item.Id,
                        OtherLanguage = item.OtherLanguage,
                        IsAustralian = item.IsAustralian
                    };
                    comm.EmployeeHobbiesModel = (from comminfo in _dbContext.EmployeeHobbies
                                                              where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.EmployeeId == request.Id
                                                              select new LHSAPI.Application.Employee.Models.EmployeeHobbiesModel
                                                              {
                                                                  Id = comminfo.Id,
                                                                  EmployeeId = comminfo.EmployeeId,
                                                                  Hobbies = comminfo.Hobbies,
                                                                  HobbiesName = _dbContext.StandardCode.Where(x => x.ID == comminfo.Hobbies).Select(x => x.CodeDescription).FirstOrDefault(),
                                                              }).OrderByDescending(x => x.Id).ToList();

                    Employeelist.Add(comm);

                }
                var EmployeeData = Employeelist.FirstOrDefault();

                if (empList != null)
                {
                    _EmployeeDetails.EmployeePicInfo = _dbContext.EmployeePicInfo.Where(x => x.IsDeleted == false && x.IsActive && x.EmployeeId == request.Id).FirstOrDefault();
                    _EmployeeDetails.EmployeeMiscInfo = (from emp in _dbContext.EmployeeMiscInfo
                                                         where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.Id
                                                         select new LHSAPI.Application.Employee.Models.EmployeeMiscInfo
                                                         {
                                                             Id = emp.Id,
                                                             EmployeeId = emp.EmployeeId,
                                                             Ethnicity = emp.Ethnicity,
                                                             Religion = emp.Religion,
                                                             Weight = emp.Weight,
                                                             Height = emp.Height,
                                                             Smoker = emp.Smoker,
                                                             EthnicityName = _dbContext.StandardCode.Where(x => x.ID == emp.Ethnicity).Select(x => x.CodeDescription).FirstOrDefault(),
                                                             ReligionName = _dbContext.StandardCode.Where(x => x.ID == emp.Religion).Select(x => x.CodeDescription).FirstOrDefault()
                                                         }).FirstOrDefault();

                    _EmployeeDetails.EmployeeKinInfo = (from emp in _dbContext.EmployeeKinInfo
                                                        where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.Id
                                                        select new LHSAPI.Application.Employee.Models.EmployeeKinInfo
                                                        {
                                                            Id = emp.Id,
                                                            EmployeeId = emp.EmployeeId,
                                                            FirstName = emp.FirstName,
                                                            MiddelName = emp.MiddelName,
                                                            LastName = emp.LastName,
                                                            RelationShip = emp.RelationShip,
                                                            ContactNo = emp.ContactNo=="null"?"": emp.ContactNo,
                                                            Email = emp.Email,
                                                            OtherRelation = emp.OtherRelation,
                                                            RelationShipName = _dbContext.StandardCode.Where(x => x.ID == emp.RelationShip).Select(x => x.CodeDescription).FirstOrDefault()
                                                        }).FirstOrDefault();
                    _EmployeeDetails.EmployeeAwardInfo = (from emp in _dbContext.EmployeeAwardInfo
                                                          where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.Id
                                                          select new LHSAPI.Application.Employee.Models.EmployeeAwardInfo
                                                          {
                                                              Id = emp.Id,
                                                              EmployeeId = emp.EmployeeId,
                                                              AwardGroup = emp.AwardGroup,
                                                              Allowances = emp.Allowances,
                                                              Dailyhours = emp.Dailyhours,
                                                              Weeklyhours = emp.Weeklyhours,
                                                              AwardGroupName = _dbContext.StandardCode.Where(x => x.ID == emp.AwardGroup).Select(x => x.CodeDescription).FirstOrDefault(),

                                                          }).FirstOrDefault();

                    _EmployeeDetails.EmployeeDrivingLicenseInfo = (from emp in _dbContext.EmployeeDrivingLicenseInfo
                                                                   where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.Id
                                                                   select new LHSAPI.Application.Employee.Models.EmployeeDriverLicenseModel
                                                                   {
                                                                       Id = emp.Id,
                                                                       EmployeeId = emp.EmployeeId,
                                                                       DrivingLicense = emp.DrivingLicense,
                                                                       CarInsurance = emp.CarInsurance,
                                                                       CarRegExpiryDate = emp.CarRegExpiryDate,
                                                                       CarRegNo = emp.CarRegNo,
                                                                       LicenseType = emp.LicenseType,
                                                                       LicenseState = emp.LicenseState,
                                                                       LicenseNo = emp.LicenseNo,
                                                                       LicenseExpiryDate = emp.LicenseExpiryDate,
                                                                       InsuranceExpiryDate = emp.InsuranceExpiryDate,
                                                                       LicenseTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.LicenseType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                       LicenseStateName = _dbContext.StandardCode.Where(x => x.ID == emp.LicenseState).Select(x => x.CodeDescription).FirstOrDefault()
                                                                   }).FirstOrDefault();


                    _EmployeeDetails.EmployeeEducation = (from emp in _dbContext.EmployeeEducation
                                                          where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.Id
                                                          select new LHSAPI.Application.Employee.Models.EmployeeEducationModel
                                                          {
                                                              Id = emp.Id,
                                                              EmployeeId = emp.EmployeeId,
                                                              Institute = emp.Institute,
                                                              Degree = emp.Degree,
                                                              FieldStudy = emp.FieldStudy,
                                                              CompletionDate = emp.CompletionDate,
                                                              AdditionalNotes = emp.AdditionalNotes,
                                                              DocumentPath = emp.DocumentPath,
                                                              QualificationType = emp.QualificationType,
                                                              QualificationTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.QualificationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                              CreatedDate = emp.CreatedDate
                                                          }).ToList();
                    _EmployeeDetails.EmployeeJobProfile = (from emp in _dbContext.EmployeeJobProfile
                                                           where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.Id
                                                           select new LHSAPI.Application.Employee.Models.EmployeeJobProfile
                                                           {
                                                               Id = emp.Id,
                                                               EmployeeId = emp.EmployeeId,
                                                               DateOfJoining = emp.DateOfJoining,
                                                               DepartmentName = emp.DepartmentId != null ? _dbContext.StandardCode.Where(x => x.ID == emp.DepartmentId).Select(x => x.CodeDescription).FirstOrDefault() : "",
                                                               DistanceTravel = emp.DistanceTravel,
                                                               // LocationId = emp.LocationId,
                                                               // LocationName = emp.OtherLocation != "" ? emp.OtherLocation:_dbContext.Location.Where(x => x.LocationId == emp.LocationId).Select(x => x.Name).FirstOrDefault(),
                                                               DepartmentId = emp.DepartmentId,
                                                               JobDesc = emp.JobDesc,
                                                               ReportingToId = emp.ReportingToId,
                                                               ReportingToName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == emp.ReportingToId).Select(x => string.Join(' ', x.FirstName, x.MiddleName, x.LastName)).FirstOrDefault(),
                                                               Source = emp.Source,
                                                               SourceName = _dbContext.StandardCode.Where(x => x.ID == emp.Source).Select(x => x.CodeDescription).FirstOrDefault(),
                                                               WorkingHoursWeekly = emp.WorkingHoursWeekly,
                                                               //  LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == emp.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                               // OtherLocation = emp.OtherLocation,
                                                               // LocationType=emp.LocationType
                                                           }).FirstOrDefault();
                    _EmployeeDetails.EmployeeWorkExp = _dbContext.EmployeeWorkExp.Where(x => x.IsDeleted == false && x.IsActive && x.EmployeeId == request.Id).ToList();
                    _EmployeeDetails.EmployeePayRate = _dbContext.EmployeePayRate.Where(x => x.IsDeleted == false && x.IsActive && x.EmployeeId == request.Id).FirstOrDefault();
                    _EmployeeDetails.EmployeeTraining = (from emp in _dbContext.EmployeeTraining
                                                         where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.Id
                                                         select new LHSAPI.Application.Employee.Models.EmployeeTraining
                                                         {
                                                             Id = emp.Id,
                                                             EmployeeId = emp.Id,
                                                             MandatoryTraining = emp.MandatoryTraining,
                                                             TrainingType = emp.TrainingType,
                                                             CourseType = emp.CourseType,
                                                             StartDate = emp.StartDate,
                                                             EndDate = emp.EndDate,
                                                             Remarks = emp.Remarks,
                                                             IsAlert = emp.IsAlert,
                                                             CourseTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.CourseType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                             MandatoryName = _dbContext.StandardCode.Where(x => x.ID == emp.MandatoryTraining).Select(x => x.CodeDescription).FirstOrDefault(),
                                                             TrainingTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.TrainingType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                             FileName = emp.FileName
                                                         }).ToList();
                   
                    _EmployeeDetails.EmployeePrimaryInfo = EmployeeData;
                 if (_EmployeeDetails.EmployeeMiscInfo == null) _EmployeeDetails.EmployeeMiscInfo = new LHSAPI.Application.Employee.Models.EmployeeMiscInfo();
                    if (_EmployeeDetails.EmployeeKinInfo == null) _EmployeeDetails.EmployeeKinInfo = new LHSAPI.Application.Employee.Models.EmployeeKinInfo();
                    if (_EmployeeDetails.EmployeeAwardInfo == null) _EmployeeDetails.EmployeeAwardInfo = new LHSAPI.Application.Employee.Models.EmployeeAwardInfo();
                    if (_EmployeeDetails.EmployeeDrivingLicenseInfo == null) _EmployeeDetails.EmployeeDrivingLicenseInfo = new LHSAPI.Application.Employee.Models.EmployeeDriverLicenseModel();
                    if (_EmployeeDetails.EmployeeJobProfile == null) _EmployeeDetails.EmployeeJobProfile = new LHSAPI.Application.Employee.Models.EmployeeJobProfile();
                    if (_EmployeeDetails.EmployeeWorkExp == null) _EmployeeDetails.EmployeeWorkExp = new List<Domain.Entities.EmployeeWorkExp>();
                    if (_EmployeeDetails.EmployeePayRate == null) _EmployeeDetails.EmployeePayRate = new Domain.Entities.EmployeePayRate();
                    if (_EmployeeDetails.EmployeeEducation == null) _EmployeeDetails.EmployeeEducation = new List<LHSAPI.Application.Employee.Models.EmployeeEducationModel>();
                    response.SuccessWithOutMessage(_EmployeeDetails);
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
        #endregion
    }
}
