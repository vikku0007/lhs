using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class StandardCode:BaseEntity
    {
    public int ID { get; set; }
    public int? Value { get; set; }
    public string CodeData { get; set; }
         
    public string CodeDescription { get; set; }
    public int? CodeValue { get; set; }

    }
}
