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

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeAppraisal
{

    public class DeleteEmployeeAppraisalCommandHandler : IRequestHandler<DeleteEmployeeAppraisalCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteEmployeeAppraisalCommandHandler(LHSDbContext context,ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteEmployeeAppraisalCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var AppraisalResult = _context.EmployeeAppraisalDetails.FirstOrDefault(x => x.Id == request.Id &&  x.IsDeleted == false && x.IsActive == true);
                   
                    if (AppraisalResult == null)
                    {

                        response.NotFound();
                    }
                    else
                    {
                       
                        AppraisalResult.IsDeleted = true;
                        AppraisalResult.IsActive = false;
                        AppraisalResult.DeletedDate = DateTime.UtcNow;
                        AppraisalResult.DeletedById = await _ISessionService.GetUserId();
                        _context.EmployeeAppraisalDetails.Update(AppraisalResult);
                        await _context.SaveChangesAsync();
                        var Existstandard = _context.EmployeeAppraisalStandards.Where(x => x.AppraisalId == request.Id && x.IsDeleted == false && x.IsActive == true).ToList();
                        if (Existstandard == null)
                        {
                        }
                        else
                        {
                            foreach (var item in Existstandard)
                            {
                                var StandardResult = _context.EmployeeAppraisalStandards.FirstOrDefault(x => x.Id == item.Id && x.IsDeleted == false && x.IsActive == true);
                                StandardResult.IsDeleted = true;
                                StandardResult.IsActive = false;
                                StandardResult.DeletedDate = DateTime.UtcNow;
                                StandardResult.DeletedById = await _ISessionService.GetUserId();
                                _context.EmployeeAppraisalStandards.Update(StandardResult);
                                await _context.SaveChangesAsync();
                            }
                           
                        }

                        response.Delete(AppraisalResult);

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
