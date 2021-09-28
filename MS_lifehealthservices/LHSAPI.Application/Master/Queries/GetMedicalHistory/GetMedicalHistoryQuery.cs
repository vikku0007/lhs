using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetMedicalHistory
{
    public class GetMedicalHistoryQuery : IRequest<ApiResponse>
    {
        public int ID { get; set; }

        public string CodeData { get; set; }

        public string CodeDescription { get; set; }
    }
}
