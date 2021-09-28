
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using LHSAPI.Application.EmployeeStaff.Models;
using AutoMapper;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetDayReportDetails
{
    public class GetDayReportDetailsHandler : IRequestHandler<GetDayReportDetailsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetDayReportDetailsHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetDayReportDetailsQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                DayReportInfoModel _dayDetails = new DayReportInfoModel();



                _dayDetails.DayReportDetail = (from dayinfo in _dbContext.DayReportDetail
                                                         where dayinfo.IsActive == true && dayinfo.IsDeleted == false && dayinfo.ShiftId == request.ShiftId
                                                         select new LHSAPI.Application.EmployeeStaff.Models.DayReportDetail
                                                         {
                                                             Id = dayinfo.Id,
                                                             ShiftId = dayinfo.ShiftId,
                                                             HouseAddress = dayinfo.HouseAddress,
                                                             Date = dayinfo.Date,
                                                            
                                                         }).FirstOrDefault();
                _dayDetails.DayReportSupportWorkers = (from comminfo in _dbContext.DayReportSupportWorkers
                                                            where comminfo.IsDeleted == false && comminfo.IsActive == true  && comminfo.ShiftId == request.ShiftId
                                                            select new LHSAPI.Application.EmployeeStaff.Models.DayReportSupportWorkers
                                                            {
                                                                Id = comminfo.Id,
                                                                ShiftId = comminfo.ShiftId,
                                                                StaffName = comminfo.StaffName,
                                                                TimeIn = comminfo.TimeIn,
                                                                TimeOut = comminfo.TimeOut,
                                                                Signature=comminfo.Signature,
                                                                TimeInString = comminfo.CreatedDate == null ? null : comminfo.CreatedDate.Value.Date.Add(comminfo.TimeIn.Value).ToString(@"hh\:mm tt"),
                                                                TimeOutString = comminfo.CreatedDate == null ? null : comminfo.CreatedDate.Value.Date.Add(comminfo.TimeOut.Value).ToString(@"hh\:mm tt")
                                                            }).OrderByDescending(x => x.Id).ToList();
                _dayDetails.DayReportVisitor = (from comminfo in _dbContext.DayReportVisitor
                                                where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ShiftId == request.ShiftId
                                                       select new LHSAPI.Application.EmployeeStaff.Models.DayReportVisitor
                                                       {
                                                           Id = comminfo.Id,
                                                           ShiftId = comminfo.ShiftId,
                                                           VisitorName = comminfo.VisitorName,
                                                           VisitReason = comminfo.VisitReason,
                                                           Time = comminfo.Time,
                                                           TimeString = comminfo.CreatedDate == null ? null : comminfo.CreatedDate.Value.Date.Add(comminfo.Time.Value).ToString(@"hh\:mm tt")
                                                          
                                                       }).OrderByDescending(x => x.Id).ToList();

                _dayDetails.DayReportCashHandOver = (from comminfo in _dbContext.DayReportCashHandOver
                                                     where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ShiftId == request.ShiftId
                                                select new LHSAPI.Application.EmployeeStaff.Models.DayReportCashHandOver
                                                {
                                                    Id = comminfo.Id,
                                                    ShiftId = comminfo.ShiftId,
                                                    CashHandover = comminfo.CashHandover,
                                                    BalanceaBroughtAM = comminfo.BalanceaBroughtAM,
                                                    BalanceaBroughtPM = comminfo.BalanceaBroughtPM,
                                                    ExpenseAM = comminfo.ExpenseAM,
                                                    ExpensePM = comminfo.ExpensePM,
                                                    Signature=comminfo.Signature

                                                }).OrderByDescending(x => x.Id).ToList();
                _dayDetails.DayReportTelePhoneMsg = (from comminfo in _dbContext.DayReportTelePhoneMsg
                                                     where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ShiftId == request.ShiftId
                                                     select new LHSAPI.Application.EmployeeStaff.Models.DayReportTelePhoneMsg
                                                     {
                                                         Id = comminfo.Id,
                                                         ShiftId = comminfo.ShiftId,
                                                         Caller = comminfo.Caller,
                                                         Time = comminfo.Time,
                                                         MessageFor = comminfo.MessageFor,
                                                         Message = comminfo.Message,
                                                         TimeString = comminfo.CreatedDate == null ? null : comminfo.CreatedDate.Value.Date.Add(comminfo.Time.Value).ToString(@"hh\:mm tt"),
                                                         Signature = comminfo.Signature

                                                     }).OrderByDescending(x => x.Id).ToList();
                _dayDetails.DayReportAppointments = (from comminfo in _dbContext.DayReportAppointments
                                                     join shiftdata in _dbContext.ClientShiftInfo on comminfo.ShiftId equals shiftdata.ShiftId
                                                     where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ShiftId == request.ShiftId
                                                     select new LHSAPI.Application.EmployeeStaff.Models.DayReportAppointments
                                                     {
                                                         Id = comminfo.Id,
                                                         ShiftId = comminfo.ShiftId,
                                                         Time = comminfo.Time,
                                                         ClientId = comminfo.ClientId,
                                                         Details = comminfo.Details,
                                                         TimeString = comminfo.CreatedDate == null ? null : comminfo.CreatedDate.Value.Date.Add(comminfo.Time.Value).ToString(@"hh\:mm tt"),
                                                         GeneralInformation = comminfo.GeneralInformation,
                                                         ClientName = _dbContext.ClientPrimaryInfo.Where(x => x.Id == shiftdata.ClientId).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                                     }).OrderByDescending(x => x.Id).ToList();
                _dayDetails.DayReportFoodIntake = (from comminfo in _dbContext.DayReportFoodIntake
                                                   where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ShiftId == request.ShiftId
                                                     select new LHSAPI.Application.EmployeeStaff.Models.DayReportFoodIntake
                                                     {
                                                         Id = comminfo.Id,
                                                         ShiftId = comminfo.ShiftId,
                                                         ResidentName = comminfo.ResidentName,
                                                         Date = comminfo.Date,
                                                         Signature = comminfo.Signature,
                                                         Breakfast = comminfo.Breakfast,
                                                         Lunch = comminfo.Lunch,
                                                         Dinner = comminfo.Dinner,
                                                         Snacks = comminfo.Snacks,
                                                         fluids = comminfo.fluids,
                                                         StaffName = comminfo.StaffName,

                                                     }).OrderByDescending(x => x.Id).ToList();
                _dayDetails.DayReportDailyHandOver = (from comminfo in _dbContext.DayReportDailyHandOver
                                                      where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ShiftId == request.ShiftId
                                                   select new LHSAPI.Application.EmployeeStaff.Models.DayReportDailyHandOver
                                                   {
                                                      Id = comminfo.Id,
                                                      ShiftId = comminfo.ShiftId,
                                                      IsAfterNoonWorker=comminfo.IsAfterNoonWorker,
                                                      IsMorningWorker=comminfo.IsMorningWorker,
                                                      IsSleepOverWorker=comminfo.IsSleepOverWorker,
                                                      AfterNoonWorkerSign=comminfo.AfterNoonWorkerSign,
                                                      MorningWorkerSignature=comminfo.MorningWorkerSignature,
                                                      SleepOverWorkerSign=comminfo.SleepOverWorkerSign,
                                                      Description=comminfo.Description
                                                   }).OrderByDescending(x => x.Id).ToList();
                if (_dayDetails.DayReportDetail == null) _dayDetails.DayReportDetail = new LHSAPI.Application.EmployeeStaff.Models.DayReportDetail();
                if (_dayDetails.DayReportSupportWorkers == null) _dayDetails.DayReportSupportWorkers = new List<LHSAPI.Application.EmployeeStaff.Models.DayReportSupportWorkers>();
                if (_dayDetails.DayReportVisitor == null) _dayDetails.DayReportVisitor = new List<LHSAPI.Application.EmployeeStaff.Models.DayReportVisitor>();
                if (_dayDetails.DayReportCashHandOver == null) _dayDetails.DayReportCashHandOver = new List<LHSAPI.Application.EmployeeStaff.Models.DayReportCashHandOver>();
                if (_dayDetails.DayReportDailyHandOver == null) _dayDetails.DayReportDailyHandOver = new List<LHSAPI.Application.EmployeeStaff.Models.DayReportDailyHandOver>();
                if (_dayDetails.DayReportTelePhoneMsg == null) _dayDetails.DayReportTelePhoneMsg = new List<LHSAPI.Application.EmployeeStaff.Models.DayReportTelePhoneMsg>();
                if (_dayDetails.DayReportAppointments == null) _dayDetails.DayReportAppointments = new List<LHSAPI.Application.EmployeeStaff.Models.DayReportAppointments>();
                if (_dayDetails.DayReportFoodIntake == null) _dayDetails.DayReportFoodIntake = new List<LHSAPI.Application.EmployeeStaff.Models.DayReportFoodIntake>();
               
                response.SuccessWithOutMessage(_dayDetails);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        #endregion
    }
}
