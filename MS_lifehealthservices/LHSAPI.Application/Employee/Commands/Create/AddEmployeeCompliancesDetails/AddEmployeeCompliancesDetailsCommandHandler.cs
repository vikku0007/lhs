using LHSAPI.Application.Employee.Models;
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeCompliancesDetails
{
    public class AddEmployeeCompliancesDetailsCommandHandler : IRequestHandler<AddEmployeeCompliancesDetailsCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;

        public AddEmployeeCompliancesDetailsCommandHandler(LHSDbContext context, ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeCompliancesDetailsCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (int.Parse(request.EmployeeId) > 0)
                {

                    var ExistUser = _context.EmployeeCompliancesDetails.FirstOrDefault(x => x.EmployeeId == int.Parse(request.EmployeeId) && x.DocumentName == int.Parse(request.DocumentName) && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        EmployeeCompliancesDetails user = new EmployeeCompliancesDetails();
                        user.EmployeeId = int.Parse(request.EmployeeId);
                        user.DocumentName = int.Parse(request.DocumentName);
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.TrainingType = int.Parse(request.TrainingType);
                        user.IsActive = true;
                        if (request.ExpiryDate== "null")
                        {
                            user.ExpiryDate = null;
                        }
                        else
                        {
                            user.ExpiryDate = DateTime.Parse(request.ExpiryDate);
                        }
                        if (request.IssueDate == "null")
                        {
                            user.IssueDate = null;
                        }
                        else
                        {
                            user.IssueDate = DateTime.Parse(request.IssueDate);
                        }

                        user.Description = request.Description;
                        if (request.HasExpiry == "1")
                        {
                            user.HasExpiry = true;
                        }
                        else
                        {
                            user.HasExpiry = false;
                        }
                        if (request.Alert == "1")
                        {
                            user.Alert = true;
                        }
                        else
                        {
                            user.Alert = false;
                        }
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
                            user.FileName = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        await _context.EmployeeCompliancesDetails.AddAsync(user);
                        _context.SaveChanges();
                        EmployeeComplianceModel model = new Models.EmployeeComplianceModel();
                        model.Id = user.Id;
                        model.EmployeeId = user.EmployeeId;
                        model.DocumentName = user.DocumentName;
                        model.CreatedDate = user.CreatedDate;
                      //  model.DocumentType = user.DocumentType;
                        model.IsActive = user.IsActive;
                        model.ExpiryDate = user.ExpiryDate;
                        model.IssueDate = user.IssueDate;
                        model.Description = user.Description;
                        model.HasExpiry = user.HasExpiry;
                        model.Alert = user.Alert;
                      //  model.DocumentTypeName = _context.StandardCode.Where(x => x.ID == user.DocumentType).Select(x => x.CodeDescription).FirstOrDefault();
                       
                        response.Success(model);

                    }
                    else
                    {
                        response.AlreadyExist();
                    }
                }
                else
                {

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
