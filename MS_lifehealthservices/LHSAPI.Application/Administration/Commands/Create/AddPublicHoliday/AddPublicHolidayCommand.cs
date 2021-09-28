using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Create.AddPublicHoliday
{
  public class AddPublicHolidayCommand : IRequest<ApiResponse>
  {
        public int ID { get; set; }
        public string Holiday { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
        public int? Year { get; set; }

    }
}
