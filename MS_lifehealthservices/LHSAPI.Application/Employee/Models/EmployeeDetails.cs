using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
  public class EmployeeDetails
  {

    public EmployeePrimaryInfoViewModel EmployeePrimaryInfo  { get; set; }   
    public EmployeeMiscInfo EmployeeMiscInfo { get; set; }
    public LHSAPI.Application.Employee.Models.EmployeeKinInfo EmployeeKinInfo { get; set; }
    public EmployeeAwardInfo EmployeeAwardInfo { get; set; }
    public EmployeePicInfo EmployeePicInfo { get; set; }
    public LHSAPI.Application.Employee.Models.EmployeeDriverLicenseModel EmployeeDrivingLicenseInfo { get; set; }
    public List<EmployeeEducationModel> EmployeeEducation { get; set; }
    public LHSAPI.Application.Employee.Models.EmployeeJobProfile EmployeeJobProfile { get; set; }
    
    public List<EmployeeWorkExp> EmployeeWorkExp { get; set; }
    public EmployeePayRate EmployeePayRate { get; set; }
     public List<EmployeeTraining> EmployeeTraining { get; set; }
     public LHSAPI.Application.Employee.Models.EmployeeToDoShift EmployeeToDoShift { get; set; }
     public List<LHSAPI.Application.Employee.Models.EmployeeToDoListShift> EmployeeToDoListShift { get; set; }
     public List<LHSAPI.Application.Employee.Models.EmployeeHobbiesModel> EmployeeHobbiesModels { get; set; }
     
    }
}
