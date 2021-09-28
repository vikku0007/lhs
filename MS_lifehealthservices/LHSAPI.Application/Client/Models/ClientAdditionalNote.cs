using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
  public class ClientAdditionalNote 
  {
   
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string PrivateNote { get; set; }

    public string PublicNote { get; set; }

   

  }

}
