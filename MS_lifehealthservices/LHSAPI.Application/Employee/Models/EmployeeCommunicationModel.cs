using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeeCommunicationModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public int AssignedTo { get; set; }

        public string EmployeeName {get;set;}

        public DateTime? CreatedDate { get; set; }

        public string AssignedToName { get; set; }
        public string FullName { get; set; }
        public List<CommunicationRecepientmodel> CommunicationRecepientmodel { get; set; }

    }
}
