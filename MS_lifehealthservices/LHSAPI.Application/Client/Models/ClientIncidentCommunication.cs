using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Application.Client.Models
{
  public  class ClientIncidentCommunication 
    {
       
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int ImpactPersonId { get; set; }

        public int CommunicationId { get; set; }
        public string CommunicationName { get; set; }
       


    }
}
