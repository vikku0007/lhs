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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeCommunicationInfo
{
    public class AddEmployeeCommunicationInfoCommandHandler : IRequestHandler<AddEmployeeCommunicationInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private readonly IMessageService _IMessageService;
        public AddEmployeeCommunicationInfoCommandHandler(LHSDbContext context, ISessionService ISessionService, IMessageService IMessageService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _IMessageService = IMessageService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeCommunicationInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {
                    // var ExistUser = _context.EmployeeCommunicationInfo.FirstOrDefault(x => x.EmployeeId == request.EmployeeId && x.IsActive == true);
                    // if (ExistUser == null)
                    // {
                    EmployeeCommunicationInfo user = new EmployeeCommunicationInfo();
                    user.EmployeeId = request.EmployeeId;
                    user.CreatedById = await _ISessionService.GetUserId();
                    user.CreatedDate = DateTime.Now;
                    user.Subject = request.Subject;
                    user.IsActive = true;
                    user.Message = request.Message;
                    await _context.EmployeeCommunicationInfo.AddAsync(user);
                    _context.SaveChanges();

                    foreach (var id in request.AssignedTo)
                    {
                        CommunicationRecipient comm = new CommunicationRecipient();
                        comm.EmployeeId = id;
                        comm.CommunicationId = user.Id;
                        comm.IsDeleted = false;
                        comm.CreatedDate = DateTime.Now;
                        comm.IsActive = true;
                        comm.CreatedById = await _ISessionService.GetUserId();
                        await _context.CommunicationRecipient.AddAsync(comm);
                        _context.SaveChanges();
                        string EmailId = _context.EmployeePrimaryInfo.Where(x => x.Id == comm.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmailId).FirstOrDefault();
                        string UserName = _context.EmployeePrimaryInfo.Where(x => x.Id == comm.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();
                       
                        if (!string.IsNullOrEmpty(EmailId))
                        {
                            string emailBody = _IMessageService.GetCommunicationTemplate();
                            string Message = request.Message;
                            string Subject = request.Subject;
                            emailBody = emailBody.Replace("{Message}", Message);
                            emailBody = emailBody.Replace("{Subject}", Subject);
                            emailBody = emailBody.Replace("{UserName}", UserName);
                            _IMessageService.SendingEmails(EmailId, Subject, emailBody);
                        }
                    }

                    response.Success(user);
                    // }                    
                }
                else
                {
                    response.AlreadyExist();
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
