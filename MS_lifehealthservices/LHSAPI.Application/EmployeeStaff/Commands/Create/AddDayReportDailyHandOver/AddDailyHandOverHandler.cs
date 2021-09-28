
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportDailyHandOver
{
    public class AddDailyHandOverHandler : IRequestHandler<AddDailyHandOverCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
       
        public AddDailyHandOverHandler(LHSDbContext context,ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            

        }

        public async Task<ApiResponse> Handle(AddDailyHandOverCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ShiftId > 0)
                {


                    var ExistUser = _context.DayReportDetail.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        DayReportDetail dayreport = new DayReportDetail();
                        dayreport.ShiftId = request.ShiftId;
                        dayreport.CreatedById = await _ISessionService.GetUserId();
                        dayreport.CreatedDate = DateTime.Now;
                        dayreport.HouseAddress = request.HouseAddress;
                        dayreport.IsActive = true;
                        dayreport.Date = request.Date;
                        await _context.DayReportDetail.AddAsync(dayreport);
                        _context.SaveChanges();
                        List<DayReportDailyHandOver> Shiftlist = new List<DayReportDailyHandOver>();
                        if (request.DailyHandOverItem != null && request.DailyHandOverItem.Count > 0)
                        {
                            foreach (var item in request.DailyHandOverItem)
                            {
                                if (item.IsAfterNoonWorker == false && item.IsMorningWorker == false && item.IsSleepOverWorker == false)
                                {

                                }
                                else
                                {
                                    DayReportDailyHandOver StandardItem = new DayReportDailyHandOver

                                    {


                                        Description = item.Description,
                                        ShiftId = request.ShiftId,
                                        IsMorningWorker = item.IsMorningWorker,
                                        IsSleepOverWorker = item.IsSleepOverWorker,
                                        IsAfterNoonWorker = item.IsAfterNoonWorker,
                                        MorningWorkerSignature = item.MorningWorkerSignature,
                                        AfterNoonWorkerSign = item.AfterNoonWorkerSign,
                                        SleepOverWorkerSign = item.SleepOverWorkerSign,
                                        CreatedById = await _ISessionService.GetUserId(),
                                        CreatedDate = DateTime.Now,
                                        IsActive = true
                                    };
                                    Shiftlist.Add(StandardItem);
                                }

                            }
                            _context.DayReportDailyHandOver.AddRange(Shiftlist);
                            _context.SaveChanges();

                        }
                        response.Success(Shiftlist);
                    }
                    else
                    {

                        List<DayReportDailyHandOver> Shiftlist = new List<DayReportDailyHandOver>();
                        if (request.DailyHandOverItem != null && request.DailyHandOverItem.Count > 0)
                        {
                            var dayList = (from todo in _context.DayReportDailyHandOver where todo.ShiftId == request.ShiftId select todo).ToList();
                            foreach (var item in dayList)
                            {
                                item.IsActive = false;
                                item.IsDeleted = true;
                            }
                            _context.SaveChanges();

                            foreach (var item in request.DailyHandOverItem)
                            {
                                if (item.IsAfterNoonWorker == false && item.IsMorningWorker == false && item.IsSleepOverWorker == false)
                                {

                                }
                                else
                                {
                                    DayReportDailyHandOver StandardItem = new DayReportDailyHandOver

                                    {


                                        Description = item.Description,
                                        ShiftId = request.ShiftId,
                                        IsMorningWorker = item.IsMorningWorker,
                                        IsSleepOverWorker = item.IsSleepOverWorker,
                                        IsAfterNoonWorker = item.IsAfterNoonWorker,
                                        MorningWorkerSignature = item.MorningWorkerSignature,
                                        AfterNoonWorkerSign = item.AfterNoonWorkerSign,
                                        SleepOverWorkerSign = item.SleepOverWorkerSign,
                                        UpdateById = await _ISessionService.GetUserId(),
                                        UpdatedDate = DateTime.Now,
                                       
                                    };
                                    Shiftlist.Add(StandardItem);
                                }

                            }
                            _context.DayReportDailyHandOver.AddRange(Shiftlist);
                            _context.SaveChanges();
                           
                        }
                        response.Success(Shiftlist);
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
