using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
   public class ClientOtherInformtion 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string OtherInformation { get; set; }
    }
}
