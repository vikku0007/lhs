﻿using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeEducationInfo
{
    public class DeleteEmployeeEducationInfoCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}
