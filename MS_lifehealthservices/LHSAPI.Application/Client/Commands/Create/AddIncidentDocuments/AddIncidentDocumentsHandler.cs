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
using LHSAPI.Application.Client.Models;
using Microsoft.Extensions.Configuration;
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentDocuments
{
    public class AddIncidentDocumentsHandler : IRequestHandler<AddIncidentDocumentsCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;

        public AddIncidentDocumentsHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddIncidentDocumentsCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {

                if (int.Parse(request.ClientId) > 0 && int.Parse(request.ShiftId) > 0)
                {
                    var ExistUser = _context.IncidentDocumentDetails.FirstOrDefault(x => x.ClientId == int.Parse(request.ClientId) && x.IsActive == true && x.IsDeleted == false && x.ShiftId == int.Parse(request.ShiftId));

                    if (ExistUser == null)
                    {
                        IncidentDocumentDetails user = new IncidentDocumentDetails();
                        user.ClientId = int.Parse(request.ClientId);
                        user.ShiftId = int.Parse(request.ShiftId);
                        user.EmployeeId = int.Parse(request.EmployeeId);
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        user.DocumentName = request.DocumentName;
                        if (request.files == null)
                        {

                        }
                        else if (!String.IsNullOrEmpty(request.files.FileName))
                        {
                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["ClientDocumentPath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["ClientDocumentPath"]);
                            string fileExtension = System.IO.Path.GetExtension(request.files.FileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;
                            using (var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create))
                            {
                                request.files.CopyTo(fileStream);
                            }
                            user.FileName = _configuration["ClientDocumentPath"] + "/" + guidname;
                        }
                        await _context.IncidentDocumentDetails.AddAsync(user);
                        _context.SaveChanges();
                        response.Success(user);

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
