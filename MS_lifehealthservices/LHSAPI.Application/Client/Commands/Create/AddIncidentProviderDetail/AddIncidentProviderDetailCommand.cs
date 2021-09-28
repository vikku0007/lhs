using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentProviderDetail
{
  public class AddIncidentProviderDetailCommand : IRequest<ApiResponse>
  {

        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string ReportCompletedBy { get; set; }

        public string ProviderName { get; set; }

        public string ProviderregistrationId { get; set; }

        public string ProviderABN { get; set; }

        public string OutletName { get; set; }

        public string Registrationgroup { get; set; }

        public int State { get; set; }
        public int? ShiftId { get; set; }
        
    }
}
