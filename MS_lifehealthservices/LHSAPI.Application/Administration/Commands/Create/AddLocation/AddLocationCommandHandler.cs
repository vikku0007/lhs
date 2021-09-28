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

namespace LHSAPI.Application.Administration.Commands.Create.AddLocation
{
    public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddLocationCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;

        }

        public async Task<ApiResponse> Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.Name))
                {

                    var ExistUser = _context.Location.FirstOrDefault(x => x.Name == request.Name && x.City == request.City && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        Location Loc = new Location();
                        Loc.Name = request.Name;
                        Loc.Address = request.Address;
                        Loc.CreatedById = await _ISessionService.GetUserId();
                        Loc.CreatedDate = DateTime.Now;
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
                        //await _context.Location.AddAsync(Loc);
                        //_context.SaveChanges();
                        response.Status = (int)Number.One;
                        response.Message = ResponseMessage.Success;

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
