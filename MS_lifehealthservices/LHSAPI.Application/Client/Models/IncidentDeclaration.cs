using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Application.Client.Models
{
   public class IncidentDeclaration 
    {
       
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string Name { get; set; }

        public string PositionAtOrganisation { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsDeclaration { get; set; }
    }
}
