using System;
using System.Collections.Generic;
using System.Text;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Common.ApiResponse
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Status = 0;
            Message = String.Empty;
            StatusCode = ResponseCode.BadRequest;
        }
        public string Message { get; set; }
        public object ResponseData { get; set; }
        public int Status { get; set; }
        public int StatusCode { get; set; }
        public int Total { get; set; }
       
        public ApiResponse Success(object ResponseData = null)
        {
            this.StatusCode = ResponseCode.Ok;
            this.Status = (int)Number.One;
            this.Message = ResponseMessage.Success;
            this.ResponseData = ResponseData;
            return this;
        }
        public void SuccessWithOutMessage(object ResponseData = null)
        {
            this.StatusCode = ResponseCode.Ok;
            Message = string.Empty;
            this.Status = (int)Number.One;
            this.ResponseData = ResponseData;
        }
        public void Update(object ResponseData = null)
        {
            this.StatusCode = ResponseCode.Ok;
            this.Status = (int)Number.One;
            this.Message = ResponseMessage.UpdateSuccess;
            this.ResponseData = ResponseData;
        }
        public void Failed(string message)
        {
            this.StatusCode = ResponseCode.ServerError;
            this.Status = (int)Number.Zero;
            this.Message = message;
        }
        public ApiResponse NotFound()
        {
            this.StatusCode = ResponseCode.NotFound;
            this.Status = (int)Number.Zero;
            this.Message = ResponseMessage.NOTFOUND;
            return this;
        }
        public void AlreadyExist()
        {
            this.StatusCode = ResponseCode.Ok;
            this.Status = (int)Number.Zero;
            this.Message = ResponseMessage.Exist;
        }
        public void ValidationError()
        {
            this.StatusCode = ResponseCode.BadRequest;
            this.Status = (int)Number.Zero;
            this.Message = ResponseMessage.ValidationError;
        }
        public void Delete(object ResponseData = null)
        {
            this.StatusCode = ResponseCode.Ok;
            this.Status = (int)Number.One;
            this.Message = ResponseMessage.DeleteMsg;
            this.ResponseData = ResponseData;
        }
    }
}
