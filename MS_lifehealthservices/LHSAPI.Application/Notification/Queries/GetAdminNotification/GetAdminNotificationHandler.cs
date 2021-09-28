
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
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Domain.Entities;
using INotification = LHSAPI.Application.Interface.INotification;
using CleanArchitecture.Application.Notification.Models;

namespace LHSAPI.Application.Notification.Queries.GetAdminNotification
{
    public class GetAdminNotificationHandler : IRequestHandler<GetAdminNotificationListQuery, ApiResponse>, IRequestHandler<GetEmployeeNotificationListQuery, ApiResponse>, IRequestHandler<UpdateAdminNotificationCommand, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly INotification _NotificationService;
        //   readonly ILoggerManager _logger;
        public GetAdminNotificationHandler(LHSDbContext dbContext, INotification notification)
        {
            _dbContext = dbContext;
            _NotificationService = notification;
        }

        public async Task<ApiResponse> Handle(GetAdminNotificationListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                bool IsAdminAlert = true;
                int EmployeeId = 0;
                // response = _NotificationService.GetNotification(true);
                var notificationList = (from notification in _dbContext.Notification
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
                  ).OrderByDescending(x => x.CreatedDate).ToList();
                if (notificationList != null)
                {
                    var totalCount = notificationList.Count();
                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.NotificationOrderBy.Date:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                notificationList = notificationList.OrderBy(x => x.CreatedDate).ToList();
                            }
                            else
                            {
                                notificationList = notificationList.OrderByDescending(x => x.CreatedDate).ToList();
                            }
                            break;
                        case Common.Enums.Employee.NotificationOrderBy.EmployeeName:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                notificationList = notificationList.OrderBy(x => x.EmployeeName).ToList();
                            }
                            else
                            {
                                notificationList = notificationList.OrderByDescending(x => x.EmployeeName).ToList();
                            }
                            break;
                        case Common.Enums.Employee.NotificationOrderBy.Description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                notificationList = notificationList.OrderBy(x => x.Description).ToList();
                            }
                            else
                            {
                                notificationList = notificationList.OrderByDescending(x => x.Description).ToList();
                            }
                            break;
                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                notificationList = notificationList.OrderBy(x => x.CreatedDate).ToList();
                            }
                            else
                            {
                                notificationList = notificationList.OrderByDescending(x => x.CreatedDate).ToList();
                            }

                            break;
                    }
                    var list = notificationList.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(list);
                }

            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public async Task<ApiResponse> Handle(GetEmployeeNotificationListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                response = _NotificationService.GetNotification(false, request.EmployeeId);

            }
            catch (Exception ex)
            {

            }
            return response;
        }
        public async Task<ApiResponse> Handle(UpdateAdminNotificationCommand request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                response = await _NotificationService.UpdateReadNotification(request.IsAdmin, request.EmployeeId);

            }
            catch (Exception ex)
            {

            }
            return response;
        }

    }
}
