using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class ServiceTypeShiftInfoTemplate : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ServiceTypeId { get; set; }

    public int ShiftId { get; set; }
    public int ShiftTemplateId { get; set; }
  }
}
