using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class EmployeeMiscInfo :BaseEntity
  {
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public double? Weight { get; set; }

    public double? Height { get; set; }

    public int? Ethnicity { get; set; }

    public int? Religion { get; set; }

    public bool Smoker { get; set; }

  }

}
