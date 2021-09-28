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

namespace LHSAPI.Application.Administration.Commands.Update.EditLocation
{
  public class EditLocationCommandHandler : IRequestHandler<EditLocationCommand, ApiResponse>
  {
    private readonly LHSDbContext _context;
    
    public EditLocationCommandHandler(LHSDbContext context)
    {
      _context = context;
   
    }

    public async Task<ApiResponse> Handle(EditLocationCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.Location.FirstOrDefault(x => x.LocationId == request.LocationId && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        var Loc = _context.Location.Where(x => x.LocationId == request.LocationId).FirstOrDefault();
                        Loc.Name = request.Name;
                        Loc.Address = request.Address;
                        Loc.UpdateById = 1;
                        Loc.UpdatedDate = DateTime.Now;
                        Loc.WeekDay = request.WeekDay;
                        Loc.IsActive = true;
                        Loc.City = request.City;
                        Loc.State = request.State;
                        Loc.Country = request.Country;
                        Loc.CalenderView = request.CalenderView;
                        Loc.ExternalCode = request.ExternalCode;
                        Loc.InvoicePrefix = request.InvoicePrefix;
                        Loc.ManagerId = request.ManagerId;
                        Loc.ManagerContact = request.ManagerContact;
                        Loc.City = request.City;
                        Loc.AdditionalSetting = request.AdditionalSetting;
                        Loc.Description = request.Description;
                        Loc.Status = request.Status;
                        _context.Location.Update(Loc);
                        _context.SaveChanges();
                        response.Status = (int)Number.One;
                        response.Message = ResponseMessage.UpdateSuccess;

                    }
                    else
                    {
                        response.Status = (int)Number.One;
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
