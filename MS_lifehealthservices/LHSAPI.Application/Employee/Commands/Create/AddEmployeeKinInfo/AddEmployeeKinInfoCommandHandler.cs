
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeKinInfo
{
    public class AddEmployeeKinInfoCommandHandler : IRequestHandler<AddEmployeeKinInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddEmployeeKinInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeKinInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeKinInfo.FirstOrDefault(x => x.EmployeeId == request.EmployeeId & x.IsActive == true);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeeKinInfo EmployeeKinInfo = new LHSAPI.Domain.Entities.EmployeeKinInfo();
                        EmployeeKinInfo.EmployeeId = request.EmployeeId;
                        EmployeeKinInfo.FirstName = request.FirstName;
                        EmployeeKinInfo.MiddelName = request.MiddelName;
                        EmployeeKinInfo.LastName = request.LastName;
                        EmployeeKinInfo.RelationShip = request.RelationShip;
                        EmployeeKinInfo.ContactNo = request.ContactNo;
                        EmployeeKinInfo.Email = request.Email;
                        EmployeeKinInfo.OtherRelation = request.OtherRelation;
                        EmployeeKinInfo.IsActive = true;
                        EmployeeKinInfo.CreatedById = await _ISessionService.GetUserId();
                        EmployeeKinInfo.CreatedDate = DateTime.Now;
                        await _context.EmployeeKinInfo.AddAsync(EmployeeKinInfo);
                        _context.SaveChanges();
                        LHSAPI.Application.Employee.Models.EmployeeKinInfo kin = new Models.EmployeeKinInfo();
                        kin.FirstName = request.FirstName;
                        kin.MiddelName = request.MiddelName;
                        kin.LastName = request.LastName;
                        kin.RelationShip = request.RelationShip;
                        kin.ContactNo = request.ContactNo;
                        kin.Email = request.Email;
                        kin.Id = EmployeeKinInfo.Id;
                        kin.EmployeeId = EmployeeKinInfo.EmployeeId;
                        kin.EmployeeId = EmployeeKinInfo.EmployeeId;
                        kin.OtherRelation = EmployeeKinInfo.OtherRelation;
                        kin.RelationShipName = _context.StandardCode.Where(x => x.ID == kin.RelationShip).Select(x => x.CodeDescription).FirstOrDefault();
                        response.Success(kin);

                    }
                    else
                    {
                        ExistUser.FirstName = request.FirstName;
                        ExistUser.MiddelName = request.MiddelName;
                        ExistUser.LastName = request.LastName;
                        ExistUser.RelationShip = request.RelationShip;
                        ExistUser.ContactNo = request.ContactNo;
                        ExistUser.Email = request.Email;
                        ExistUser.OtherRelation = request.OtherRelation;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.EmployeeKinInfo.Update(ExistUser);
                        LHSAPI.Application.Employee.Models.EmployeeKinInfo kin = new Models.EmployeeKinInfo();
                        kin.FirstName = request.FirstName;
                        kin.MiddelName = request.MiddelName;
                        kin.LastName = request.LastName;
                        kin.RelationShip = request.RelationShip;
                        kin.ContactNo = request.ContactNo;
                        kin.OtherRelation = request.OtherRelation;
                        kin.Email = request.Email;
                        kin.Id = ExistUser.Id;
                        kin.EmployeeId = ExistUser.EmployeeId;
                        kin.RelationShipName = _context.StandardCode.Where(x => x.ID == kin.RelationShip).Select(x => x.CodeDescription).FirstOrDefault();


                        await _context.SaveChangesAsync();
                        response.Update(kin);

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
