using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
    public class ToDoShift
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int ShiftId { get; set; }
        public DateTime? DateTime { get; set; }
        public int ShiftType { get; set; }
        public string ShiftTypeString { get; set; }
        public TimeSpan ShiftTime { get; set; }
        public string ShiftTimeString { get; set; }
        public string ShiftTypeName { get; set; }
    }

    public class EditToDoShift
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public string ShiftType { get; set; }
        public int ShiftTypeId { get; set; }
        public List<ToDoItem> ToDoItemList { get; set; }

    }

    public class ToDoItem
    {
        public int Id { get; set; }
        public bool IsInitials { get; set; }
        public string Initials { get; set; }
        public string Description { get; set; }

    }
}
