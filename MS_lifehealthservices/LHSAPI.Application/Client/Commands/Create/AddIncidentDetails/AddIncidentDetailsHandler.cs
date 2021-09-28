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

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentDetails
{
    public class AddIncidentDetailsHandler : IRequestHandler<AddIncidentDetailsCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddIncidentDetailsHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentDetailsCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
               
                    if (request.ClientId > 0 && request.ShiftId > 0)
                    {
                       var ExistUser = _context.ClientIncidentDetails.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                  
                    if (ExistUser == null)
                    {
                        ClientIncidentDetails user = new ClientIncidentDetails();
                        user.ClientId = request.ClientId;
                        user.ShiftId = request.ShiftId;
                        user.EmployeeId = request.EmployeeId;
                        user.LocationId = request.LocationId;
                        user.LocationType = request.LocationType;
                        user.OtherLocation = request.OtherLocation;
                        user.DateTime = request.DateTime;
                        user.UnknowndateReason = request.UnknowndateReason;
                        user.NdisProviderTime = TimeSpan.Parse(request.NdisProviderTime);
                        user.NdisProviderDate = request.NdisProviderDate;
                        user.IncidentAllegtion = request.IncidentAllegtion;
                        user.AllegtionCircumstances = request.AllegtionCircumstances;
                        user.Address = request.Address;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.ClientIncidentDetails.AddAsync(user);
                        _context.SaveChanges();

                        LHSAPI.Application.Client.Models.ClientIncidentDetails Incident = new LHSAPI.Application.Client.Models.ClientIncidentDetails();
                        Incident.Id = user.Id;
                        Incident.ClientId = user.ClientId;
                        Incident.LocationId = user.LocationId;
                        Incident.LocationType = user.LocationType;
                        Incident.OtherLocation = user.OtherLocation;
                        Incident.DateTime = user.DateTime;
                        Incident.UnknowndateReason = user.UnknowndateReason;
                        Incident.NdisProviderDate = user.NdisProviderDate;
                        Incident.NdisProviderTime = user.NdisProviderTime;
                        Incident.StartTimeString = user.NdisProviderDate.Date.Add(user.NdisProviderTime).ToString(@"hh\:mm");
                        Incident.AllegtionCircumstances = user.AllegtionCircumstances;
                        Incident.IncidentAllegtion = user.IncidentAllegtion;
                        Incident.Address = user.Address;
                        response.Success(Incident);
                    }
                    else
                    {
                        ExistUser.LocationId = request.LocationId;
                        ExistUser.LocationType = request.LocationType;
                        ExistUser.OtherLocation = request.OtherLocation;
                        ExistUser.DateTime = request.DateTime;
                        ExistUser.UnknowndateReason = request.UnknowndateReason;
                        ExistUser.NdisProviderTime = TimeSpan.Parse(request.NdisProviderTime);
                        ExistUser.NdisProviderDate = request.NdisProviderDate;
                        ExistUser.IncidentAllegtion = request.IncidentAllegtion;
                        ExistUser.AllegtionCircumstances = request.AllegtionCircumstances;
                        ExistUser.Address = request.Address;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientIncidentDetails.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        LHSAPI.Application.Client.Models.ClientIncidentDetails Incident = new LHSAPI.Application.Client.Models.ClientIncidentDetails();

                        Incident.ClientId = ExistUser.ClientId;
                        Incident.LocationId = ExistUser.LocationId;
                        Incident.LocationType = ExistUser.LocationType;
                        Incident.OtherLocation = ExistUser.OtherLocation;
                        Incident.DateTime = ExistUser.DateTime;
                        Incident.UnknowndateReason = ExistUser.UnknowndateReason;
                        Incident.NdisProviderDate = ExistUser.NdisProviderDate;
                        Incident.NdisProviderTime = (ExistUser.NdisProviderTime);
                        Incident.StartTimeString = ExistUser.NdisProviderDate.Date.Add(ExistUser.NdisProviderTime).ToString(@"hh\:mm");
                        Incident.AllegtionCircumstances = ExistUser.AllegtionCircumstances;
                        Incident.IncidentAllegtion = ExistUser.IncidentAllegtion;
                        Incident.Address = ExistUser.Address;
                        response.Success(Incident);
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
