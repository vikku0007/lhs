using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
  public class GlobalPayRate : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Level { get; set; }

    public double MonToFri6To12AM { get; set; }

    public double Sat6To12AM { get; set; }

    public double Sun6To12AM { get; set; }

    public double? Holiday6To12AM { get; set; }

    public double MonToFri12To6AM { get; set; }

    public double Sat12To6AM { get; set; }

    public double Sun12To6AM { get; set; }

    public double? Holiday12To6AM { get; set; }
    public double ActiveNightsAndSleep { get; set; }
    public double HouseCleaning { get; set; }
    public double TransportPetrol { get; set; }
    }
}
