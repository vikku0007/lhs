using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.UpdateMasterActiveInActive
{
  public class UpdateMasterActiveInActiveCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public bool Status { get; set; }
        


    }
}
