using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeAppraisalInfo
{

    public class EditEmployeeAppraisalInfoCommandHandler : IRequestHandler<EditEmployeeAppraisalInfoCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public EditEmployeeAppraisalInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(EditEmployeeAppraisalInfoCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var AppraisalResult = _context.EmployeeAppraisalDetails.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);

                    if (AppraisalResult == null)
                    {

                        response.NotFound();
                    }
                    else
                    {


                        // AppraisalResult.DepartmentName = request.DepartmentName;
                        AppraisalResult.UpdateById = await _ISessionService.GetUserId();
                        AppraisalResult.UpdatedDate = DateTime.Now;
                        AppraisalResult.AppraisalType = request.AppraisalType;
                        AppraisalResult.AppraisalDateFrom = request.AppraisalDateFrom;
                        AppraisalResult.AppraisalDateTo = request.AppraisalDateTo;
                        _context.EmployeeAppraisalDetails.Update(AppraisalResult);
                        await _context.SaveChangesAsync();


                        if (request.StandardList != null && request.StandardList.Count > 0)
                        {
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
                            List<EmployeeAppraisalStandards> AppraisalStandard = new List<EmployeeAppraisalStandards>();

                            foreach (var item in request.StandardList)
                            {
                                if (item.IsExceeds == false && item.IsAchieves == false && item.IsBelow == false)
                                {

                                }
                                else
                                {
                                    EmployeeAppraisalStandards StandardItem = new EmployeeAppraisalStandards

                                    {
                                        AppraisalId = request.Id,
                                        EmployeeId = item.EmployeeId,
                                        DescriptionName = item.DescriptionName,
                                        IsExceeds = item.IsExceeds,
                                        IsAchieves = item.IsAchieves,
                                        IsBelow = item.IsBelow,
                                        CreatedById = await _ISessionService.GetUserId(),
                                        CreatedDate = DateTime.Now,
                                        IsActive = true
                                    };
                                    AppraisalStandard.Add(StandardItem);
                                }

                            }
                            _context.EmployeeAppraisalStandards.AddRange(AppraisalStandard);
                            _context.SaveChanges();

                        }
                        response.Update(AppraisalResult);

                    }
                }
                else
                {
                    response.ValidationError();
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
