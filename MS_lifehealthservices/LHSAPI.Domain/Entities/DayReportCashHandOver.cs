using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
   public class DayReportCashHandOver : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        public string CashHandover { get; set; }

        public double? BalanceaBroughtAM { get; set; }

        public double? ExpenseAM { get; set; }

        public double? BalanceaBroughtPM { get; set; }

        public double? ExpensePM { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }
    }
}
