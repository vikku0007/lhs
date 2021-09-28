using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class ClientProgressNotes : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string PatientName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string MedicalRecordNo { get; set; }

   
  }
}
