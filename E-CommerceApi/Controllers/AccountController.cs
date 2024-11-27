using E_CommerceApi.HandlingErrors;
using E_CommerceDomain.DTOs.Account_Module;
using E_CommerceDomain.Interfaces.Account_Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService AccountService;

        public AccountController(IAccountService AccountService)
        {
            this.AccountService = AccountService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDTO User)
        {
            var Result = await AccountService.Register(User);

            if(Result.Count() == 0)
            {
                return Created();
            }

            var Response = new ApiValidationErrorResponse((int)HttpStatusCode.BadRequest, Result, "Register Faild");

            return BadRequest(Response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO User)
        {
            var UserInfo = await AccountService.Login(User);

            if(UserInfo is not null)
            {
                return Ok(UserInfo);
            }

            return BadRequest(new ApiErrorResponse((int)HttpStatusCode.BadRequest, "Invalid Email Or Password"));
        }

        [HttpGet]
        [Authorize]
        public async Task<UserInfoDTO> GetCurrentUserWithAddress()
        {
            var CurrentUserEmail = User.FindFirstValue(ClaimTypes.Email);

            if(CurrentUserEmail is null)
            {
                return null;
            }

            var UserInfo = await AccountService.GetCurrentUser(CurrentUserEmail);

            if(UserInfo is null)
            {
                return null;
            }

            return UserInfo;
        }
    }
}
