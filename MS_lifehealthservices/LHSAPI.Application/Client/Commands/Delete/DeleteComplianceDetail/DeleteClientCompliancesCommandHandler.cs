using LHSAPI.Application.Interface;
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

namespace LHSAPI.Application.Client.Commands.Delete.DeleteClientCompliances
{

    public class DeleteClientCompliancesCommandHandler : IRequestHandler<DeleteClientCompliancesCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteClientCompliancesCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteClientCompliancesCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var AvalResult = _context.ClientCompliancesDetails.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                    if (AvalResult == null)
                    {
                        response.NotFound();
                    }
                    else
                    {

                        AvalResult.IsDeleted = true;
                        AvalResult.IsActive = false;
                        AvalResult.DeletedDate = DateTime.UtcNow;
                        AvalResult.DeletedById = await _ISessionService.GetUserId();
                        _context.ClientCompliancesDetails.Update(AvalResult);
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
