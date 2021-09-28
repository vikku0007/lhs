
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Client.Commands.Create.AddClientPicInfo
{
    public class AddClientPicInfoCommandHandler : IRequestHandler<AddClientPicInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;

        public AddClientPicInfoCommandHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientPicInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (int.Parse(request.ClientId) > 0)
                {

                    var ExistUser = _context.ClientPicInfo.FirstOrDefault(x => x.ClientId == int.Parse(request.ClientId) && x.IsActive == true && x.IsDeleted == false);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.ClientPicInfo ClientPicInfo = new LHSAPI.Domain.Entities.ClientPicInfo();
                        if (request.files.Length > 0)
                        {

                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["ClientProfileImagePath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["ClientProfileImagePath"]);

                            string fileExtension = System.IO.Path.GetExtension(request.files.FileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;

                            using (var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create))
                            {
                                request.files.CopyTo(fileStream);
                            }
                            ClientPicInfo.Path = _configuration["ClientProfileImagePath"].ToString() + "/" + guidname;
                        }
                        ClientPicInfo.ClientId = int.Parse(request.ClientId);
                        ClientPicInfo.CreatedById = await _ISessionService.GetUserId();
                        ClientPicInfo.CreatedDate = DateTime.Now;
                        ClientPicInfo.IsDeleted = false;
                        ClientPicInfo.IsActive = true;
                        await _context.ClientPicInfo.AddAsync(ClientPicInfo);
                        _context.SaveChanges();
                        response.Success(ClientPicInfo);

                    }

                    else
                    {

                        if (request.files.Length > 0)
                        {

                            string path = this._hostingEnvironment.WebRootPath + "\\" + _configuration["ClientProfileImagePath"] + "\\";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            string guidname = "";
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["ClientProfileImagePath"]);

                            string fileExtension = System.IO.Path.GetExtension(request.files.FileName);
                            guidname = Guid.NewGuid().ToString() + fileExtension;
                            using (var fileStream = new FileStream(Path.Combine(uploads, guidname), FileMode.Create))
                            {
                                request.files.CopyTo(fileStream);
                            }
                            ExistUser.Path = _configuration["ClientProfileImagePath"] + "/" + guidname;
                        }

                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.ClientPicInfo.Update(ExistUser);
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
