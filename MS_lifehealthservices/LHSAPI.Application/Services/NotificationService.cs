using CleanArchitecture.Application.Notification.Models;
using LHSAPI.Application.Interface;


using LHSAPI.Common.ApiResponse;
using LHSAPI.Common.CommonMethods;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Services
{
    public enum NotiFicationSaveMode
    {
        InBoth,
        Employee,
        Adminn,
    }
    public class NotificationService : INotification
    {
        private readonly LHSDbContext _dbContext;
        private readonly ISessionService _ISessionService;
        public NotificationService(LHSDbContext dbContext, ISessionService ISessionService)
        {
            _dbContext = dbContext;
            _ISessionService = ISessionService;
        }


        public async Task<ApiResponse> SaveNotification(LHSAPI.Domain.Entities.Notification notification, NotiFicationSaveMode mode)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (notification != null && !string.IsNullOrEmpty(notification.EventName) && !string.IsNullOrEmpty(notification.Description))
                {
                    if (mode == NotiFicationSaveMode.Adminn)
                    {
                        notification.CreatedById = await _ISessionService.GetUserId();
                        notification.CreatedDate = DateTime.Now;
                        notification.IsActive = true;
                        notification.IsAdminAlert = true;
                        await _dbContext.Notification.AddAsync(notification);
                        _dbContext.SaveChanges();

                        response.Success(notification);
                    }
                    if (mode == NotiFicationSaveMode.Employee)
                    {
                        notification.CreatedById = await _ISessionService.GetUserId();
                        notification.CreatedDate = DateTime.Now;
                        notification.IsActive = true;
                        notification.IsAdminAlert = false;
                        await _dbContext.Notification.AddAsync(notification);
                        _dbContext.SaveChanges();

                        response.Success(notification);
                    }
                    else
                    {
                        notification.CreatedById = await _ISessionService.GetUserId();
                        notification.CreatedDate = DateTime.Now;
                        notification.IsActive = true;
                        notification.IsAdminAlert = true;
                        await _dbContext.Notification.AddAsync(notification);

                        var Employeenotification = new Domain.Entities.Notification();
                        Employeenotification.EventName = notification.EventName;
                        Employeenotification.Description = notification.Description;
                        Employeenotification.Email = notification.Email;
                        Employeenotification.CreatedById = await _ISessionService.GetUserId();
                        Employeenotification.CreatedDate = DateTime.Now;
                        Employeenotification.IsActive = true;
                        Employeenotification.IsAdminAlert = false;
                        await _dbContext.Notification.AddAsync(Employeenotification);
                        _dbContext.SaveChanges();

                        response.Success(notification);
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
                throw ex;
            }
            return response;
        }
        public async Task<ApiResponse> UpdateReadNotification(bool IsAdmin, int EmployeeId = 0)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lnotification = _dbContext.Notification.Where(x => x.IsAdminAlert == IsAdmin && x.IsReaded == false || x.EmployeeId == EmployeeId).ToList();
                if (lnotification != null)
                {
                    foreach (var item in lnotification)
                    {
                        item.UpdateById = await _ISessionService.GetUserId();
                        item.UpdatedDate = DateTime.Now;
                        item.IsReaded = true;
                    }
                    _dbContext.Notification.UpdateRange(lnotification);
                    _dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.Success();
                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
                throw ex;

            }
            return response;
        }

        public ApiResponse GetNotification(bool IsAdminAlert = true, int EmployeeId = 0)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var lnotification = (from notification in _dbContext.Notification
                                     join emp in _dbContext.EmployeePrimaryInfo on notification.EmployeeId equals emp.Id
                                     where emp.IsDeleted == false && emp.IsActive == true && notification.IsReaded == false && ((IsAdminAlert && notification.IsAdminAlert == true) || (IsAdminAlert == false && EmployeeId > 0 && emp.Id == EmployeeId))
                                     select new NotificationViewModel()
                                     {
                                         Id = notification.Id,
                                         EmployeeId = notification.EmployeeId,
                                         Description = notification.Description,
                                         EmployeeName = emp.FirstName + " " + (string.IsNullOrEmpty(emp.MiddleName) ? "" : emp.MiddleName + " ") + emp.LastName,
                                         EventName = notification.EventName,
                                         CreatedDate = notification.CreatedDate
                                     }
                 ).OrderByDescending(x => x.CreatedDate).ToList(); ;
                response.Total = lnotification.Count;
                response.SuccessWithOutMessage(lnotification);

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
                throw ex;

            }
            return response;
        }
    }
}
