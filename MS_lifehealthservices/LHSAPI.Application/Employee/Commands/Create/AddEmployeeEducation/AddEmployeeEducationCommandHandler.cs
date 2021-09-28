
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeEducation
{
    public class AddEmployeeEducationCommandHandler : IRequestHandler<AddEmployeeEducationCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;

        public AddEmployeeEducationCommandHandler(LHSDbContext context, ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeEducationCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (int.Parse(request.EmployeeId) > 0)
                {
                    LHSAPI.Domain.Entities.EmployeeEducation EmployeeEducation = new LHSAPI.Domain.Entities.EmployeeEducation();
                    var ExistUser = _context.EmployeeEducation.Where(x => x.EmployeeId == int.Parse(request.EmployeeId) & x.IsActive == true &&
                    x.Id == request.Id && x.Id != 0).FirstOrDefault();
                    if (ExistUser == null)
                    {

                        EmployeeEducation.EmployeeId = int.Parse(request.EmployeeId);
                        EmployeeEducation.Degree = request.Degree;
                        EmployeeEducation.CompletionDate = DateTime.Parse(request.CompletionDate);
                        EmployeeEducation.AdditionalNotes = request.AdditionalNotes;
                        EmployeeEducation.FieldStudy = request.FieldStudy;
                        EmployeeEducation.Institute = request.Institute;
                        EmployeeEducation.QualificationType = request.QualificationType;
                        EmployeeEducation.CreatedById = await _ISessionService.GetUserId();
                        EmployeeEducation.IsActive = true;
                        EmployeeEducation.CreatedDate = DateTime.Now;
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
                            EmployeeEducation.DocumentPath = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        await _context.EmployeeEducation.AddAsync(EmployeeEducation);
                        _context.SaveChanges();
                        response.Success(EmployeeEducation);

                    }
                    else
                    {
                        ExistUser.EmployeeId = int.Parse(request.EmployeeId);
                        ExistUser.Degree = request.Degree;
                        ExistUser.CompletionDate = DateTime.Parse(request.CompletionDate);
                        ExistUser.AdditionalNotes = request.AdditionalNotes;
                        ExistUser.FieldStudy = request.FieldStudy;
                        ExistUser.Institute = request.Institute;
                        ExistUser.QualificationType = request.QualificationType;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.CreatedDate = DateTime.Now;
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
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
                            ExistUser.DocumentPath = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        _context.EmployeeEducation.Update(ExistUser);
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
