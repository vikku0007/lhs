using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Update.UpdateClientFundingInfo
{
  public class UpdateClientFundingCommand : IRequest<ApiResponse>
  {

        public int Id { get; set; }
        public int ClientId { get; set; }

        public double Ammount { get; set; }

        public int? NoDays { get; set; }

        public int? ServiceType { get; set; }

        public double TotalAmount { get; set; }
        public int? ClaimNumber { get; set; }
        public int? PaymentTerm { get; set; }
        public int? Payer { get; set; }
        public int? ReferenceNumber { get; set; }
    }
}
