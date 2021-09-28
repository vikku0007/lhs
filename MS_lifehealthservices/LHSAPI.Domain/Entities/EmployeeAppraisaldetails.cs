using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeAppraisalDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string DepartmentName { get; set; }

        public int AppraisalType { get; set; }

        public DateTime? AppraisalDateFrom { get; set; }

        public DateTime? AppraisalDateTo { get; set; }

    }
}
