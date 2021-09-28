using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportCashHandOver
{
  public class AddCashHandOverCommand : IRequest<ApiResponse>
  {
        public string CashHandover { get; set; }

        public double? BalanceaBroughtAM { get; set; }

        public double? ExpenseAM { get; set; }

        public double? BalanceaBroughtPM { get; set; }

        public double? ExpensePM { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }


    }
}
