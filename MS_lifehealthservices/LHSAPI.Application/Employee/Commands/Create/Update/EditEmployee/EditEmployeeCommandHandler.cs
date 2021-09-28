using LHSAPI.Application.Employee.Models;
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Create.Update.EditEmployee
{
    class EditEmployeeCommandHandler : IRequestHandler<EditEmployeeCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public EditEmployeeCommandHandler(LHSDbContext context, ISessionService ISessionService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _ISessionService = ISessionService;
      _userManager = userManager;
      _roleManager = roleManager;
    }

        public async Task<ApiResponse> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            List<EmployeeHobbies> HobbyList = new List<EmployeeHobbies>();
            List<EmployeeHobbiesModel> HobbyData = new List<EmployeeHobbiesModel>();
            try
            {
                if (request.Id > 0)
                {

                    var ExistEmp = _context.EmployeePrimaryInfo.FirstOrDefault(x => x.Id == request.Id & x.IsActive == true);
                    if (ExistEmp != null)
                    {
                        int existRole = ExistEmp.Role.Value; 
                        ExistEmp.FirstName = request.Firstname;
                        ExistEmp.LastName = request.LastName;
                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                        ExistEmp.City = request.City;
                        ExistEmp.State = request.State;
                        ExistEmp.Country = request.Country;
                        ExistEmp.DateOfBirth = request.DateOfBirth;
                        ExistEmp.EmailId = request.EmailId;
                        ExistEmp.Code = request.Code;
                        ExistEmp.Address1 = request.Address1;
                        ExistEmp.MobileNo = request.MobileNo;
                        ExistEmp.Role = request.Role;
                        ExistEmp.MiddleName = request.MiddleName;
                        ExistEmp.Saluation = request.Salutation;
                        ExistEmp.Status = request.Status;
                        ExistEmp.EmployeeLevel = request.EmployeeLevel;
                        ExistEmp.MaritalStatus = request.MaritalStatus;
                        ExistEmp.Language = request.Language;
                        ExistEmp.EmpType = request.EmpType;
                        ExistEmp.EmployeeId = request.EmployeeId;
                        ExistEmp.IsActive = true;
                        ExistEmp.HasVisa = request.HasVisa;
                        ExistEmp.PassportNumber = request.PassportNumber;
                        ExistEmp.VisaNumber = request.VisaNumber;
                        ExistEmp.VisaType = request.VisaType;
                        ExistEmp.VisaExpiryDate = request.VisaExpiryDate;
                        ExistEmp.Religion = request.Religion;
                        ExistEmp.OtherHobbies = request.OtherHobbies;
                        ExistEmp.OtherReligion = request.OtherReligion;
                        ExistEmp.OtherLanguage = request.OtherLanguage;
                        ExistEmp.IsAustralian = request.IsAustralian;
            _context.Database.BeginTransaction();
                        _context.EmployeePrimaryInfo.Update(ExistEmp);
                        await _context.SaveChangesAsync();
            if(existRole != request.Role)
            {
              var objNew = await _userManager.FindByEmailAsync(ExistEmp.EmailId);
              if (objNew !=null)
              {
                var currentRole = GetRoles(request.Role).ToString();
                var Privious = existRole > 0 ? GetRoles(existRole).ToString() : UserRole.Employee.ToString();
              
                  await _roleManager.CreateAsync(new IdentityRole(currentRole));

             var r1 =   await _userManager.RemoveClaimAsync(objNew, new Claim("Roles", Privious));
                
              var r2 =  await _userManager.RemoveFromRoleAsync(objNew, Privious);
                await _userManager.AddClaimAsync(objNew, new Claim("Roles", currentRole));
                await _userManager.AddToRoleAsync(objNew, currentRole);
                await _context.SaveChangesAsync();
              }

            }




                        var Existstandard = _context.EmployeeHobbies.Where(x => x.EmployeeId == ExistEmp.Id && x.IsDeleted == false && x.IsActive == true).ToList();
                        if (Existstandard != null)
                        {
                            foreach (var item in Existstandard)
                            {
                                var StandardResult = _context.EmployeeHobbies.FirstOrDefault(x => x.Id == item.Id && x.IsDeleted == false && x.IsActive == true);
                                StandardResult.IsDeleted = true;
                                StandardResult.IsActive = false;
                                StandardResult.DeletedDate = DateTime.UtcNow;
                                StandardResult.DeletedById = await _ISessionService.GetUserId();
                                _context.EmployeeHobbies.Update(StandardResult);
                                await _context.SaveChangesAsync();
                            }
                        }
                       
                       
                        foreach (var id in request.Hobbies)
                        {
                            EmployeeHobbies comm1 = new EmployeeHobbies();
                            comm1.EmployeeId = ExistEmp.Id;
                            comm1.Hobbies = id;
                            comm1.CreatedById = await _ISessionService.GetUserId();
                            comm1.CreatedDate = DateTime.Now;
                            comm1.IsActive = true;
                            await _context.EmployeeHobbies.AddAsync(comm1);
                            HobbyList.Add(comm1);
                            _context.SaveChanges();
                        }
                       
                        foreach (var item in HobbyList)
                        {
                            LHSAPI.Application.Employee.Models.EmployeeHobbiesModel Hobby = new LHSAPI.Application.Employee.Models.EmployeeHobbiesModel()
                            {
                                EmployeeId = item.EmployeeId,
                                Hobbies = item.Hobbies,
                               
                            };
                            HobbyData.Add(Hobby);
                        }
                      
                        EmployeePrimaryInfoViewModel model = new EmployeePrimaryInfoViewModel();
                        model.Id = request.Id;
                        model.FirstName = request.Firstname;
                        model.LastName = request.LastName;
                        model.City = request.City;
                        model.State = request.State;
                        model.Country = request.Country;
                        model.DateOfBirth = request.DateOfBirth;
                        model.EmailId = request.EmailId;
                        model.Code = request.Code;
                        model.Address1 = request.Address1;
                        model.MobileNo = request.MobileNo;
                        model.Role = request.Role;
                        model.MiddleName = request.MiddleName;
                        model.Saluation = request.Salutation;
                        model.Status = request.Status;
                        model.EmployeeLevel = request.EmployeeLevel;
                        model.MaritalStatus = request.MaritalStatus;
                        model.Language = request.Language;
                        model.EmpType = request.EmpType;
                        model.EmployeeId = request.EmployeeId;
                        model.Age = DateTime.Now.Year - Convert.ToDateTime(model.DateOfBirth).Year;
                        model.RoleName = _context.StandardCode.Where(x => x.ID == model.Role).Select(x => x.CodeDescription).FirstOrDefault();
                        model.GenderName = _context.StandardCode.Where(x => x.ID == ExistEmp.Gender).Select(x => x.CodeDescription).FirstOrDefault();
                        model.SalutationName = _context.StandardCode.Where(x => x.ID == model.Saluation).Select(x => x.CodeDescription).FirstOrDefault();
                        model.MaritalStatusName = _context.StandardCode.Where(x => x.ID == ExistEmp.MaritalStatus).Select(x => x.CodeDescription).FirstOrDefault();
                        var imageUrl = _context.EmployeePicInfo.Where(x => x.EmployeeId == request.Id).Select(x => x.Path).Any() ? _context.EmployeePicInfo.Where(x => x.EmployeeId == request.Id).Select(x => x.Path).FirstOrDefault() : null; ;
                        model.ImageUrl = imageUrl;
                        model.HasVisa = request.HasVisa;
                        model.PassportNumber = request.PassportNumber;
                        model.VisaNumber = request.VisaNumber;
                        model.VisaType = request.VisaType;
                        model.VisaExpiryDate = request.VisaExpiryDate;
                        model.CountryName = request.Country;
                        model.StateName = request.State;
                        model.Religion = request.Religion;
                        model.OtherHobbies = request.OtherHobbies;
                        model.OtherLanguage = request.OtherLanguage;
                        model.IsAustralian = request.IsAustralian;
                        model.EmployeeHobbiesModel = HobbyData;
                        response.Update(model);
            _context.Database.CommitTransaction();

          }
                    else
                    {
                        response.NotFound();


                    }
                }
                else
                {
                    response.ValidationError();
                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
        _context.Database.RollbackTransaction();
      }
            return response;

        }
    private UserRole GetRoles(int id)
    {
      UserRole role;
      var str = _context.StandardCode.Where(x=>x.ID == id).FirstOrDefault().CodeDescription;
      switch (str)
      {
        case "Chief Executive Officer":
          {
            role = UserRole.ChiefExecutiveOfficer;
            break;
          }
        case "Operations Manager (Proposed)":
          {
            role = UserRole.OperationsManager;
            break;
          }
        case "General Manager":
          {
            role = UserRole.GeneralManager;
            break;
          }

        case "Compliance Officer":
          {
            role = UserRole.ComplianceOfficer;
            break;
          }
        case "Care Coordinator":
          {
            role = UserRole.CareCoordinator;
            break;
          }
        case "Accounts Officer":
          {
            role = UserRole.AccountsOfficer;
            break;
          }
        case "Incident Reporting Officer":
          {
            role = UserRole.IncidentReportingOfficer;
            break;
          }
        case "House Team Leader":
          {
            role = UserRole.HouseTeamLeader;
            break;
          }
        case "Human Resources Officer":
          {
            role = UserRole.HumanResourcesOfficer;
            break;
          }
        case "Disability Support Worker":
          {
            role = UserRole.DisabilitySupportWorker;
            break;
          }
        case "Mental Health Worker/ Disability Support Worker":
          {
            role = UserRole.MentalHealthWorkerOrDisabilitySupportWorker;

            break;
          }
        default:
          {
            role = UserRole.HouseTeamLeader;
            break;
          }
      }

      return role;
    }
  }
}
