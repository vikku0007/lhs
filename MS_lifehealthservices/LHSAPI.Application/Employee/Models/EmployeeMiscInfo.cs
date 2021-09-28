using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
   public class EmployeeMiscInfo
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public int? Ethnicity { get; set; }

        public int? Religion { get; set; }

        public bool Smoker { get; set; }

        
        public string EthnicityName { get; set; }

        public string ReligionName { get; set; }
    }
}
