
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientProgressNotesList
{
    public class GetClientProgressNotesListQuery : IRequest<ApiResponse>
    {
        public int ClientId { get; set; }
        public int ShiftId { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Client.ClientProgressInfoOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }


    }
}
