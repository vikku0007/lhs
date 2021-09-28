using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
   public class EmployeeOtherdetails
   {
        public EmployeePrimaryInfo EmployeePrimaryInfo { get; set; }
        public EmployeeCommunicationInfo EmployeeCommunicationInfo { get; set; }
        public EmployeeAccidentInfo EmployeeAccidentInfo { get; set; }
        public EmployeeAppraisalDetails EmployeeAppraisalDetails { get; set; }
        public EmployeeAvailabilityDetails EmployeeAvailabilityDetails { get; set; }
        public EmployeeStaffWarning EmployeeStaffWarning { get; set; }
        public EmployeeLeaveInfo EmployeeLeaveInfo { get; set; }
        
    }
}
