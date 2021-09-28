using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
    public class ShiftHistoryDetails
    {
        public LHSAPI.Application.EmployeeStaff.Models.ShiftHistory ShiftHistory { get; set; }
        public List<LHSAPI.Application.EmployeeStaff.Models.ProgressNotesList> ProgressNotesList { get; set; }
        public LHSAPI.Application.EmployeeStaff.Models.ToDoShift ToDoShift { get; set; }
        public List<ShiftToDoList> ShiftToDoList { get; set; }
    }
}
