using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Application.Client.Models
{
    public class IncidentAllegationOtherDisability 
    {
       
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int DisableAllegationId { get; set; }
        public int CodeId { get; set; }

        //public int OtherDisability { get; set; }
        public string OtherNeurological { get; set; }
        public string OtherPhysical { get; set; }
        public string Other { get; set; }
        //public string SecondaryDisabilityName { get; set; }
        public string CodeName { get; set; }

    }
}