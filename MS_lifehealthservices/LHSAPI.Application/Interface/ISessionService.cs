using LHSAPI.Application.Shift.Models;
using LHSAPI.Common.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LHSAPI.Application.Interface
{
  public interface ISessionService
    {
     Task<int> GetUserId();
     
    }
}
