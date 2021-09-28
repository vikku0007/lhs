﻿using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeOtherDocument
{
    public class DeleteEmployeeOtherDocumentCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}
