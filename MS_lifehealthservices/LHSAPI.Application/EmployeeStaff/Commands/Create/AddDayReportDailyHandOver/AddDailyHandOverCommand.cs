using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportDailyHandOver
{
  public class AddDailyHandOverCommand : IRequest<ApiResponse>
  {
        public List<DailyHandOverItem> DailyHandOverItem { get; set; }
        public int ShiftId { get; set; }
        public string HouseAddress { get; set; }
        public DateTime? Date { get; set; }

    }
    public class DailyHandOverItem
    {
    public string Description { get; set; }

    public bool? IsMorningWorker { get; set; }

    public string MorningWorkerSignature { get; set; }

    public bool? IsAfterNoonWorker { get; set; }

    public string AfterNoonWorkerSign { get; set; }

    public bool? IsSleepOverWorker { get; set; }

    public string SleepOverWorkerSign { get; set; }
}
}
