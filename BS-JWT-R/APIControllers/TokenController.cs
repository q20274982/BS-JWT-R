using BS_JWT_R.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult SignIn()
        {
            
        }

        public IActionResult GetClaims()
        {

        }

        public IActionResult GetUserName()
        {

        }

    }
}
