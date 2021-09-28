using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
   public class ClientPersonalPreferences
    {
       
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string Interest { get; set; }

        public string ClientImportance { get; set; }

        public string Charecteristics { get; set; }

        public string FearsandAnxities { get; set; }
    }
}
