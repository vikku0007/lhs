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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeTraining
{
    public class AddEmployeeTrainingCommandHandler : IRequestHandler<AddEmployeeTrainingCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _ISessionService;
        public AddEmployeeTrainingCommandHandler(LHSDbContext context, ISessionService ISessionService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _ISessionService = ISessionService;

        }

        public async Task<ApiResponse> Handle(AddEmployeeTrainingCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {
                    LHSAPI.Domain.Entities.EmployeeTraining Employeetraining = new LHSAPI.Domain.Entities.EmployeeTraining();
                    var ExistUser = _context.EmployeeTraining.Where(x => x.EmployeeId == request.EmployeeId && x.MandatoryTraining == request.MandatoryTraining && x.StartDate == request.StartDate && x.EndDate == request.EndDate && x.IsActive == true
          ).FirstOrDefault();
                    if (ExistUser == null)
                    {

                        Employeetraining.EmployeeId = request.EmployeeId;
                        Employeetraining.MandatoryTraining = request.MandatoryTraining;
                        Employeetraining.TrainingType = request.TrainingType;
                        Employeetraining.StartDate = request.StartDate;
                        Employeetraining.EndDate = request.EndDate;
                        Employeetraining.CourseType = request.CourseType;
                        Employeetraining.Remarks = request.Remarks;
                        Employeetraining.OtherTraining = request.OtherTraining;
                        if (request.IsAlert == "1")
                        {
                            Employeetraining.IsAlert = true;
                        }
                        else
                        {
                            Employeetraining.IsAlert = false;
                        }
                        Employeetraining.CreatedById = await _ISessionService.GetUserId();
                        Employeetraining.IsActive = true;
                        Employeetraining.CreatedDate = DateTime.Now;
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
                            Employeetraining.FileName = _configuration["EmployeeDocumentPath"] + "/" + guidname;
                        }
                        await _context.EmployeeTraining.AddAsync(Employeetraining);
                        _context.SaveChanges();
                        LHSAPI.Application.Employee.Models.EmployeeTraining model = new Models.EmployeeTraining();
                        model.Id = Employeetraining.Id;
                        model.EmployeeId = Employeetraining.EmployeeId;
                        model.MandatoryTraining = Employeetraining.MandatoryTraining;
                        model.TrainingType = Employeetraining.TrainingType;
                        model.StartDate = Employeetraining.StartDate;
                        model.EndDate = Employeetraining.EndDate;
                        model.CourseType = Employeetraining.CourseType;
                        model.Remarks = Employeetraining.Remarks;
                        model.IsAlert = Employeetraining.IsAlert;
                        model.CourseTypeName = _context.StandardCode.Where(x => x.ID == model.CourseType).Select(x => x.CodeDescription).FirstOrDefault();
                        model.MandatoryName = _context.StandardCode.Where(x => x.ID == model.MandatoryTraining).Select(x => x.CodeDescription).FirstOrDefault();
                        model.TrainingTypeName = _context.StandardCode.Where(x => x.ID == model.TrainingType).Select(x => x.CodeDescription).FirstOrDefault();
                        response.Success(model);

                    }
                    else
                    {
                        response.AlreadyExist();

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
