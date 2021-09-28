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

namespace LHSAPI.Application.Client.Commands.Delete.DeleteOtherContactPerson
{

    public class DeleteOtherContactPersonHandler : IRequestHandler<DeleteOtherContactPersonCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteOtherContactPersonHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteOtherContactPersonCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistEmp = _context.ClientPrimaryCareInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                    if (ExistEmp == null)
                    {
                        response.NotFound();
                    }
                    else
                    {

                        ExistEmp.IsDeleted = true;
                        ExistEmp.IsActive = false;
                        ExistEmp.DeletedDate = DateTime.UtcNow;
                        ExistEmp.DeletedById = await _ISessionService.GetUserId();
                        _context.ClientPrimaryCareInfo.Update(ExistEmp);
                        await _context.SaveChangesAsync();
                        response.Delete(ExistEmp);

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
