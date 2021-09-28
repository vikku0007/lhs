using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
   public class ClientPersonalPreferences : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string Interest { get; set; }

        public string ClientImportance { get; set; }

        public string Charecteristics { get; set; }

        public string FearsandAnxities { get; set; }
    }
}
