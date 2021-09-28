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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAppraisalDetails
{
    public class AddEmployeeAppraisalDetailsCommandHandler : IRequestHandler<AddEmployeeAppraisalDetailsCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddEmployeeAppraisalDetailsCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeAppraisalDetailsCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeAppraisalDetails.FirstOrDefault(x => x.EmployeeId == request.EmployeeId && x.AppraisalDateFrom == request.AppraisalDateFrom && x.AppraisalDateTo == request.AppraisalDateTo && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        EmployeeAppraisalDetails Appraisl = new EmployeeAppraisalDetails();
                        Appraisl.EmployeeId = request.EmployeeId;
                        // Appraisl.DepartmentName = request.DepartmentName;
                        Appraisl.CreatedById = await _ISessionService.GetUserId();
                        Appraisl.CreatedDate = DateTime.Now;
                        Appraisl.AppraisalType = request.AppraisalType;
                        Appraisl.IsActive = true;
                        Appraisl.AppraisalDateFrom = request.AppraisalDateFrom;
                        Appraisl.AppraisalDateTo = request.AppraisalDateTo;
                        await _context.EmployeeAppraisalDetails.AddAsync(Appraisl);
                        _context.SaveChanges();
                        int AppraisalId = _context.EmployeeAppraisalDetails.Max(x => x.Id);

                        List<EmployeeAppraisalStandards> AppraisalStandard = new List<EmployeeAppraisalStandards>();
                        if (request.StandardList != null && request.StandardList.Count > 0)
                        {
                            foreach (var item in request.StandardList)
                            {
                                if (item.IsExceeds == false && item.IsAchieves == false && item.IsBelow == false)
                                {

                                }
                                else
                                {
                                    EmployeeAppraisalStandards StandardItem = new EmployeeAppraisalStandards

                                    {
                                        AppraisalId = AppraisalId,
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
                        response.Success(Appraisl);

                    }
                    else
                    {
                        response.AlreadyExist();

                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);

                //response.Status = (int)Number.Zero;
                //response.Message = ResponseMessage.Error;

            }
            return response;

        }
    }
}
