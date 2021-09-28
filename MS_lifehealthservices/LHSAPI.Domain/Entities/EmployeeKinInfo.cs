using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
  public class EmployeeKinInfo :BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string FirstName { get; set; }

    public string MiddelName { get; set; }

    public string LastName { get; set; }

    public int RelationShip { get; set; }

    public string ContactNo { get; set; }

    public string Email { get; set; }
    public string OtherRelation { get; set; }
    

  }

}
