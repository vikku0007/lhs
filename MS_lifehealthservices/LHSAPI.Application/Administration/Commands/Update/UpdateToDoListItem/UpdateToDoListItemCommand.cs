using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Update.UpdateToDoListItem
{
  public class UpdateToDoListItemCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int ShiftType { get; set; }
        public string Description { get; set; }

    }
}
