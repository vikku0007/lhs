using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Common.ApiResponse
{
    public class ResponseMessage
    {
        public const string Error = "Some internal error occured";
        public const string UserExist = "Username already exist";
        public const string Success = "Record saved successfully";
        public const string PhoneExist = "Mobile number already exist";
        public const string InvalidXMLRSAkey = "Invalid XML RSA key";
        public const string InvalidKeySize = "Key size is not valid";
        public const string KeyIsNullOrEmpty = "Key is null or empty";
        public const string RSAKeyValue = "RSAKeyValue";
        public const string Modulus = "Modulus";
        public const string Exponent = "Exponent";
        public const string P = "P";
        public const string Q = "Q";
        public const string DP = "DP";
        public const string DQ = "DQ";
        public const string InverseQ = "InverseQ";
        public const string D = "D";
        public const string Expire = "Token has expired";
        public const string UserNotExist = "User does not exists";
        public const string InvalidPassword = "The User/Email and password provided is incorret";
        public const string NOTFOUND = "No Data Found";
        public const string Exist = "Already exist";
        public const string ValidationError = "Required params not supplied";
    public const string EmployeeIdNull = "Employee id null or Empty";
        public const string UpdateSuccess = "Record updated successfully";
        public const string DeleteMsg = "Record deleted successfully";
        public const string RecordNotExist = "Record deleted successfully";
    }
  #region HTTP Codes
  public static class HTTPStatusCode
  {
    public const int SUCCESSSTATUSCODE = 200;
    public const int NO_DATA_FOUND = 204;
    public const int ERRORSTATUSCODE = 203;
    public const int REDIRECTIONSTATUSCODE = 302;
    public const int INTERNAL_SERVER_ERROR = 500;
    public const int BADREQUEST = 400;
    public const int NOTFOUND = 404;
    public const int NOTAUTHORIZED = 401;
  }
  #endregion
}
