using CompanyApi.Dtos;
using CompanyServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpPost("register")]
        public IActionResult RegisterCompany([FromForm] CompanyRegisterDTO registerDTO)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    authService.RegisterCompany(registerDTO);
                    return Ok(new { message = "Company registered successfully." });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
