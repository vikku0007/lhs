
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientProgressNotesItem
{
    public class AddClientProgressNotesItemCommandHandler : IRequestHandler<AddClientProgressNotesItemCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private readonly IMessageService _IMessageService;
        private readonly IConfiguration _configuration;
        public AddClientProgressNotesItemCommandHandler(LHSDbContext context, ISessionService ISessionService, IMessageService IMessageService, IConfiguration configuration)
        {
            _context = context;
            _ISessionService = ISessionService;
            _IMessageService = IMessageService;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(AddClientProgressNotesItemCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0  && request.ShiftId > 0)
                {

                   var ExistUser = _context.ProgressNotesList.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.Date == request.Date
                   && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                   
                    if (ExistUser == null)
                    {
                        ProgressNotesList ProgressNotesList = new ProgressNotesList();
                        ProgressNotesList.ClientId = request.ClientId;
                        ProgressNotesList.ShiftId = request.ShiftId;
                        ProgressNotesList.EmployeeId = request.EmployeeId;
                        ProgressNotesList.ClientProgressNoteId = request.ClientProgressNoteId;
                        ProgressNotesList.Date = request.Date;
                        ProgressNotesList.Note9AMTo11AM = request.Note9AMTo11AM;
                        ProgressNotesList.Note11AMTo1PM = request.Note11AMTo1PM;
                        ProgressNotesList.Note1PMTo15PM = request.Note1PMTo15PM;
                        ProgressNotesList.Note15PMTo17PM = request.Note15PMTo17PM;
                        ProgressNotesList.Note17PMTo19PM = request.Note17PMTo19PM;
                        ProgressNotesList.Note19PMTo21PM = request.Note19PMTo21PM;
                        ProgressNotesList.Note21PMTo23PM = request.Note21PMTo23PM;
                        ProgressNotesList.Note23PMTo1AM = request.Note23PMTo1AM;
                        ProgressNotesList.Note1AMTo3AM = request.Note1AMTo3AM;
                        ProgressNotesList.Note3AMTo5AM = request.Note3AMTo5AM;
                        ProgressNotesList.Note5AMTo7AM = request.Note5AMTo7AM;
                        ProgressNotesList.Note7AMTo9AM = request.Note7AMTo9AM;
                        ProgressNotesList.Summary = request.Summary;
                        ProgressNotesList.OtherInfo = request.OtherInfo;
                        ProgressNotesList.CreatedById = await _ISessionService.GetUserId();
                        ProgressNotesList.CreatedDate = DateTime.Now;
                        ProgressNotesList.IsActive = true;
                        ProgressNotesList.IsDeleted = false;
                        await _context.ProgressNotesList.AddAsync(ProgressNotesList);
                        _context.SaveChanges();                        
                        var ExistHistory = _context.ShiftHistoryInfo.Where(x => x.ShiftId == request.ShiftId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                        if (ExistHistory != null)
                        {
                            ExistHistory.ProgreeNotesId = ProgressNotesList.Id;
                            ExistHistory.Description = "Progress Notes Filled";
                            ExistHistory.IsActive = true;
                            ExistHistory.IsDeleted = false;
                            _context.ShiftHistoryInfo.Update(ExistHistory);
                            _context.SaveChanges();
                        }
                        response.Success(ProgressNotesList);
                        //int EmployeeId = _context.EmployeeShiftInfo.Where(x => x.Id == request.ShiftId && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmployeeId).FirstOrDefault();
                        //string EmailId = _context.EmployeePrimaryInfo.Where(x => x.Id == EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmailId).FirstOrDefault();
                        //string UserName = _context.EmployeePrimaryInfo.Where(x => x.Id == EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();
                        //var _ShiftInfo = _context.ShiftInfo.FirstOrDefault(x => x.Id == request.ShiftId && x.IsActive == true && x.IsDeleted == false);
                        //if (!string.IsNullOrEmpty(EmailId))
                        //{
                        //    string emailBody = _IMessageService.GetEmailTemplate();
                        //    string Message = "Progress Notes are filled for Shift";
                        //    string Date = " Start Date :" + _ShiftInfo.StartDate.ToShortDateString() + "and End Date :" + _ShiftInfo.EndDate.ToShortDateString();
                        //    string Time = " Start Time: " + _ShiftInfo.StartDate.Add(_ShiftInfo.StartTime).ToString(@"hh\:mm tt") + "- End Time: " + _ShiftInfo.StartDate.Add(_ShiftInfo.EndTime).ToString(@"hh\:mm tt");
                        //    string Subject = "Progress Notes Filled";
                        //    emailBody = emailBody.Replace("{Message}", Message);
                        //    emailBody = emailBody.Replace("{Subject}", Subject);
                        //    emailBody = emailBody.Replace("{UserName}", UserName);
                        //    emailBody = emailBody.Replace("{Date}", Date);
                        //    emailBody = emailBody.Replace("{Time}", Time);
                        //    _IMessageService.SendingEmailsAsynWithCC(EmailId, Subject, emailBody, _configuration.GetSection("SMTP:CC_RegistrationEmail").Value, null);
                        //}
                    }
                    else
                    {
                        response.AlreadyExist();
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
