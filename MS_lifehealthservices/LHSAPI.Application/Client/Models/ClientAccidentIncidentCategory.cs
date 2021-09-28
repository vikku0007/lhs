using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
   public class ClientAccidentIncidentCategory
    {

        public ClientIncidentCategory ClientIncidentCategory { get; set; }
        public List<ClientPrimaryIncidentCategory> ClientPrimaryIncidentCategory { get; set; }
        public List<ClientSecondaryIncidentCategory> ClientSecondaryIncidentCategory { get; set; }
        
       
    }

}
