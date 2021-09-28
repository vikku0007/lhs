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

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeCommunication
{
    public class EditEmployeeCommunicationCommandHandler : IRequestHandler<EditEmployeeCommunicationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public EditEmployeeCommunicationCommandHandler(LHSDbContext context,ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(EditEmployeeCommunicationCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeCommunicationInfo.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {

                        
                        ExistEmp.Subject = request.Subject;
                        ExistEmp.Message = request.Message;
                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                        _context.EmployeeCommunicationInfo.Update(ExistEmp);
                        _context.SaveChanges();
                        response.Update(ExistEmp);

                        foreach (var id in request.EmployeeId)
                        {
                            CommunicationRecipient comm = new CommunicationRecipient();
                            comm.EmployeeId = id;
                            comm.CommunicationId = ExistEmp.Id;
                            comm.IsDeleted = false;
                            comm.CreatedDate = DateTime.Now;
                            comm.IsActive = true;
                            comm.CreatedById = await _ISessionService.GetUserId();
                            await _context.CommunicationRecipient.AddAsync(comm);
                        }

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
