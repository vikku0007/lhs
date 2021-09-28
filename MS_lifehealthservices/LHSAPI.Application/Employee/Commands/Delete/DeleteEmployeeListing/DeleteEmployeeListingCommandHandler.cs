using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeListing
{
    public class DeleteEmployeeListingCommandHandler : IRequestHandler<DeleteEmployeeListingCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteEmployeeListingCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteEmployeeListingCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistEmp = _context.EmployeePrimaryInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                      
                    if (ExistEmp == null)
                    {
                        response.NotFound();
                    }
                    else
                    {
                        var AccidentEmp = _context.EmployeeAccidentInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var AppraisalEmp = _context.EmployeeAppraisalDetails.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var ApprsltandEmp = _context.EmployeeAppraisalStandards.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var AwardInfoEmp = _context.EmployeeAwardInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var CommEmp = _context.EmployeeCommunicationInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var reqCommplianceEmp = _context.EmployeeCompliancesDetails.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var DrivingEmp = _context.EmployeeDrivingLicenseInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var EducationEmp = _context.EmployeeEducation.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var JobEmp = _context.EmployeeJobProfile.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var KinEmp = _context.EmployeeKinInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var LeaveEmp = _context.EmployeeLeaveInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var MiscEmp = _context.EmployeeMiscInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var OthcomplEmp = _context.EmployeeOtherComplianceDetails.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var PayrateEmp = _context.EmployeePayRate.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var PicInfoEmp = _context.EmployeePicInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var ShitEmp = _context.EmployeeShiftInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var StaffwarningEmp = _context.EmployeeStaffWarning.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var WorkExpEmp = _context.EmployeeWorkExp.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var globalrateEmp = _context.GlobalPayRate.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);

                        if (AccidentEmp != null)
                        {
                            AccidentEmp.IsDeleted = true;
                            AccidentEmp.IsActive = false;
                            AccidentEmp.DeletedDate = DateTime.UtcNow;
                            AccidentEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeAccidentInfo.Update(AccidentEmp);
                        }
                        if (AppraisalEmp != null)
                        {
                           AppraisalEmp.IsDeleted = true;
                           AppraisalEmp.IsActive = false;
                           AppraisalEmp.DeletedDate = DateTime.UtcNow;
                           AppraisalEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeAppraisalDetails.Update(AppraisalEmp);
                        }
                        if (ApprsltandEmp != null)
                        {
                            ApprsltandEmp.IsDeleted = true;
                            ApprsltandEmp.IsActive = false;
                            ApprsltandEmp.DeletedDate = DateTime.UtcNow;
                            ApprsltandEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeAppraisalStandards.Update(ApprsltandEmp);
                        }
                        if (AwardInfoEmp != null)
                        {
                           AwardInfoEmp.IsDeleted = true;
                           AwardInfoEmp.IsActive = false;
                           AwardInfoEmp.DeletedDate = DateTime.UtcNow;
                           AwardInfoEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeAwardInfo.Update(AwardInfoEmp);
                        }
                        if (CommEmp != null)
                        {
                            CommEmp.IsDeleted = true;
                            CommEmp.IsActive = false;
                            CommEmp.DeletedDate = DateTime.UtcNow;
                            CommEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeCommunicationInfo.Update(CommEmp);
                        }
                        if (reqCommplianceEmp != null)
                        {
                            reqCommplianceEmp.IsDeleted = true;
                            reqCommplianceEmp.IsActive = false;
                            reqCommplianceEmp.DeletedDate = DateTime.UtcNow;
                            reqCommplianceEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeCompliancesDetails.Update(reqCommplianceEmp);
                        }
                        if (OthcomplEmp != null)
                        {
                            OthcomplEmp.IsDeleted = true;
                            OthcomplEmp.IsActive = false;
                            OthcomplEmp.DeletedDate = DateTime.UtcNow;
                            OthcomplEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeOtherComplianceDetails.Update(OthcomplEmp);
                        }
                        if (DrivingEmp != null)
                        {
                           DrivingEmp.IsDeleted = true;
                           DrivingEmp.IsActive = false;
                           DrivingEmp.DeletedDate = DateTime.UtcNow;
                           DrivingEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeDrivingLicenseInfo.Update(DrivingEmp);
                        }
                        if (EducationEmp != null)
                        {
                            EducationEmp.IsDeleted = true;
                            EducationEmp.IsActive = false;
                            EducationEmp.DeletedDate = DateTime.UtcNow;
                            EducationEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeEducation.Update(EducationEmp);
                        }
                        if (JobEmp != null)
                        {
                            JobEmp.IsDeleted = true;
                            JobEmp.IsActive = false;
                            JobEmp.DeletedDate = DateTime.UtcNow;
                            JobEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeJobProfile.Update(JobEmp);
                        }
                        if (KinEmp != null)
                        {
                            KinEmp.IsDeleted = true;
                            KinEmp.IsActive = false;
                            KinEmp.DeletedDate = DateTime.UtcNow;
                            KinEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeKinInfo.Update(KinEmp);
                        }
                        if (LeaveEmp != null)
                        {
                            LeaveEmp.IsDeleted = true;
                            LeaveEmp.IsActive = false;
                            LeaveEmp.DeletedDate = DateTime.UtcNow;
                            LeaveEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeLeaveInfo.Update(LeaveEmp);
                        }
                        if (MiscEmp != null)
                        {
                            MiscEmp.IsDeleted = true;
                            MiscEmp.IsActive = false;
                            MiscEmp.DeletedDate = DateTime.UtcNow;
                            MiscEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeMiscInfo.Update(MiscEmp);
                        }
                        if (PayrateEmp != null)
                        {
                            PayrateEmp.IsDeleted = true;
                            PayrateEmp.IsActive = false;
                            PayrateEmp.DeletedDate = DateTime.UtcNow;
                            PayrateEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeePayRate.Update(PayrateEmp);
                        }
                        if (ShitEmp != null)
                        {
                            ShitEmp.IsDeleted = true;
                            ShitEmp.IsActive = false;
                            ShitEmp.DeletedDate = DateTime.UtcNow;
                            ShitEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeShiftInfo.Update(ShitEmp);
                        }
                        if (StaffwarningEmp != null)
                        {
                            StaffwarningEmp.IsDeleted = true;
                            StaffwarningEmp.IsActive = false;
                            StaffwarningEmp.DeletedDate = DateTime.UtcNow;
                            StaffwarningEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeStaffWarning.Update(StaffwarningEmp);
                        }
                        if (WorkExpEmp != null)
                        {
                            WorkExpEmp.IsDeleted = true;
                            WorkExpEmp.IsActive = false;
                            WorkExpEmp.DeletedDate = DateTime.UtcNow;
                            WorkExpEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.EmployeeWorkExp.Update(WorkExpEmp);
                        }
                        if (PayrateEmp != null)
                        {
                            PayrateEmp.IsDeleted = true;
                            PayrateEmp.IsActive = false;
                            PayrateEmp.DeletedDate = DateTime.UtcNow;
                            PayrateEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.GlobalPayRate.Update(globalrateEmp);
                        }
                        

                        ExistEmp.IsDeleted = true;
                        ExistEmp.IsActive = false;
                        ExistEmp.DeletedDate = DateTime.UtcNow;
                        ExistEmp.DeletedById = await _ISessionService.GetUserId();
                        _context.EmployeePrimaryInfo.Update(ExistEmp);
                        await _context.SaveChangesAsync();
                        response.Delete(ExistEmp);

                    }
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
