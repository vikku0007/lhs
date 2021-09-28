using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class EmployeeAwardInfo :BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AwardGroup { get; set; }

    public int EmployeeId { get; set; }

    public double? Allowances { get; set; }

    public double? Dailyhours { get; set; }

    public double? Weeklyhours { get; set; }

  }

}
