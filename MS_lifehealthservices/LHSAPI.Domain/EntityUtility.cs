using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Domain
{
  public class EntityUtility
  {
    public const string SavedMessage = " Saved Successfully";
    public const string DeletedMessage = " Deleted Successfully";
    public const string UpdatedMessage = " Updated Successfully";
    public static string GetUserReadableEntityName(string name)
    {
      string _Name = string.Empty;

      switch (name)
      {
        case "ClientPicInfo":
          _Name = "Client Pic";
          break;
        case "ClientPrimaryCareInfo":
          _Name = "Client PrimaryCare";
          break;
        case "ClientPrimaryIncidentCategory":
          _Name = "Client Primary Incident Categoty";
          break;
        case "ClientPrimaryInfo":
          _Name = "Client Primary info";
          break;
        case "ClientProgressNotes":
          _Name = "Client Progress note";
          break;
        
        case "ClientSocialConnections":
          _Name = "Client Social Connection";
          break;
        case "ClientSupportCoordinatorInfo":
          _Name = "Client Support Coordinator";
          break;
        case "EmployeeAccidentInfo":
          _Name = "Employee AccidentInfo";
          break;
        case "EmployeeAppraisalDetails":
          _Name = "Employee Appraisal Details";
          break;
        case "EmployeeAwardInfo":
          _Name = "Employee Award Info";
          break;
        case "EmployeeCompliancesDetails":
          _Name = "Employee Compliances Details";
          break;
        case "EmployeeDrivingLicenseInfo":
          _Name = "Employee Driving LicenseInfo";
          break;
        case "EmployeeEducation":
          _Name = "Employee Education";
          break;
        case "EmployeeJobProfile":
           _Name = "Employee JobProfile";
          break;
        case "EmployeeKinInfo":
           _Name = "Employee KinInfo";
          break;
        case "EmployeeLeaveInfo":
           _Name = "Employee Leave Info";
          break;
        case "EmployeeMiscInfo":
           _Name = "Employee Misc Info";
          break;
        case "EmployeePayRate":
           _Name = "Employee Pay Rate";
          break;
        case "EmployeePicInfo":
           _Name = "Employee Pic Info";
          break;
        case "EmployeePrimaryInfo":
           _Name = "Employee Primary Info";
          break;
        case "EmployeeWorkExp":
           _Name = "Employee Work Experience";
          break;
        case "ShiftInfo":
           _Name = "Shift";
          break;
        case "ShiftTemplate":
           _Name = "Shift Template";
          break;
        case "ClientMedicalHistory":
           _Name = "Client Medical History";
          break;
        case "ToDoShiftItem":
           _Name = "ToDo Item";
          break;
        case "ClientGuardianInfo":
           _Name = "Client Guardian Info";
          break;
        default:
          _Name = name;
          break;
      }

      return _Name;
    }

  }
}
