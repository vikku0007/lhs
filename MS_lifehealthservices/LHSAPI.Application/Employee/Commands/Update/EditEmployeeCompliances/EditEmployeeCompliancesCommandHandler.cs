using LHSAPI.Application.Employee.Models;
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeCompliances
{
    public class EditEmployeeCompliancesCommandHandler : IRequestHandler<EditEmployeeCompliancesCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;
        public EditEmployeeCompliancesCommandHandler(LHSDbContext context,  ISessionService ISessionService,IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;

        }

        public async Task<ApiResponse> Handle(EditEmployeeCompliancesCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeCompliancesDetails.FirstOrDefault(x => x.Id == int.Parse(request.Id) && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        EmployeeComplianceModel model = new EmployeeComplianceModel();
                        ExistEmp.DocumentName = int.Parse(request.DocumentName);
                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                        ExistEmp.TrainingType = int.Parse(request.TrainingType);
                        ExistEmp.IsActive = true;
                        if (request.ExpiryDate == "null")
                        {
                            ExistEmp.ExpiryDate = null;
                        }
                        else
                        {
                            ExistEmp.ExpiryDate = DateTime.Parse(request.ExpiryDate);
                        }
                        if (request.IssueDate == "null")
                        {
                            ExistEmp.IssueDate = null;
                        }
                        else
                        {
                            ExistEmp.IssueDate = DateTime.Parse(request.IssueDate);
                        }
                        ExistEmp.Description = request.Description;
                        if (request.HasExpiry == "1")
                        {
                            ExistEmp.HasExpiry = true;
                        }
                        else
                        {
                            ExistEmp.HasExpiry = false;
                        }
                        if (request.Alert == "1")
                        {
                            ExistEmp.Alert = true;
                        }
                        else
                        {
                            ExistEmp.Alert = false;
                        }
                        //user.HasExpiry = Boolean.Parse(request.HasExpiry);
                        //user.Alert = Boolean.Parse(request.Alert);
                        if (request.files == null)
                        {

                        }
                        
                     else  if (!String.IsNullOrEmpty(request.files.FileName))
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
                            ExistEmp.FileName = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        _context.EmployeeCompliancesDetails.Update(ExistEmp);
                        _context.SaveChanges();
                        // Add entity to model
                        model.Id = ExistEmp.Id;
                        model.DocumentName = ExistEmp.DocumentName;
                        model.UpdatedDate = ExistEmp.UpdatedDate;
                        model.IsActive = ExistEmp.IsActive;
                        model.ExpiryDate = ExistEmp.ExpiryDate;
                        model.IssueDate = ExistEmp.IssueDate;
                        model.Description = ExistEmp.Description;
                        model.HasExpiry = ExistEmp.HasExpiry;
                        model.Alert = ExistEmp.Alert;
                        response.Update(model);

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
