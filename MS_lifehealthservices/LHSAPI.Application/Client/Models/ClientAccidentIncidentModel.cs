using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Models
{
   public  class ClientAccidentIncidentModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }

        public DateTime? AccidentDate { get; set; }

        public int? IncidentType { get; set; }

        public int? LocationId { get; set; }

        public int ReportedBy { get; set; }

        public string PhoneNo { get; set; }
        public bool PoliceNotified { get; set; }

        public string IncidentCause { get; set; }

        public string FollowUp { get; set; }
        public string LocationName { get; set; }
        public string EventTypeName { get; set; }
        public string ReportedByName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
        public string FullName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LocationType { get; set; }
        public string LocationTypeName { get; set; }
        public string OtherLocation { get; set; }
    }
}
