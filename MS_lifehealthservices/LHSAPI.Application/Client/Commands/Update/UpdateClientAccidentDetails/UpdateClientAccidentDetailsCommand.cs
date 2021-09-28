using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Update.UpdateClientAccidentDetails
{
  public class UpdateClientAccidentDetailsCommand : IRequest<ApiResponse>
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
        public string OtherLocation { get; set; }

        public int? LocationType { get; set; }

    }
}
