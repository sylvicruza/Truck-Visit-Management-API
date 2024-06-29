using Microsoft.AspNetCore.Mvc;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Security;
using Truck_Visit_Management.Services.ServiceImpl;

namespace Truck_Visit_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto login)
        {
            var token = _authService.Authenticate(login);
            if (token == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(new { token });
        }

        [Authorize("Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            try
            {
                await _authService.Register(model);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
