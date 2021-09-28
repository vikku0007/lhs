using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeeComplianceModel
    {
        public int EmployeeId { get; set; }
        public int Id { get; set; }
        public int DocumentName { get; set; }
        public int CreatedById { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public int? DocumentType { get; set; }
        public string DocumentTypeName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
        public string Description { get; set; }
        public Nullable<bool> HasExpiry { get; set; }
        public Nullable<bool> Alert { get; set; }
        public int UpdateById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string FileName { get; set; }
        public string Document { get; set; }

    }
}
