using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public  class ShiftToDoList : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int ClientId { get; set; }

        public string Description { get; set; }

        public bool IsInitials { get; set; }

        public string Initials { get; set; }

        public int? TodoItemId { get; set; }
        public int ShiftId { get; set; }
    }
}
