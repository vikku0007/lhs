using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Models
{
    public class ClientComplianceModel
    {
        public int ClientId { get; set; }
        public int Id { get; set; }
        public int DocumentName { get; set; }
        
        public int? DocumentType { get; set; }
        public string DocumentTypeName { get; set; }
       
        public Nullable<DateTime> ExpiryDate { get; set; }
        public Nullable<DateTime> IssueDate { get; set; }
        public string Description { get; set; }
        public Nullable<bool> HasExpiry { get; set; }

        public Nullable<bool> Alert { get; set; }
        
        public string FileName { get; set; }
        public string Document { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
