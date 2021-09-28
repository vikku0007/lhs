using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
  public class EmployeeJobProfile
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    public string JobDesc { get; set; }

    public string DepartmentName { get; set; }
    public string LocationName { get; set; }
    public string ReportingToName { get; set; }
    public string SourceName { get; set; }

    public int? DepartmentId { get; set; }

    public int? LocationId { get; set; }

    public int? ReportingToId { get; set; }

    public DateTime? DateOfJoining { get; set; }

    public int? Source { get; set; }



    public int? WorkingHoursWeekly { get; set; }

    public int? DistanceTravel { get; set; }
        public int? LocationType { get; set; }
        public string LocationTypeName { get; set; }
        public string OtherLocation { get; set; }

    }
}
