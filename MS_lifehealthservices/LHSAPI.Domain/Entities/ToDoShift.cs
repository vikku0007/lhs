using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class ToDoShift : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ShiftId { get; set; }
        public DateTime? DateTime { get; set; }
        public int ShiftType { get; set; }        
        public TimeSpan ShiftTime { get; set; }

    }
}
