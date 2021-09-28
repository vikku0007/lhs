using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDailyFoodIntake
{
  public class UpdateDailyFoodIntakeCommand : IRequest<ApiResponse>
  {

        public int Id { get; set; }

        public string ResidentName { get; set; }

        public DateTime? Date { get; set; }

        public string DailyFoodIntake { get; set; }
        public string Signature { get; set; }

        public int ShiftId { get; set; }
        public string StaffName { get; set; }

        public string Breakfast { get; set; }

        public string Lunch { get; set; }

        public string Dinner { get; set; }

        public string Snacks { get; set; }

        public string fluids { get; set; }


    }
}
