using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareMind.Core.Helpers;

namespace SoftwareMind.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NameController : ControllerBase
{
    private readonly IJwtAuthenticationManager _authenticationManager;
    private readonly ITokenRefresher _tokenRefresher;

    public NameController(IJwtAuthenticationManager authenticationManager, ITokenRefresher tokenRefresher)
    {
        _authenticationManager = authenticationManager;
        _tokenRefresher = tokenRefresher;
    }
    
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate(UserCred userCred)
    {
        var token = _authenticationManager.Authenticate(userCred.Username, userCred.Password);
            
        if (token == null) 
            return Unauthorized();
            
        return Ok(token);
    }
    
    [AllowAnonymous]
    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshCred refreshCred)
    {
        var token = _tokenRefresher.Refresh(refreshCred);

        if (token == null)
            return Unauthorized();

        return Ok(token);
    }
}