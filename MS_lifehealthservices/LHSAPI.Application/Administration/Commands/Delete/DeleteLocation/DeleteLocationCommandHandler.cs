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

namespace LHSAPI.Application.Administration.Commands.Delete.DeleteLocation
{

    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;

        public DeleteLocationCommandHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<ApiResponse> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.LocationId > 0)
                {

                    var ExistEmp = _context.Location.FirstOrDefault(x => x.LocationId == request.LocationId &&  x.IsDeleted == false && x.IsActive == true);
                    if (ExistEmp == null)
                    {
                        response.StatusCode = ResponseCode.NotFound;
                        response.Message = ResponseMessage.RecordNotExist;
                    }
                    else
                    {
                        var eventResult = _context.Location.FirstOrDefault(x => x.LocationId == request.LocationId && x.IsDeleted == false && x.IsActive == true);
                        eventResult.IsDeleted = true;
                        eventResult.IsActive = false;
                        eventResult.DeletedDate = DateTime.UtcNow;
                        eventResult.DeletedById = 1;
                        _context.Location.Update(eventResult);
                        await _context.SaveChangesAsync();
                        response.Delete(eventResult);

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
