using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Notification.Models
{
    public class NotificationViewModel 
    {
    public int Id { get; set; }

    public string EventName { get; set; }

    public string Description { get; set; }

   
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime? CreatedDate { get; set; }


  }
}
