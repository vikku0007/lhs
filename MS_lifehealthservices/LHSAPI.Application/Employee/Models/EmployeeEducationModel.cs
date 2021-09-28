using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
   public class EmployeeEducationModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string Institute { get; set; }

        public string Degree { get; set; }

        public string FieldStudy { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string AdditionalNotes { get; set; }
        public string DocumentPath { get; set; }
        public int? QualificationType { get; set; }
        public string QualificationTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
