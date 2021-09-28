
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Client.Commands.Create.AddClientSupportCoordinator
{
    public class AddClientSupportCoordinatorCommandHandler : IRequestHandler<AddClientSupportCoordinatorCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientSupportCoordinatorCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientSupportCoordinatorCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientSupportCoordinatorInfo.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false);
                    if (ExistUser == null)
                    {
                        ClientSupportCoordinatorInfo _ClientSupportCoordinatorInfo = new ClientSupportCoordinatorInfo();
                        _ClientSupportCoordinatorInfo.FirstName = request.FirstName;
                        _ClientSupportCoordinatorInfo.MiddleName = request.MiddleName;
                        _ClientSupportCoordinatorInfo.LastName = request.LastName;
                        _ClientSupportCoordinatorInfo.OtherRelation = request.OtherRelation;
                        _ClientSupportCoordinatorInfo.Relationship = request.Relationship;
                        _ClientSupportCoordinatorInfo.CreatedById = await _ISessionService.GetUserId();
                        _ClientSupportCoordinatorInfo.ClientId = request.ClientId;
                        _ClientSupportCoordinatorInfo.CreatedDate = DateTime.Now;
                        _ClientSupportCoordinatorInfo.Gender = request.Gender;
                        _ClientSupportCoordinatorInfo.IsActive = true;
                        _ClientSupportCoordinatorInfo.EmailId = request.EmailId;
                        _ClientSupportCoordinatorInfo.Salutation = request.Salutation;
                        _ClientSupportCoordinatorInfo.MobileNo = request.MobileNo;
                        _ClientSupportCoordinatorInfo.PhoneNo = request.PhoneNo;
                        _ClientSupportCoordinatorInfo.IsActive = true;
                        _ClientSupportCoordinatorInfo.IsDeleted = false;
                        await _context.ClientSupportCoordinatorInfo.AddAsync(_ClientSupportCoordinatorInfo);
                        _context.SaveChanges();
                        response.Success(_ClientSupportCoordinatorInfo);

                    }
                    else
                    {
                        ExistUser.FirstName = request.FirstName;
                        ExistUser.MiddleName = request.MiddleName;
                        ExistUser.LastName = request.LastName;
                        ExistUser.OtherRelation = request.OtherRelation;
                        ExistUser.Relationship = request.Relationship;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.ClientId = request.ClientId;
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.Gender = request.Gender;
                        ExistUser.IsActive = true;
                        ExistUser.EmailId = request.EmailId;
                        ExistUser.Salutation = request.Salutation;
                        ExistUser.MobileNo = request.MobileNo;
                        ExistUser.PhoneNo = request.PhoneNo;
                        _context.ClientSupportCoordinatorInfo.Update(ExistUser);
                        _context.SaveChanges();
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
