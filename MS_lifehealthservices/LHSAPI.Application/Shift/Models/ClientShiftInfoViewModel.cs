using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Models
{
    public class ClientShiftInfoViewModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }

        public int IsProgressNotesSubmitted { get; set; }
        public int IsToDoListSubmitted { get; set; }
        public bool IsCheckoutCompleted { get; set; }
    }
}
