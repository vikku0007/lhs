using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Update.EditMasterEntries
{
  public class EditMasterEntriesCommand : IRequest<ApiResponse>
  {
        public int ID { get; set; }
        public int Value { get; set; }
        public string CodeData { get; set; }

        public string CodeDescription { get; set; }
        public int? CodeDataId { get; set; }


    }
}
