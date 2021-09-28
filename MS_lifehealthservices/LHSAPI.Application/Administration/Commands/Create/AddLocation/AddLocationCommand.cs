using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Create.AddLocation
{
  public class AddLocationCommand : IRequest<ApiResponse>
  {
        public string Name { get; set; }
        public string Address { get; set; }
        public string CalenderView { get; set; }
        public string WeekDay { get; set; }
        public int ExternalCode { get; set; }
        public string InvoicePrefix { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerContact { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool AdditionalSetting { get; set; }

    }
}
