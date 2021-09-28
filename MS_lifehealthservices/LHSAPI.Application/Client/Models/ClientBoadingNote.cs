using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
  public class ClientBoadingNote 
  {
    
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string CareNote { get; set; }

    public string CareNoteByClient { get; set; }

   
  }

}
