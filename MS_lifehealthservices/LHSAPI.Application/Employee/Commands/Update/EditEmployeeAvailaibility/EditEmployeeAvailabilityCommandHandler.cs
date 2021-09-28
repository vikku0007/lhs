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

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeAvailability
{
  public class EditEmployeeAvailabilityCommandHandler : IRequestHandler<EditEmployeeAvailabilityCommand, ApiResponse>
  {
    private readonly LHSDbContext _context;
    
    public EditEmployeeAvailabilityCommandHandler(LHSDbContext context)
    {
      _context = context;
   
    }

    public async Task<ApiResponse> Handle(EditEmployeeAvailabilityCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeAvailabilityDetails.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        
                       ExistEmp.AvailabilityDay = request.AvailabilityDay;
                       ExistEmp.StartTime = request.StartTime;
                       ExistEmp.EndTime = request.EndTime;
                       ExistEmp.WorksMon = request.WorksMon;
                       ExistEmp.UpdateById = 1;
                       ExistEmp.UpdatedDate = DateTime.Now;
                         _context.EmployeeAvailabilityDetails.Update(ExistEmp);
                        _context.SaveChanges();
                        response.Status = (int)Number.One;
                        response.ResponseData = ExistEmp;
                        response.Message = ResponseMessage.UpdateSuccess;

                    }
                    else
                    {
                        response.Status = (int)Number.Zero;
                        response.Message = ResponseMessage.Exist;

                    }
                }
                else
                {

                }
            }

      
      catch (Exception ex)
      {
        throw ex;

      }
      return response;

    }
  }
}
