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

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeCommunication
{

    public class DeleteEmployeeCommunicationCommandHandler : IRequestHandler<DeleteEmployeeCommunicationCommand, ApiResponse>
    { 

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteEmployeeCommunicationCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteEmployeeCommunicationCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var AvalResult = _context.EmployeeCommunicationInfo.FirstOrDefault(x => x.Id == request.Id &&  x.IsDeleted == false && x.IsActive == true);
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
                        _context.EmployeeCommunicationInfo.Update(AvalResult);
                        await _context.SaveChangesAsync();

                        var Existstandard = _context.CommunicationRecipient.Where(x => x.CommunicationId == request.Id && x.IsDeleted == false && x.IsActive == true).ToList();
                        if (Existstandard == null)
                        {
                        }
                        else
                        {
                            foreach (var item in Existstandard)
                            {
                                var StandardResult = _context.CommunicationRecipient.FirstOrDefault(x => x.Id == item.Id && x.IsDeleted == false && x.IsActive == true);
                                StandardResult.IsDeleted = true;
                                StandardResult.IsActive = false;
                                StandardResult.DeletedDate = DateTime.UtcNow;
                                StandardResult.DeletedById = await _ISessionService.GetUserId();
                                _context.CommunicationRecipient.Update(StandardResult);
                                await _context.SaveChangesAsync();
                            }

                        }


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
