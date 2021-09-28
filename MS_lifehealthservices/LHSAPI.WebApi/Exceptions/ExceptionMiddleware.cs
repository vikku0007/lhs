
using Microsoft.AspNetCore.Http;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LHSDbContext _context;

        public ExceptionMiddleware(RequestDelegate next, LHSDbContext context)
        {
            _next = next;
            _context = context;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
                error.UserId = httpContext.User.Identity.Name;
                error.StatusCode = httpContext.Response.StatusCode;
                await _context.AddAsync(error);
                await _context.SaveChangesAsync();
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ApiResponse()
            {
                Status = (int)Number.Zero,
                Message = ResponseMessage.Error,
                StatusCode = context.Response.StatusCode
            }.ToString());
        }
    }
}
