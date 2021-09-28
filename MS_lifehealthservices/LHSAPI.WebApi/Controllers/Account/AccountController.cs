using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using LHSAPI.Application.Account.Commands.Create.SignUp;
using LHSAPI.Application.Account.Queries.Login;
using LHSAPI.Application.ValidateToken;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using LHSAPI.Application.Account.Queries.LogOut;

namespace LHSAPI.WebApi.Controllers.Account
{


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LHSDbContext _dbContext;
        private readonly JsonSerializerSettings _serializerSettings;

        public AccountController(UserManager<ApplicationUser> userManager, LHSDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        [Authorize]
        private async Task<dynamic> CheckTokenQueryAsync()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            dynamic res = await Mediator.Send(new ValidateTokenQuery()
            {
                UserId = user.Id
            });
            if (res.StatusCode == 200)
                return user.Id;
            else return res;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignupCommand model)
        {
            return Ok(await Mediator.Send(model));
        }

        //  [Authorize(Policy = "ApiKeyPolicy")]
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        [Route("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromBody] EmailConfirmation model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok(await Mediator.Send(new Logout()));
        }

    }
}
