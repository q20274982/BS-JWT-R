using BS_JWT_R.Helpers;
using BS_JWT_R.Models.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BS_JWT_R.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtHelper _jwt;

        public TokenController(JwtHelper jwt)
        {
            _jwt = jwt;
        }

        [HttpPost("signin")]
        public IActionResult SignIn(LoginVM query)
        {
            if (ValidateUser(query))
            {
                return Ok(_jwt.GenerateToken(query.username));
            }

            return BadRequest();
        }

        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(x => new { x.Type, x.Value }));
        }

        [HttpGet("username")]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }

        private bool ValidateUser(LoginVM query)
        {
            return true;
        }
    }
}
