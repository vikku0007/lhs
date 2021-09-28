using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Common.Enums
{
  public class ResponseEnums
  {
    public enum Number
    {
      One = 1,
      Zero = 0
    }

    public enum UserRole
    {
      SuperAdmin = 1,
      Admin = 2,
      Employee = 3,
      Client = 4,
      ChiefExecutiveOfficer = 5,
      GeneralManager = 6,
      OperationsManager = 7,
      ComplianceOfficer = 8,
      CareCoordinator = 9,
      AccountsOfficer = 10,
      IncidentReportingOfficer = 11,
      HouseTeamLeader = 12,
      HumanResourcesOfficer = 13,
      DisabilitySupportWorker = 14,
      MentalHealthWorkerOrDisabilitySupportWorker = 15,
    }


    public enum Gender
    {
      Male = 1,
      Female = 2
    }
    public enum Salutation
    {
      Mr,
      Miss,
      Mrs
    }
    public enum Roles
    {
      CareCoordinator,
      Navigator,
      HealthCoach,
      MedicalScribe,
      TelehealthtrainedPhysician,
      NursePractitioner,
      PhysicianAssistant,
      Manager,
      Accountant,
      HRManager,
      AdminManager
    }
    public enum EmployeeType
    {
      Contract,
      Hourly,
      ParttimeSalaried
    }
    public enum Language
    {
      English,
      African,
      Spanish,
      German,
      Russian
    }
    public enum MaritalStatus
    {
      Single,
      Married,
      Divorced
    }
    public enum Relationship
    {
      Brother,
      Sister,
      Mother,
      Father,
      Uncle,
      Aunt,
      Friend,
      Relative
    }

    public enum Department
    {
      HealthCare,
      Admin,
      InformationTechnology,
      HumanResource,
      Accounts
    }

    public enum SourceOfHire
    {
      Referral,
      Agency,
      Advertisement,
      DirectSourcing
    }

    public enum StandardCode
    {
      Department,
      EmployeeType,
      EventType,
      Gender,
      Language,
      Level,
      MaritalStatus,
      Relationship,
      Roles,
      Salutation,
      SourceOfHire,
      OffenseType,
      WarningType,
      AppraisalType,
      AwardGroup,
      FundType,
      DocumentType,
      ServiceType,
      DocumentName,
      OtherDocumentName,
      LeaveType,
      VisaType,
      ShiftStatus,
      SymptomsType,
      ComplianceType,
      ConditionType,
      Ethnicity,
      Religion,
      Country,
      State,
      CourseType,
      MandatoryTraining,
      OptionalTraining,
      TrainingType,
      DriverLicenseType,
      LicenseType,
      CodeData,
      LocationType,
      Hobbies,
      QualificationType,
      AppraisalCrieteria,
      PaymentTerm,
      Payers,
      PrimaryCategory,
      SecondaryCategory,
      PrimaryDisability,
      SecondaryDisability,
      CommunicationType,
      ConcernBehaviour,
      ShiftTime,
      AMShift,
      PMShift,
      ActiveNightShift,
      MandatoryDocument,
      Optionaldocument,
      Year
     }
     
    }
  }



  
