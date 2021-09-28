using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Client.Commands.Delete.DeleteIncidentDocument
{

    public class DeleteIncidentDocumentHandler : IRequestHandler<DeleteIncidentDocumentCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;

        public DeleteIncidentDocumentHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<ApiResponse> Handle(DeleteIncidentDocumentCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var AvalResult = _context.IncidentDocumentDetails.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                    if (AvalResult == null)
                    {
                        response.NotFound();
                    }
                    else
                    {
                        AvalResult.FileName = null;
                        _context.IncidentDocumentDetails.Update(AvalResult);
                        await _context.SaveChangesAsync();
                        response.Delete(AvalResult);

                    }
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
