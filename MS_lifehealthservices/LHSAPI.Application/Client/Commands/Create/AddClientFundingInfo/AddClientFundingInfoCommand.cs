using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientFundingInfo
{
  public class AddClientFundingInfoCommand : IRequest<ApiResponse>
  {
        public int ClientId { get; set; }
        public int Id { get; set; }

        public int? FundType { get; set; }

        public string RefNumber { get; set; }

        public string Other { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public double Amount { get; set; }
        public int? ClaimNumber { get; set; }
    }
}
