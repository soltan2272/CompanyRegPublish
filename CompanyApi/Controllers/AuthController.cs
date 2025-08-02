using CompanyApi.Dtos;
using CompanyServiceLayer.Dtos;
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

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto verifyOtp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isVerified = authService.VerifyOtp(verifyOtp);
                    if (isVerified)
                    {
                        return Ok(new { message = "OTP verified successfully." });
                    }
                    else
                    {
                        return BadRequest(new { message = "Invalid OTP." });
                    }
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

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPasswordAsync([FromBody] SetPasswordDto setPasswordDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isSet = await authService.SetPasswordAsync(setPasswordDto);
                    if (isSet)
                    {
                        return Ok(new { message = "Password set successfully." });
                    }
                    else
                    {
                        return BadRequest(new { message = "Failed to set password." });
                    }
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

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            if (ModelState.IsValid)
            {
                    var result = await authService.LoginAsync(dto);
                    if (result != null)
                    {
                        return Ok(new { message = result});
                    }
                    else
                    {
                        return Unauthorized(new { message = "Invalid email or password." });
                    }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
