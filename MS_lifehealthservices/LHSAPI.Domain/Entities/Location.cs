using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace LHSAPI.Domain.Entities
{
   public  class Location : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        
        public string  Name { get; set; }
        
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CalenderView { get; set; }
        public string WeekDay { get; set; }
        public int? ExternalCode { get; set; }
        public string InvoicePrefix { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerContact { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool? AdditionalSetting { get; set; }
        public double? JobCode { get; set; }

    }
}
