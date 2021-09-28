
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientGuardianInfo
{
    public class AddClientGuardianCommandHandler : IRequestHandler<AddClientGuardianCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientGuardianCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientGuardianCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientGuardianInfo.FirstOrDefault(x => x.ClientId == request.ClientId  && x.IsActive == true && x.IsDeleted == false);
                    if (ExistUser == null)
                    {
                        ClientGuardianInfo _ClientPrimaryInfo = new ClientGuardianInfo();
                        _ClientPrimaryInfo.FirstName = request.FirstName;
                        _ClientPrimaryInfo.MiddleName = request.MiddleName;
                        _ClientPrimaryInfo.LastName = request.LastName;
                        _ClientPrimaryInfo.RelationShip = request.RelationShip;
                        _ClientPrimaryInfo.ContactNo = Convert.ToString(request.ContactNo);
                        _ClientPrimaryInfo.Email = request.Email;
                        _ClientPrimaryInfo.PhoneNo = Convert.ToString(request.PhoneNo);
                        _ClientPrimaryInfo.ClientId = request.ClientId;
                        _ClientPrimaryInfo.IsDeleted = false;
                        _ClientPrimaryInfo.IsActive = true;
                        _ClientPrimaryInfo.CreatedById = await _ISessionService.GetUserId();
                        _ClientPrimaryInfo.CreatedDate = DateTime.Now;
                        await _context.ClientGuardianInfo.AddAsync(_ClientPrimaryInfo);
                        _context.SaveChanges();
                        response.Success(_ClientPrimaryInfo);
                    }
                    else
                    {

                        ExistUser.FirstName = request.FirstName;
                        ExistUser.MiddleName = request.MiddleName;
                        ExistUser.LastName = request.LastName;
                        ExistUser.RelationShip = request.RelationShip;
                        ExistUser.ContactNo = Convert.ToString(request.ContactNo);
                        ExistUser.PhoneNo = Convert.ToString(request.PhoneNo);
                        ExistUser.Email = request.Email;
                        ExistUser.ClientId = request.ClientId;
                        ExistUser.IsDeleted = false;
                        ExistUser.IsActive = true;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.ClientGuardianInfo.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);
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
