using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddToDoList
{
    public class AddToDoListCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int ShiftId { get; set; }
        //public int ClientId { get; set; }

        public DateTime? DateTime { get; set; }
        public int ShiftType { get; set; }
        public string ShiftTime { get; set; }

        public List<ShiftToDoListItem> ShiftToDoListItem { get; set; }
    }
    public class ShiftToDoListItem
    {
        public int EmployeeId { get; set; }
        //public int ClientId { get; set; }
        public int ShitToDoId { get; set; }
        public string Description { get; set; }

        public bool IsInitials { get; set; }

        public string Initials { get; set; }

        public int ShiftId { get; set; }

    }    

    public class UpdateToDoListCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public int ShiftId { get; set; }        

        public DateTime? DateTime { get; set; }
        public int ShiftType { get; set; }
        public string ShiftTime { get; set; }

        public int ToDoItemId { get; set; }

        public List<ShiftToDoListItem> ShiftToDoListItem { get; set; }
    }
}
