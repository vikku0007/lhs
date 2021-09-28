using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeeDriverLicenseModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public bool DrivingLicense { get; set; }

        public bool CarInsurance { get; set; }

        public DateTime? CarRegExpiryDate { get; set; }

        public string CarRegNo { get; set; }

        public int? LicenseType { get; set; }
        public int? LicenseOrigin { get; set; }

        public int? LicenseState { get; set; }

        public string LicenseNo { get; set; }

        public DateTime? LicenseExpiryDate { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public string LicenseTypeName { get; set; }
        public string LicenseStateName { get; set; }
        public string LicenseOriginName { get; set; }
    }
}
