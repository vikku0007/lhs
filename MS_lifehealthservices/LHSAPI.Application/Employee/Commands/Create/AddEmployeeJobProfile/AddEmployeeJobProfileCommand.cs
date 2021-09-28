using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeJobProfile
{
    public class AddEmployeeJobProfileCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string JobDesc { get; set; }

        public int DepartmentId { get; set; }

        public int? LocationId { get; set; }

        public int ReportingToId { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public int Source { get; set; }

         public int? WorkingHoursWeekly { get; set; }
        public int? DistanceTravel { get; set; }
        public string OtherLocation { get; set; }

        public int? LocationType { get; set; }

    }
}
