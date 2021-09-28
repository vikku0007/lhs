using LHSAPI.Application.Client.Models;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.Client.Queries.GetProgressNotesDetails
{
    public class GetProgressNotesDetailsQueryHandler : IRequestHandler<GetProgressNotesDetailsQuery, ApiResponse>
    {
        private readonly LHSDbContext _context;
        //   readonly ILoggerManager _logger;
        public GetProgressNotesDetailsQueryHandler(LHSDbContext dbContext)
        {
            _context = dbContext;
            // _logger = logger;
        }

        public async Task<ApiResponse> Handle(GetProgressNotesDetailsQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var CompList = _context.ProgressNotesList.Where(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive
                ).ToList();
                if (CompList != null && CompList.Any())
                {
                    var totalCount = CompList.Count();
                    response.ResponseData = CompList.ToList();
                    response.Message = ResponseMessage.Success;
                    response.StatusCode = HTTPStatusCode.SUCCESSSTATUSCODE;
                    response.Total = totalCount;
                }
                else
                {
                    response.Message = ResponseMessage.NOTFOUND;
                    response.StatusCode = HTTPStatusCode.NO_DATA_FOUND;
                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;



        }
    }
}
