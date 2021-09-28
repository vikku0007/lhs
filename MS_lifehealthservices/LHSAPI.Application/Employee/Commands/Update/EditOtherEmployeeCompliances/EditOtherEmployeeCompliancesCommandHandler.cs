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

namespace LHSAPI.Application.Employee.Commands.Update.EditOtherEmployeeCompliances
{
  public class EditOtherEmployeeCompliancesCommandHandler : IRequestHandler<EditOtherEmployeeCompliancesCommand, ApiResponse>
  {
    private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;
        public EditOtherEmployeeCompliancesCommandHandler(LHSDbContext context, ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
    {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

    public async Task<ApiResponse> Handle(EditOtherEmployeeCompliancesCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _context.EmployeeOtherComplianceDetails.FirstOrDefault(x => x.Id == int.Parse(request.Id) && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        
                        ExistEmp.OtherDocumentName = int.Parse(request.OtherDocumentName);
                       
                        ExistEmp.IsActive = true;
                        ExistEmp.OtherExpiryDate = DateTime.Parse(request.OtherExpiryDate);
                        ExistEmp.OtherIssueDate = DateTime.Parse(request.OtherIssueDate);
                        ExistEmp.OtherDescription = request.OtherDescription;
                        if (request.OtherHasExpiry == "1")
                        {
                            ExistEmp.OtherHasExpiry = true;
                        }
                        else
                        {
                            ExistEmp.OtherHasExpiry = false;
                        }
                        if (request.OtherAlert == "1")
                        {
                            ExistEmp.OtherAlert = true;
                        }
                        else
                        {
                            ExistEmp.OtherAlert = false;
                        }
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
                            ExistEmp.OtherFileName = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        if (!String.IsNullOrEmpty(request.OtherFileName))
                        {
                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["EmployeeDocumentPath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["EmployeeDocumentPath"]);

                            string fileExtension = System.IO.Path.GetExtension(request.OtherFileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;
                            var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create);
                            ExistEmp.OtherFileName = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }


                        ExistEmp.UpdateById = await _ISessionService.GetUserId();
                        ExistEmp.UpdatedDate = DateTime.Now;
                         _context.EmployeeOtherComplianceDetails.Update(ExistEmp);
                        _context.SaveChanges();
                        response.Update(ExistEmp);
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
