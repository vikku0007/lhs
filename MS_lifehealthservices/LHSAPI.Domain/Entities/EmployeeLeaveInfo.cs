using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeLeaveInfo : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
       // public bool Approve { get; set; }
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
        public int LeaveType { get; set; }
        public string ReasonOfLeave { get; set; }
        public int? RejectedById { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? RejectedDate { get; set; }

        public int? ApprovedById { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsRejected { get; set; }
        public string RejectRemark { get; set; }
    }
}
