using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetEmployeeCurrentLocation
{
    public class GetEmployeeCurrentLocationQuery : IRequest<ApiResponse>
    {
        public int ShiftId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
