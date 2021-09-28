using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class EmployeeEducation : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Institute { get; set; }

    public string Degree { get; set; }

    public string FieldStudy { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string AdditionalNotes { get; set; }
    public string DocumentPath { get; set; }
    public int? QualificationType { get; set; }
    }

}
