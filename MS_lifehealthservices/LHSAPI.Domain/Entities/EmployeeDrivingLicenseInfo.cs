using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class EmployeeDrivingLicenseInfo : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public bool DrivingLicense { get; set; }

    public bool CarInsurance { get; set; }

    public DateTime? CarRegExpiryDate { get; set; }

    public string CarRegNo { get; set; }

    public int? LicenseType { get; set; }

    public int? LicenseState { get; set; }

    public string LicenseNo { get; set; }

    public DateTime? LicenseExpiryDate { get; set; }
    public DateTime? InsuranceExpiryDate { get; set; }
    public int? LicenseOrigin { get; set; }
    }
}
