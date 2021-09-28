using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class EmployeeWorkExp : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Company { get; set; }

    public string JobTitle { get; set; }

    public string JobDesc { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
   public string Duration { get; set; }
   public string DocumentPath { get; set; }
    }

}
