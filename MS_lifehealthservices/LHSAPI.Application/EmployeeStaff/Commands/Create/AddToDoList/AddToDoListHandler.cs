using LHSAPI.Application.Interface;
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddToDoList
{
    public class AddToDoListHandler : IRequestHandler<AddToDoListCommand, ApiResponse>, IRequestHandler<UpdateToDoListCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddToDoListHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddToDoListCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.ToDoShift.FirstOrDefault(x => x.ShiftType == request.ShiftType && x.DateTime == request.DateTime
                     && x.IsActive == true
                     //&& x.ClientId==request.ClientId 
                     && x.ShiftId == request.ShiftId);
                    if (ExistUser == null)
                    {
                        ToDoShift todolist = new ToDoShift();
                        todolist.EmployeeId = request.EmployeeId;
                        //todolist.ClientId = request.ClientId;
                        todolist.ShiftId = request.ShiftId;
                        todolist.CreatedById = await _ISessionService.GetUserId();
                        todolist.CreatedDate = DateTime.Now;
                        todolist.ShiftType = request.ShiftType;
                        todolist.IsActive = true;
                        todolist.DateTime = request.DateTime;
                        todolist.ShiftTime = TimeSpan.Parse(request.ShiftTime);
                        await _context.ToDoShift.AddAsync(todolist);
                        _context.SaveChanges();


                        List<ShiftToDoList> Shiftlist = new List<ShiftToDoList>();
                        if (request.ShiftToDoListItem != null && request.ShiftToDoListItem.Count > 0)
                        {
                            foreach (var item in request.ShiftToDoListItem)
                            {
                                if (item.IsInitials == false)
                                {

                                }
                                else
                                {
                                    ShiftToDoList StandardItem = new ShiftToDoList

                                    {

                                        EmployeeId = item.EmployeeId,
                                        //ClientId = item.ClientId,
                                        Description = item.Description,
                                        TodoItemId = todolist.Id,
                                        ShiftId = request.ShiftId,
                                        IsInitials = item.IsInitials,
                                        Initials = item.Initials,
                                        CreatedById = await _ISessionService.GetUserId(),
                                        CreatedDate = DateTime.Now,
                                        IsActive = true
                                    };
                                    Shiftlist.Add(StandardItem);
                                }

                            }
                            _context.ShiftToDoList.AddRange(Shiftlist);
                            _context.SaveChanges();
                            var ExistHistory = _context.ShiftHistoryInfo.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true && x.IsDeleted == false);
                            if (ExistHistory != null)
                            {
                                ExistHistory.ToDoItemId = todolist.Id;
                                _context.ShiftHistoryInfo.Update(ExistHistory);
                                _context.SaveChanges();
                            }
                        }
                        response.Success(Shiftlist);
                    }
                    else
                    {
                        response.AlreadyExist();
                    }



                    //if (request.ShiftToDoList != null && request.ShiftToDoList.Count > 0)
                    //{
                    //    var Existstandard = _context.EmployeeToDoListShift.Where(x => x.ShiftId == request.ShiftId && x.DateTime == request.DateTime && x.IsDeleted == false && x.IsActive == true).ToList();
                    //    if (Existstandard == null)
                    //    {
                    //    }
                    //    else
                    //    {
                    //        foreach (var item in Existstandard)
                    //        {
                    //            var StandardResult = _context.EmployeeToDoListShift.FirstOrDefault(x => x.Id == item.Id && x.IsDeleted == false && x.IsActive == true);
                    //            StandardResult.IsDeleted = true;
                    //            StandardResult.IsActive = false;
                    //            StandardResult.DeletedDate = DateTime.UtcNow;
                    //            StandardResult.DeletedById = 1;
                    //            _context.EmployeeToDoListShift.Update(StandardResult);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //    }
                    //    List<EmployeeToDoListShift> Shiftlist = new List<EmployeeToDoListShift>();

                    //    foreach (var item in request.ShiftToDoList)
                    //    {
                    //        if (item.IsInitials == false && item.IsReceived == false)
                    //        {

                    //        }
                    //        else
                    //        {
                    //            EmployeeToDoListShift StandardItem = new EmployeeToDoListShift

                    //            {

                    //                EmployeeId = item.EmployeeId,
                    //                DescriptionId = item.DescriptionId,
                    //                ShiftId = request.ShiftId,
                    //                DateTime = request.DateTime,
                    //                IsInitials = item.IsInitials,
                    //                Initials = item.Initials,
                    //                IsReceived = item.IsReceived,
                    //                Received = item.Received,
                    //                CreatedById = 1,
                    //                CreatedDate = DateTime.Now,
                    //                IsActive = true
                    //            };
                    //            Shiftlist.Add(StandardItem);
                    //        }

                    //    }
                    //    _context.EmployeeToDoListShift.AddRange(Shiftlist);
                    //    _context.SaveChanges();

                    //}
                    //response.Update(ExistUser);


                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);

                //response.Status = (int)Number.Zero;
                //response.Message = ResponseMessage.Error;

            }
            return response;

        }

        public async Task<ApiResponse> Handle(UpdateToDoListCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ToDoItemId > 0)
                {

                    var ExistUser = _context.ToDoShift.Where(x => x.Id == request.ToDoItemId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    if (ExistUser != null)
                    {
                        ExistUser.IsDeleted = true;
                        ExistUser.IsActive = false;
                        _context.SaveChanges();
                        ToDoShift todolist = new ToDoShift();
                        todolist.EmployeeId = ExistUser.EmployeeId;
                        todolist.ShiftId = ExistUser.ShiftId;
                        todolist.CreatedById = ExistUser.CreatedById;
                        todolist.CreatedDate = ExistUser.CreatedDate;
                        todolist.UpdateById = await _ISessionService.GetUserId();
                        todolist.UpdatedDate = DateTime.Now;
                        todolist.ShiftType = request.ShiftType;
                        todolist.IsActive = true;
                        todolist.DateTime = request.DateTime;
                        todolist.ShiftTime = TimeSpan.Parse(request.ShiftTime);
                        _context.ToDoShift.Add(todolist);
                        _context.SaveChanges();

                        List<ShiftToDoList> Shiftlist = new List<ShiftToDoList>();
                        if (request.ShiftToDoListItem != null && request.ShiftToDoListItem.Count > 0)
                        {
                            var shiftToDoList = (from todo in _context.ShiftToDoList where todo.TodoItemId == request.ToDoItemId select todo).ToList();
                            foreach (var item in shiftToDoList)
                            {
                                item.IsActive = false;
                                item.IsDeleted = true;
                            }
                            _context.SaveChanges();

                            foreach (var item in request.ShiftToDoListItem)
                            {
                                if (item.IsInitials == false)
                                {

                                }
                                else
                                {
                                    ShiftToDoList StandardItem = new ShiftToDoList
                                    {
                                        EmployeeId = item.EmployeeId,
                                        Description = item.Description,
                                        TodoItemId = todolist.Id,
                                        ShiftId = request.ShiftId,
                                        IsInitials = item.IsInitials,
                                        Initials = item.Initials,
                                        CreatedById = await _ISessionService.GetUserId(),
                                        CreatedDate = DateTime.Now,
                                        IsActive = true
                                    };
                                    Shiftlist.Add(StandardItem);
                                }

                            }
                            _context.ShiftToDoList.AddRange(Shiftlist);
                            _context.SaveChanges();
                            var ExistHistory = _context.ShiftHistoryInfo.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true && x.IsDeleted == false);
                            if (ExistHistory != null)
                            {
                                ExistHistory.ToDoItemId = todolist.Id;
                                _context.ShiftHistoryInfo.Update(ExistHistory);
                                _context.SaveChanges();
                            }
                        }
                        response.Update(Shiftlist);
                    }
                    else
                    {
                        response.AlreadyExist();
                    }



                    //if (request.ShiftToDoList != null && request.ShiftToDoList.Count > 0)
                    //{
                    //    var Existstandard = _context.EmployeeToDoListShift.Where(x => x.ShiftId == request.ShiftId && x.DateTime == request.DateTime && x.IsDeleted == false && x.IsActive == true).ToList();
                    //    if (Existstandard == null)
                    //    {
                    //    }
                    //    else
                    //    {
                    //        foreach (var item in Existstandard)
                    //        {
                    //            var StandardResult = _context.EmployeeToDoListShift.FirstOrDefault(x => x.Id == item.Id && x.IsDeleted == false && x.IsActive == true);
                    //            StandardResult.IsDeleted = true;
                    //            StandardResult.IsActive = false;
                    //            StandardResult.DeletedDate = DateTime.UtcNow;
                    //            StandardResult.DeletedById = 1;
                    //            _context.EmployeeToDoListShift.Update(StandardResult);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //    }
                    //    List<EmployeeToDoListShift> Shiftlist = new List<EmployeeToDoListShift>();

                    //    foreach (var item in request.ShiftToDoList)
                    //    {
                    //        if (item.IsInitials == false && item.IsReceived == false)
                    //        {

                    //        }
                    //        else
                    //        {
                    //            EmployeeToDoListShift StandardItem = new EmployeeToDoListShift

                    //            {

                    //                EmployeeId = item.EmployeeId,
                    //                DescriptionId = item.DescriptionId,
                    //                ShiftId = request.ShiftId,
                    //                DateTime = request.DateTime,
                    //                IsInitials = item.IsInitials,
                    //                Initials = item.Initials,
                    //                IsReceived = item.IsReceived,
                    //                Received = item.Received,
                    //                CreatedById = 1,
                    //                CreatedDate = DateTime.Now,
                    //                IsActive = true
                    //            };
                    //            Shiftlist.Add(StandardItem);
                    //        }

                    //    }
                    //    _context.EmployeeToDoListShift.AddRange(Shiftlist);
                    //    _context.SaveChanges();

                    //}
                    //response.Update(ExistUser);


                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);

                //response.Status = (int)Number.Zero;
                //response.Message = ResponseMessage.Error;

            }
            return response;
        }
    }
}
