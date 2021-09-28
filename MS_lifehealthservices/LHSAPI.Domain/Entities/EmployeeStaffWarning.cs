using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeStaffWarning : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int WarningType { get; set; }

        public int OffensesType { get; set; }

        public string Description { get; set; }

        public string ImprovementPlan { get; set; }

        public string OtherOffenses { get; set; }

    }
}
