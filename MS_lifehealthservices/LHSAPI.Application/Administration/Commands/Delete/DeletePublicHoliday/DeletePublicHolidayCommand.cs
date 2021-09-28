using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Delete.PublicHoliday
{
    public class DeletePublicHolidayCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}
