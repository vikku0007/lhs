using System;
using System.Collections.Generic;
using System.Text;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using LHSAPI.Common.ApiResponse;
using MediatR;
using System.Linq;
using LHSAPI.Application.Interface;
using System.IO;

namespace LHSAPI.Application.Services
{
   public class SessionService :ISessionService
    {
        private readonly LHSDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public SessionService(LHSDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        public async Task<int> GetUserId()
        {
            ApiResponse response = new ApiResponse();
            var EmployeeId = 0;
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                ApplicationUser user1 = await _userManager.FindByNameAsync(userId);
                EmployeeId = user1.EmployeeId;
                return EmployeeId;
            }
            else
            {
                return EmployeeId = 1;
            }

        }
     

    }
}
