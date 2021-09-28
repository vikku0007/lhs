using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentRiskAssesment
{
  public class AddIncidentRiskAssesmentCommand : IRequest<ApiResponse>
  {

        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int IsRiskAssesment { get; set; }

        public DateTime? RiskAssesmentDate { get; set; }

        public string RiskDetails { get; set; }

        public string NoRiskAssesmentInfo { get; set; }

        public string InProgressRisk { get; set; }

        public string TobeFinished { get; set; }
        public int? ShiftId { get; set; }
    }
}


