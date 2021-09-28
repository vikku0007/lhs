
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.Account.Queries.LogOut
{
  public class LogoutHandler : IRequestHandler<Logout, ApiResponse>
  {
    private readonly LHSDbContext _context;
    private readonly ISessionService _ISessionService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public LogoutHandler(LHSDbContext dbContext, ISessionService ISessionService, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager)
    {

      _context = dbContext;
      _ISessionService = ISessionService;
      _userManager = userManager;
      _httpContextAccessor = httpContextAccessor;
      _signInManager = signInManager;
    }
    #region LogOut
    /// <summary>
    /// Logging Out User And Setting The Active Status To False
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ApiResponse> Handle(Logout request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
      try
      {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
          ApplicationUser user1 = await _userManager.FindByNameAsync(userId);

          var activateUser = _context.User.Where(x => x.UserById == user1.Id).FirstOrDefault();
          activateUser.Token = null;
          _context.Update(activateUser);
          await _context.SaveChangesAsync();
          if (_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
          {
            await _signInManager.SignOutAsync();
            response.Success();
          }
          else
          {
            response.Failed("User not sign In");
          }

        }
        else
        {
          response.NotFound();
        }

      }
      catch (Exception ex)
      {
        response.Failed(ex.Message);
      }
      return response;
    }
    #endregion
  }
}
