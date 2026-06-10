using ConectaEleitor.Application.DTOs.AuthDTOs;
using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConectaEleitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-assessor")]
        [Authorize(Policy = "AssemblymanFunctions")]
        public async Task<IActionResult> RegisterAssessor([FromBody] RegisterData registerData)
        {
            return Ok(await  _authService.RegisterAssessor(registerData));
        }
        
        [HttpPost("register-assemblyman")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> RegisterAssemblyman([FromBody] RegisterData registerData)
        {
            return Ok(await  _authService.RegisterAssemblyman(registerData));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginData loginData)
        {
            return Ok(await _authService.Login(loginData));
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            return Ok(await _authService.GetMe());
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok("Logout feito com sucesso.");
        }
    }
}
