using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E8ay.Common.ViewModels;
using E8ay.User.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace E8ay.User.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("")]
        public string Index()
        {
            return "Welcome!";
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginViewModel model)
        {
            var jwt = await _authService.SignInAndGetToken(model.Username, model.Password);

            if (jwt == null)
                return Unauthorized();

            return Ok(JsonConvert.DeserializeObject(jwt));
        }

    }
}