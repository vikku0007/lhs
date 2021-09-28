using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeAppraisalStandards : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AppraisalId { get; set; }
        public int EmployeeId { get; set; }

        public bool IsExceeds { get; set; }

        public bool IsAchieves { get; set; }

        public bool IsBelow { get; set; }

        public string DescriptionName { get; set; }

    }

}
