using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
    public class AssignedShiftInfoViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public List<ClientInfo> ClientInfo { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public bool IsShiftCompleted { get; set; }
        public bool IsLogin { get; set; }
        public string StatusName { get; set; }
        public bool IsLoginButtonVisible { get; set; }
        public bool IsLogoutButtonVisible { get; set; }

    }

    public class ClientInfo
    {
        public string ClientImgURL { get; set; }
        public int ClientId { get; set; }
        public int ShiftId { get; set; }
        public string ClientName { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public int Id { get; set; }

    }
}
