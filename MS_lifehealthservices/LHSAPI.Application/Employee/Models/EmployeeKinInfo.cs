using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeeKinInfo
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string MiddelName { get; set; }

        public string LastName { get; set; }

        public int RelationShip { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }
        public string RelationShipName { get; set; }
        public string OtherRelation { get; set; }
    }
}
