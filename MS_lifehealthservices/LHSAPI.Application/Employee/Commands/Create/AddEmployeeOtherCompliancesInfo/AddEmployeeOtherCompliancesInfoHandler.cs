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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeOtherCompliancesInfo
{
    public class AddEmployeeOtherCompliancesInfoCommandHandler : IRequestHandler<AddEmployeeOtherCompliancesInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;

        public AddEmployeeOtherCompliancesInfoCommandHandler(LHSDbContext context, ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeOtherCompliancesInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (int.Parse(request.EmployeeId) > 0)
                {

                    var ExistUser = _context.EmployeeOtherComplianceDetails.FirstOrDefault(x => x.EmployeeId == int.Parse(request.EmployeeId) && x.OtherDocumentName == int.Parse(request.OtherDocumentName) && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        EmployeeOtherComplianceDetails user = new EmployeeOtherComplianceDetails();
                        user.EmployeeId = int.Parse(request.EmployeeId);
                        user.OtherDocumentName = int.Parse(request.OtherDocumentName);
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        // user.OtherDocumentType = int.Parse(request.OtherDocumentType);
                        user.IsActive = true;
                        user.OtherExpiryDate = DateTime.Parse(request.OtherExpiryDate);
                        user.OtherIssueDate = DateTime.Parse(request.OtherIssueDate);
                        user.OtherDescription = request.OtherDescription;

                        if (request.OtherHasExpiry == "1")
                        {
                            user.OtherHasExpiry = true;
                        }
                        else
                        {
                            user.OtherHasExpiry = false;
                        }
                        if (request.OtherAlert == "1")
                        {
                            user.OtherAlert = true;
                        }
                        else
                        {
                            user.OtherAlert = false;
                        }
                        //user.HasExpiry = Boolean.Parse(request.HasExpiry);
                        //user.Alert = Boolean.Parse(request.Alert);
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
                            user.OtherFileName = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }

                        await _context.EmployeeOtherComplianceDetails.AddAsync(user);
                        _context.SaveChanges();
                        EmployeeOtherComplianceModel model = new Models.EmployeeOtherComplianceModel();
                        model.Id = user.Id;
                        model.EmployeeId = user.EmployeeId;
                        model.OtherDocumentName = user.OtherDocumentName;
                        // model.OtherDocumentType = user.OtherDocumentType;
                        model.OtherExpiryDate = user.OtherExpiryDate;
                        model.OtherIssueDate = user.OtherIssueDate;
                        model.OtherDescription = user.OtherDescription;
                        model.OtherHasExpiry = user.OtherHasExpiry;
                        model.OtherAlert = user.OtherAlert;
                        //  model.OtherDocumentTypeName = _context.StandardCode.Where(x => x.ID == user.OtherDocumentType).Select(x => x.CodeDescription).FirstOrDefault();
                        //  model.OtherFileName = user.OtherFileName;
                        response.Success(model);
                        //response.Success(user);

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
