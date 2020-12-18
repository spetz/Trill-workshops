using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trill.Infrastructure.Auth;

namespace Trill.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class AccountController : ControllerBase
    {
        private readonly IJwtHandler _jwtHandler;

        public AccountController(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }
        
        [HttpPost("sign-in")]
        public ActionResult<JsonWebToken> SignIn()
        {
            var userId = Guid.NewGuid().ToString("N");
            var jwt = _jwtHandler.CreateToken(userId);

            return jwt;
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<string> Me()
        {
            return User.Identity.Name;
        }
    }
}