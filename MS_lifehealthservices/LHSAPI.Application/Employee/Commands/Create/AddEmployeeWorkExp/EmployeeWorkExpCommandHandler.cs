
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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeWorkExp
{
    public class EmployeeWorkExpCommandHandler : IRequestHandler<AddEmployeeWorkExpCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;

        public EmployeeWorkExpCommandHandler(LHSDbContext context, ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeWorkExpCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeWorkExp.FirstOrDefault(x => x.EmployeeId == request.EmployeeId & x.IsActive == true
                    && x.Id == request.Id && request.Id != 0
                    //x.JobTitle == request.JobTitle
                    );
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeeWorkExp EmployeeWorkExp = new LHSAPI.Domain.Entities.EmployeeWorkExp();
                        EmployeeWorkExp.EmployeeId = request.EmployeeId;
                        EmployeeWorkExp.JobDesc = request.JobDesc;
                        EmployeeWorkExp.JobTitle = request.JobTitle;
                        EmployeeWorkExp.StartDate = request.StartDate;
                        EmployeeWorkExp.Company = request.Company;
                        EmployeeWorkExp.EndDate = request.EndDate;
                        EmployeeWorkExp.Duration = request.Duration;
                        EmployeeWorkExp.CreatedById = await _ISessionService.GetUserId();
                        EmployeeWorkExp.CreatedDate = DateTime.Now;
                        EmployeeWorkExp.IsActive = true;
                        if (request.files == null)
                        {

                        }

                        else if (!String.IsNullOrEmpty(request.files.FileName))
                        {
                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["EmployeeDocumentPath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["EmployeeDocumentPath"]);

                            string fileExtension = System.IO.Path.GetExtension(request.files.FileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;
                            // var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create);
                            using (var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create))
                            {
                                request.files.CopyTo(fileStream);
                            }
                            EmployeeWorkExp.DocumentPath = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        await _context.EmployeeWorkExp.AddAsync(EmployeeWorkExp);
                        _context.SaveChanges();
                        response.Success(EmployeeWorkExp);

                    }
                    else
                    {
                        var user = _context.EmployeeWorkExp.Where(x => x.Id == request.Id).FirstOrDefault();
                        user.EmployeeId = request.EmployeeId;
                        user.JobDesc = request.JobDesc;
                        user.JobTitle = request.JobTitle;
                        user.StartDate = request.StartDate;
                        user.Company = request.Company;
                        user.EndDate = request.EndDate;
                        user.Duration = request.Duration;
                        user.UpdateById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.UpdatedDate = DateTime.Now;
                        user.IsActive = true;
                        if (request.files == null)
                        {

                        }

                        else if (!String.IsNullOrEmpty(request.files.FileName))
                        {
                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["EmployeeDocumentPath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["EmployeeDocumentPath"]);

                            string fileExtension = System.IO.Path.GetExtension(request.files.FileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;
                            // var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create);
                            using (var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create))
                            {
                                request.files.CopyTo(fileStream);
                            }
                            user.DocumentPath = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        _context.EmployeeWorkExp.Update(user);
                        _context.SaveChanges();
                        response.Update(ExistUser);

                    }
                }
                else
                {
                    response.ValidationError();
                    //var existrecord = _context.UserRegister.FirstOrDefault(x => x.Id == request.Id);
                    //if (existrecord != null)
                    //{
                    //  existrecord.FirstName = request.Firstname;
                    //  existrecord.LastName = request.LastName;
                    //  existrecord.MiddleName = request.MiddleName;
                    //  //existrecord.EmailId = request.EmailId;
                    //  //   existrecord.Password = request.Password;
                    //  existrecord.UpdatedDate = DateTime.UtcNow;
                    //  //existrecord.PhoneNo = request.Pho;
                    //  //existrecord.OTP = number;
                    //  //existrecord.OTPStartDateTime = DateTime.UtcNow;
                    //  //existrecord.OTPEndDateTime = DateTime.UtcNow.AddMinutes(5);

                    //  _context.Update(existrecord);
                    //  await _context.SaveChangesAsync();
                    //  //SendOTPMessage.SendMessage(request.PhoneNo, number);

                    //  response.Status = (int)Number.One;
                    //  response.Message = ResponseMessage.Success;
                    //  response.ResponseData = existrecord;
                    //}
                    //else
                    //{
                    //  response.Status = (int)Number.Zero;
                    //  response.Message = ResponseMessage.PhoneExist;
                    //}

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
