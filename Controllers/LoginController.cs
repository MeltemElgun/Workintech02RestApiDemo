
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workintech02RestApiDemo.Business.Authentication;
using Workintech02RestApiDemo.Domain.Entities;

namespace Workintech02RestApiDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var result = _authenticationService.Login(username, password);
            if (result!=null)
            {
                var token = _authenticationService.GenerateToken(result);
                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpPost]

        public IActionResult Register(User user)
        {
            var result = _authenticationService.Register(user);
            if (result.Id>0)
            {
                var token = _authenticationService.GenerateToken(result);
                return Ok(token);
            }
            return Unauthorized();
        }

    }
}
