using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Application.Client.Models
{
    public class IncidentAllegationPrimaryDisability
    {

        public int Id { get; set; }
        public int ClientId { get; set; }

        public int DisableAllegationId { get; set; }

        //public int PrimaryDisability { get; set; }
        //public string PrimaryDisabilityName { get; set; }
        public int CodeId { get; set; }
        public string CodeName { get; set; }
    }
}
