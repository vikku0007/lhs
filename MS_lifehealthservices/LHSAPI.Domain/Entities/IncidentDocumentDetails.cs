﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
   public class IncidentDocumentDetails:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string FileName { get; set; }
        public string DocumentName { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }
}
