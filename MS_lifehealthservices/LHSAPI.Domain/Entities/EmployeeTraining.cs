using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeTraining:BaseEntity
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int MandatoryTraining { get; set; }

        public int TrainingType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? CourseType { get; set; }

        public string Remarks { get; set; }
        public bool? IsAlert { get; set; }
        public string FileName { get; set; }
        public string OtherTraining { get; set; }

    }

}
