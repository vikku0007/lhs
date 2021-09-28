using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Delete.DeleteShiftInfo
{
    public class DeleteShiftInfoCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}
