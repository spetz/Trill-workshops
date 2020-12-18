using System;
using System.Collections.Generic;
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
            var role = "admin";
            var claims = new Dictionary<string, IEnumerable<string>>
            {
                ["permissions"] = new[] {"secret:read", "secret:delete", "secret:create"}
            };
            var jwt = _jwtHandler.CreateToken(userId, role, claims: claims);

            return jwt;
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<string> Me()
        {
            return User.Identity.Name;
        }

        // [Authorize(Roles = "admin")]
        [Authorize(Policy = "read-secret")]
        [HttpGet("secret")]
        public ActionResult<string> Secret()
        {
            return "secret value";
        }
    }
}