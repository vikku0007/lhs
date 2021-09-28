
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

namespace LHSAPI.Application.Employee.Commands.Update.UpdateEmployeeTraining
{
    public class UpdateEmployeeTrainingCommandHandler : IRequestHandler<UpdateEmployeeTrainingCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;
        public UpdateEmployeeTrainingCommandHandler(LHSDbContext context,ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(UpdateEmployeeTrainingCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {
                    LHSAPI.Domain.Entities.EmployeeTraining Employeetraining = new LHSAPI.Domain.Entities.EmployeeTraining();
                    var ExistUser = _context.EmployeeTraining.Where(x => x.Id == request.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    if (ExistUser != null)
                    {

                        ExistUser.EmployeeId = request.EmployeeId;
                        ExistUser.MandatoryTraining = request.MandatoryTraining;
                        ExistUser.TrainingType = request.TrainingType;
                        ExistUser.StartDate = request.StartDate;
                        ExistUser.EndDate = request.EndDate;
                        ExistUser.CourseType = request.CourseType;
                        ExistUser.Remarks = request.Remarks;
                        ExistUser.OtherTraining = request.OtherTraining;
                        if (request.IsAlert == "1")
                        {
                            ExistUser.IsAlert = true;
                        }
                        else
                        {
                            ExistUser.IsAlert = false;
                        }
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
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
                            ExistUser.FileName = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        _context.EmployeeTraining.Update(ExistUser);
                        _context.SaveChanges();
                        LHSAPI.Application.Employee.Models.EmployeeTraining model = new Models.EmployeeTraining();
                        model.Id = ExistUser.Id;
                        model.EmployeeId = ExistUser.EmployeeId;
                        model.MandatoryTraining = ExistUser.MandatoryTraining;
                        model.TrainingType = ExistUser.TrainingType;
                        model.StartDate = ExistUser.StartDate;
                        model.EndDate = ExistUser.EndDate;
                        model.CourseType = ExistUser.CourseType;
                        model.Remarks = ExistUser.Remarks;
                        model.IsAlert = ExistUser.IsAlert;
                        model.CourseTypeName = _context.StandardCode.Where(x => x.ID == model.CourseType).Select(x => x.CodeDescription).FirstOrDefault();
                        model.MandatoryName = _context.StandardCode.Where(x => x.ID == model.MandatoryTraining).Select(x => x.CodeDescription).FirstOrDefault();
                        model.TrainingTypeName = _context.StandardCode.Where(x => x.ID == model.TrainingType).Select(x => x.CodeDescription).FirstOrDefault();
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
