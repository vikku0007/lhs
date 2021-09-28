using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
    public class ShiftToDoList
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int ClientId { get; set; }

        public string Description { get; set; }

        public bool IsInitials { get; set; }

        public string Initials { get; set; }

        public int? TodoItemId { get; set; }
    }
}
