using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Application.Client.Models
{
   public class IncidentDocumentDetailModel
    {
       
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string FileName { get; set; }
        public string DocumentName { get; set; }
    }
}
