using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetAllClientList
{
  public class GetAllClientListQuery : IRequest<ApiResponse>
  {
    public string SearchTextByName { get; set; }

    public string SearchTextBylocation { get; set; }
    public int PageSize { get; set; }

    public int PageNo { get; set; }
    public LHSAPI.Common.Enums.Client.ClientOrderBy OrderBy { get; set; }
    public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}
