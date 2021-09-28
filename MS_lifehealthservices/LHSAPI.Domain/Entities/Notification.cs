using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public  class Notification : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string EventName { get; set; }

    public string Description { get; set; }

    public bool Email { get; set; }

    public bool IsAdminAlert { get; set; }

    public bool IsEmailSent { get; set; }

    public int EmployeeId { get; set; }

    public bool IsReaded { get; set; }
  }
}
