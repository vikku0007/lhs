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

namespace LHSAPI.Application.Administration.Commands.Create.AddLatLongLocation
{
    public class AddLatLongLocationCommandHandler : IRequestHandler<AddLatLongLocationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddLatLongLocationCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;

        }
        public async Task<ApiResponse> Handle(AddLatLongLocationCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.Name))
                {

                    var ExistUser = _context.Location.FirstOrDefault(x => x.Name == request.Name && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        Location Loc = new Location();
                        Loc.Name = request.Name;
                        Loc.Address = request.Address;
                        Loc.CreatedById = await _ISessionService.GetUserId();
                        Loc.CreatedDate = DateTime.Now;
                        Loc.Latitude = request.Latitude;
                        Loc.Longitude = request.Longitude;
                        Loc.JobCode = request.JobCode;
                        Loc.IsActive = true;
                        Loc.ManagerId = 1;
                        Loc.Status = true;

                        await _context.Location.AddAsync(Loc);
                        _context.SaveChanges();
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
