using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class BaseEntity
    {
        [DefaultValue("true")]
        public bool IsActive { get; set; }
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
       
        public int? DeletedById { get; set; }
        public User DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
     
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; }
        
        public DateTime? CreatedDate { get; set; }
        //[ForeignKey("Users")]
        public int? UpdateById { get; set; }
        public User UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

       
    }
}
