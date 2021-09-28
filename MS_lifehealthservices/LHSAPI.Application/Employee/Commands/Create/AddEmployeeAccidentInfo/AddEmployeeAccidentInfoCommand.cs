using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAccidentInfo
{
  public class AddEmployeeAccidentInfoCommand : IRequest<ApiResponse>
  {

        public int EmployeeId { get; set; }

        public DateTime AccidentDate { get; set; }

        public int? EventType { get; set; }

        public int? LocationId { get; set; }

        public int RaisedBy { get; set; }

        public int ReportedTo { get; set; }

        public string BriefDescription { get; set; }

        public string DetailedDescription { get; set; }

        public string OtherLocation { get; set; }

        public int? LocationType { get; set; }
        public string ActionTaken { get; set; }
        public string IncidentTime { get; set; }
    }
}
