﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Domain.Entities
{
    public class IncidentAllegationOtherDisability : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int DisableAllegationId { get; set; }

        public int OtherDisability { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }
}