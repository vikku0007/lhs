using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
   public class CommunicationRecepientmodel
    {
        public int Id { get; set; }

        public int CommunicationId { get; set; }

        public int EmployeeId { get; set; }
        public string AssignedToName { get; set; }

    }
}
