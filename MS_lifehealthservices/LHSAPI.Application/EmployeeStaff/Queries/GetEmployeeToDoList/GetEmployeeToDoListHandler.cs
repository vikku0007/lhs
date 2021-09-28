using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LHSAPI.Application.EmployeeStaff.Models;

namespace LHSAPI.Application.Shift.Queries.GetEmployeeToDoList
{
    public class GetEmployeeToDoListHandler : IRequestHandler<GetEmployeeToDoListQuery, ApiResponse>, IRequestHandler<GetEmployeeNewToDoListQuery, ApiResponse>,
        IRequestHandler<GetEmployeeEditToDoListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeToDoListHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetEmployeeToDoListQuery request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                //// List<ShiftInfoViewModel> list = new List<ShiftInfoViewModel>();
                //var list = (from tooOShift in _dbContext.ToDoShift
                //            join ShiftToDO in _dbContext.ShiftToDoList on tooOShift.LocationId equals location.LocationId
                //            join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                //            join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                //            join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                //            where emShift.IsDeleted == false && emShift.IsActive == true && emShift.EmployeeId == request.EmployeeId
                //            && (request.SearchTextBylocation == 0 || location.LocationId == request.SearchTextBylocation)
                //            && (request.SearchTextByStatus == 0 || status.ID == request.SearchTextByStatus)
                //            && (string.IsNullOrEmpty(request.SearchTextByManualAddress) || shiftdata.OtherLocation.Contains(request.SearchTextByManualAddress))
                //            select new ShiftInfoViewModel()
                //            {
                //                Id = shiftdata.Id,
                //                Description = shiftdata.Description,
                //                ClientCount = shiftdata.ClientCount,
                //                EmployeeCount = shiftdata.EmployeeCount,
                //                StartDate = shiftdata.StartDate.Date.Add(shiftdata.StartTime),
                //                StartTime = shiftdata.StartTime,
                //                EndDate = shiftdata.EndDate.Date.Add(shiftdata.EndTime),
                //                EndTime = shiftdata.EndTime,
                //                StatusId = emShift.StatusId,
                //                IsPublished = shiftdata.IsPublished,
                //                LocationId = shiftdata.LocationId,
                //                StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                //                EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                //                Duration = shiftdata.Duration,
                //                LocationName = _dbContext.Location.Where(x => x.LocationId == shiftdata.LocationId).Select(x => x.Name).FirstOrDefault(),
                //                Reminder = shiftdata.Reminder,
                //                CreatedDate=shiftdata.CreatedDate,
                //                StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId).Select(x => x.CodeDescription).FirstOrDefault(),
                //                ClientShiftInfoViewModel = (from data in _dbContext.ClientPrimaryInfo
                //                                            join clShift in _dbContext.ClientShiftInfo on data.Id equals clShift.ClientId
                //                                            where data.IsDeleted == false && data.IsActive == true && ((request.SearchByClientName == 0 && shiftdata.Id == shiftdata.Id) || (request.SearchByClientName > 0 && data.Id == request.SearchByClientName && shiftdata.Id == shiftdata.Id))
                //                                             && clShift.ShiftId == shiftdata.Id
                //                                            select new ClientShiftInfoViewModel
                //                                            {
                //                                                Id = shiftdata.Id,
                //                                                ClientId = data.Id,
                //                                                Name = data.FirstName + " " + (data.MiddleName == null ? "" : data.MiddleName) + " " + data.LastName,
                //                                            }).ToList()
                //            });
                //if (list != null && list.Any())
                //{
                //    switch (request.OrderBy)
                //    {
                //        case Common.Enums.Employee.EmployeeShiftOrderBy.description:
                //            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                //            {
                //                list = list.OrderBy(x => x.Description);
                //            }
                //            else
                //            {
                //                list = list.OrderByDescending(x => x.Description);
                //            }
                //            break;

                //        case Common.Enums.Employee.EmployeeShiftOrderBy.location:
                //            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                //            {
                //                list = list.OrderBy(x => x.LocationName);
                //            }
                //            else
                //            {
                //                list = list.OrderByDescending(x => x.LocationName);
                //            }
                //            break;
                //        case Common.Enums.Employee.EmployeeShiftOrderBy.status:
                //            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                //            {
                //                list = list.OrderBy(x => x.StatusName);
                //            }
                //            else
                //            {
                //                list = list.OrderByDescending(x => x.StatusName);
                //            }
                //            break;
                //        default:
                //            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                //            {
                //                list = list.OrderBy(x => x.CreatedDate);
                //            }
                //            else
                //            {
                //                list = list.OrderByDescending(x => x.CreatedDate);
                //            }

                //            break;
                //    }

                //    var clientlist = list.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                //    var  totalCount = list.Count();
                //    response.Total = totalCount;
                //    response.SuccessWithOutMessage(clientlist);



                //}
                //else
                //{
                //    response = response.NotFound();
                //}


            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }

        public async Task<ApiResponse> Handle(GetEmployeeNewToDoListQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var toDoList = (from todo in _dbContext.ToDoShift
                                join shiftType in _dbContext.StandardCode on todo.ShiftType equals shiftType.ID
                                where todo.EmployeeId == request.EmployeeId
                                && todo.ShiftId == request.ShiftId
                                && todo.IsActive == true && todo.IsDeleted == false
                                && shiftType.IsActive == true && shiftType.IsDeleted == false
                                select new ToDoShift()
                                {
                                    Id = todo.Id,
                                    DateTime = todo.DateTime,
                                    ShiftTimeString = todo.DateTime.Value.Date.Add(todo.ShiftTime).ToString(@"hh\:mm tt"),
                                    ShiftTypeString = shiftType.CodeDescription
                                }).ToList();
                response.SuccessWithOutMessage(toDoList);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<ApiResponse> Handle(GetEmployeeEditToDoListQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var todoList = (from todo in _dbContext.ToDoShift
                                join code in _dbContext.StandardCode on todo.ShiftType equals code.ID
                                where todo.Id == request.ToDoItemId
                                && todo.IsActive == true && todo.IsDeleted == false
                                select new EditToDoShift
                                {
                                    Id = todo.Id,
                                    Date = todo.DateTime,
                                    Time = todo.DateTime.Value.Date.Add(todo.ShiftTime).ToString(@"hh\:mm tt"),
                                    ShiftTypeId = todo.ShiftType,
                                    ToDoItemList = (from shiftToDo in _dbContext.ShiftToDoList
                                                    where shiftToDo.TodoItemId == request.ToDoItemId
                                                    select new ToDoItem

                                                    {
                                                        Id = shiftToDo.Id,
                                                        Description = shiftToDo.Description,
                                                        Initials = shiftToDo.Initials,
                                                        IsInitials = shiftToDo.IsInitials
                                                    }).ToList()

                                }).FirstOrDefault();
                response.SuccessWithOutMessage(todoList);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        #endregion
    }
}
