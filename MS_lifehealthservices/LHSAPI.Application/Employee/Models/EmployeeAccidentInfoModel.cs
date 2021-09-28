using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
   public  class EmployeeAccidentInfoModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime? AccidentDate { get; set; }

        public int? EventType { get; set; }

        public int? LocationId { get; set; }

        public int RaisedBy { get; set; }

        public int ReportedTo { get; set; }

        public string BriefDescription { get; set; }

        public string DetailedDescription { get; set; }
        public string EventTypeName { get; set; }

        public string LocationName { get; set; }
        public string ReportedToName { get; set; }
        public string RaisedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LocationType { get; set; }
        public string LocationTypeName { get; set; }
        public string OtherLocation { get; set; }
        public string ActionTaken { get; set; }
        public TimeSpan? IncidentTime { get; set; }
        public string IncidentTimeName { get; set; }
        public string IncidentTimeTake { get; set; }
    }
}
