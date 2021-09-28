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


namespace LHSAPI.Application.Client.Commands.Create.UpdateClientPrimaryInfo
{
    public class UpdateClientPrimaryInfoCommandHandler : IRequestHandler<UpdateClientPrimaryInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
       
        public UpdateClientPrimaryInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            
        }

        public async Task<ApiResponse> Handle(UpdateClientPrimaryInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var _ClientPrimaryInfo = _context.ClientPrimaryInfo.FirstOrDefault(x => x.Id == request.Id & x.IsActive == true && x.IsDeleted == false);
                    if (_ClientPrimaryInfo != null)
                    {
                        _ClientPrimaryInfo.FirstName = request.FirstName;
                        _ClientPrimaryInfo.LastName = request.LastName;
                        _ClientPrimaryInfo.UpdateById = await _ISessionService.GetUserId();
                        _ClientPrimaryInfo.UpdatedDate = DateTime.Now;
                        _ClientPrimaryInfo.Gender = request.Gender;
                        _ClientPrimaryInfo.IsActive = true;
                        _ClientPrimaryInfo.DateOfBirth = request.DateOfBirth;
                        _ClientPrimaryInfo.EmailId = request.EmailId;
                        _ClientPrimaryInfo.MaritalStatus = request.MaritalStatus;
                        _ClientPrimaryInfo.Salutation = request.Salutation;
                        _ClientPrimaryInfo.LocationId = request.LocationId;
                        _ClientPrimaryInfo.MiddleName = request.MiddleName;
                        _ClientPrimaryInfo.MobileNo = request.MobileNo;
                        _ClientPrimaryInfo.EmployeeId = request.EmployeeId;
                        _ClientPrimaryInfo.Address = request.Address;
                        _ClientPrimaryInfo.ServiceType = request.ServiceType;
                        _ClientPrimaryInfo.NDIS = request.NDIS;
                        _ClientPrimaryInfo.LocationType = request.LocationType;
                        _ClientPrimaryInfo.OtherLocation = request.OtherLocation;
                        //   _ClientPrimaryInfo.Status = request.Status;
                        _context.ClientPrimaryInfo.Update(_ClientPrimaryInfo);
                        await _context.SaveChangesAsync();
                        response.Update(_ClientPrimaryInfo);
                      
                    }
                    else
                    {
                        response.NotFound();
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
