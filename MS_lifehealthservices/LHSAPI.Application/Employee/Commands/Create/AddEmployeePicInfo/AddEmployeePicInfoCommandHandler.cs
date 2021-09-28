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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeePicInfo
{
    public class AddEmployeePicInfoCommandHandler : IRequestHandler<AddEmployeePicInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;

        public AddEmployeePicInfoCommandHandler(LHSDbContext context,ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeePicInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (int.Parse(request.EmployeeId) > 0)
                {

                    var ExistUser = _context.EmployeePicInfo.FirstOrDefault(x => x.EmployeeId == int.Parse(request.EmployeeId) && x.IsActive == true && x.IsDeleted == false);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeePicInfo EmployeePicInfo = new LHSAPI.Domain.Entities.EmployeePicInfo();
                        if (request.files.Length > 0)
                        {

                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["profilePath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["profilePath"]);

                            string fileExtension = System.IO.Path.GetExtension(request.files.FileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;

                            using (var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create))
                            {
                                request.files.CopyTo(fileStream);
                            }
                            EmployeePicInfo.Path = _configuration["profilePath"].ToString() + "/" + guidname;
                        }
                        EmployeePicInfo.EmployeeId = int.Parse(request.EmployeeId);
                        EmployeePicInfo.CreatedById = await _ISessionService.GetUserId();
                        EmployeePicInfo.CreatedDate = DateTime.Now;
                        EmployeePicInfo.IsDeleted = false;
                        EmployeePicInfo.IsActive = true;

                        await _context.EmployeePicInfo.AddAsync(EmployeePicInfo);

                        _context.SaveChanges();

                        response.Success(EmployeePicInfo);

                    }
                    else
                    {

                        if (request.files.Length > 0)
                        {

                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["profilePath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["profilePath"]);

                            string fileExtension = System.IO.Path.GetExtension(request.files.FileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;
                            using (var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create))
                            {
                                request.files.CopyTo(fileStream);
                            }
                            ExistUser.Path = _configuration["profilePath"] + "/" + guidname;
                        }

                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.EmployeePicInfo.Update(ExistUser);
                        _context.SaveChanges();

                        response.Success(ExistUser);

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
