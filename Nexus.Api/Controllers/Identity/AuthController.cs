using Microsoft.AspNetCore.Mvc;
using Nexus.Core.DTOs.Identity;
using Nexus.Core.Interfaces.Identity;

namespace Nexus.Api.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IIdentityService _identityService) : ControllerBase
{
    // POST: api/auth/register]
    /// <summary>
    /// register new user
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var result = await _identityService.RegisterAsync(request);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    // POST: api/auth/login
    /// <summary>
    /// Login endpoint
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var result = await _identityService.LoginAsync(request);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }
    
}