using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Common.Enums
{
    public class Employee
    {
        public enum EmployeeOrderBy
        {
            Name,
            MobileNo,
            Email,
            DateOfEmployment,
            CreatedDate
        }
        public enum EmployeeAccidentOrderBy
        {
            Name,
            IncidentType,
            IncidentDate,
            LocationType,
            Location,
            ReportedTo,
            RaisedBy,
            Description,
            ActionTaken
        }
        public enum EmployeeCommunicationOrderBy
        {
            Name,
            Subject,
            DateTime,

        }
        public enum EmployeeCommunicationInfoOrderBy
        {

            Subject,
            DateTime,
            Message

        }
        public enum EmployeeAccidentinfoOrderBy
        {

            EventType,
            AccidentDate,
            Locationtype,
            Location,
            ReportedTo,
            RaisedBy,
            Description,
            ActionTaken,
            IncidentTimeName
        }
        public enum EmployeeAppraisalOrderBy
        {
            Name,
            EmployeedetailId,
            AppraisalType,
            AppraisalDateFrom,
            AppraisalDateTo

        }
        public enum EmployeeLeaveOrderBy
        {
            Name,
            LeaveType,
            DateFrom,
            DateTo,
            Reason,
            LeaveStatus


        }
        public enum EmployeeLeaveInfoOrderBy
        {

            LeaveType,
            DateFrom,
            DateTo,
            Reason


        }
        public enum ApplyLeaveInfoOrderBy
        {
            FullName,
            LeaveType,
            DateFrom,
            DateTo



        }
        public enum EmployeeStaffOrderBy
        {
            Name,
            warningType,
            description,
            improvementPlan,
            offensestype,
            otheroffenses

        }
        public enum EmployeeStaffInfoOrderBy
        {

            WarningType,
            offensestype,
            Description,
            ImprovementPlan,
            otheroffenses


        }
        public enum EmployeeComplianceOrderBy
        {
            Name,
            document,
            description,
            dateofissue,
            hasExpiry,
            dateOfExpiry,
            alert

        }
        public enum EmployeeEducationOrderBy
        {
            QualificationType,
            Institute,
            Degree,
            FieldStudy,
            CompletionDate,
            AdditionalNotes

        }
        public enum EmployeeExperienceOrderBy
        {
            Company,
            JobTitle,
            StarDate,
            EndDate,
            JobDesc

        }
        public enum EmployeeTrainingOrderBy
        {
            TrainingType,
            Training,
            CourseType,
            StartDate,
            EndDate,
            Duration,
            Remarks

        }
        public enum StandardCodeOrderBy
        {
            CodeData,
            CodeDescription


        }

        public enum HolidayOrderBy
        {
            Year,
            Holiday,
            DateFrom,
            DateTo


        }
        public enum EmployeeShiftOrderBy
        {
            description,
            location,
            status

        }
        public enum ActivityLogOrderBy
        {

            Description,
            CreatedBy,
            appliedDate
        }
        public enum ToDoOrderBy
        {
            Shifttype,
            Description,

        }

        public enum NotificationOrderBy
        {
            Date,
            EmployeeName,
            Description
        }
    }
    public enum SortOrder
    {
        Asc,
        Desc,
    }
    public enum LocationOrderBy
    {
        Name,
        Address,
        Status,
        Action


    }
}
