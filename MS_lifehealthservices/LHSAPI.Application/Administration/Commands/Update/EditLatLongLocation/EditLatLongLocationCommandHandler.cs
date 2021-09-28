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

namespace LHSAPI.Application.Administration.Commands.Update.EditLatLongLocation
{
    public class EditLatLongLocationCommandHandler : IRequestHandler<EditLatLongLocationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public EditLatLongLocationCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;

        }

        public async Task<ApiResponse> Handle(EditLatLongLocationCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.Name))
                {

                    var Loc = _context.Location.FirstOrDefault(x => x.LocationId == request.LocationId && x.IsActive == true && x.IsDeleted == false);
                    if (Loc != null)
                    {
                        Loc.Name = request.Name;
                        Loc.Address = request.Address;
                        Loc.Latitude = request.Latitude;
                        Loc.Longitude = request.Longitude;
                        Loc.IsActive = true;
                        Loc.UpdateById = await _ISessionService.GetUserId();
                        Loc.UpdatedDate = DateTime.UtcNow;
                        Loc.JobCode = request.JobCode;
                        _context.Location.Update(Loc);
                        _context.SaveChanges();
                        response.Update(Loc);
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
