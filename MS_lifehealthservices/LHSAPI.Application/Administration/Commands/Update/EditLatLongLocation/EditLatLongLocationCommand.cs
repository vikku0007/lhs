using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Update.EditLatLongLocation
{
    public class EditLatLongLocationCommand : IRequest<ApiResponse>
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double JobCode { get; set; }
    }
}
