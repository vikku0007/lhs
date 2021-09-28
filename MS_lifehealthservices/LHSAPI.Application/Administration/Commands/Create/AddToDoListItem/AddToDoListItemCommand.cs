using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Create.AddToDoListItem
{
  public class AddToDoListItemCommand : IRequest<ApiResponse>
  {
        public int ID { get; set; }
        public int ShiftType { get; set; }
        public string Description { get; set; }

    }
}
