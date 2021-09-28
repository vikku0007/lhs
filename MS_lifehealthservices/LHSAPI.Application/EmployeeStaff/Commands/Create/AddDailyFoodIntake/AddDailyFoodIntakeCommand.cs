using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDailyFoodIntake
{
  public class AddDailyFoodIntakeCommand : IRequest<ApiResponse>
  {

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
