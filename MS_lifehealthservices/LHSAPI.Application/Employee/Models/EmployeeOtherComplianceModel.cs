using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeeOtherComplianceModel
    {
        public int EmployeeId { get; set; }
        public int Id { get; set; }
        public int OtherDocumentName { get; set; }
        public int? OtherDocumentType { get; set; }
        public string OtherDocumentTypeName { get; set; }
       
        public Nullable<DateTime> OtherExpiryDate { get; set; }
        public Nullable<DateTime> OtherIssueDate { get; set; }
        public string OtherDescription { get; set; }
        public Nullable<bool> OtherHasExpiry { get; set; }

        public Nullable<bool> OtherAlert { get; set; }
        
        public string OtherFileName { get; set; }
        public string OtherDocument { get; set; }
    }
}
