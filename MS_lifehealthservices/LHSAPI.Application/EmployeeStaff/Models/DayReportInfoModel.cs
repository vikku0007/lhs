using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
    public class DayReportInfoModel
    {
       
        public List<LHSAPI.Application.EmployeeStaff.Models.DayReportAppointments> DayReportAppointments { get; set; }
        public List<LHSAPI.Application.EmployeeStaff.Models.DayReportCashHandOver> DayReportCashHandOver { get; set; }
        public List<LHSAPI.Application.EmployeeStaff.Models.DayReportDailyHandOver> DayReportDailyHandOver { get; set; }
        public LHSAPI.Application.EmployeeStaff.Models.DayReportDetail DayReportDetail { get; set; }
        public List<LHSAPI.Application.EmployeeStaff.Models.DayReportFoodIntake> DayReportFoodIntake { get; set; }
        public List<LHSAPI.Application.EmployeeStaff.Models.DayReportSupportWorkers> DayReportSupportWorkers { get; set; }
        public List<LHSAPI.Application.EmployeeStaff.Models.DayReportTelePhoneMsg> DayReportTelePhoneMsg { get; set; }
        public List<LHSAPI.Application.EmployeeStaff.Models.DayReportVisitor> DayReportVisitor { get; set; }
    }
}
