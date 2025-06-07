using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var result = await _authService.SignUp(dto);

        return result.Success ? Ok(result) : BadRequest();
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SignInDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);

        }

        var result = await _authService.SignIn(dto);

        return result.Success ? Ok(result) : BadRequest();

    }

    [HttpPost("signout")]
    public async Task<IActionResult> SignOutUser()
    {
        var result = await _authService.SignOut();

        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm(VerifiedDtoRequest request)
    {
        var result = await _authService.VerifyCodeAndConfirmEmail(request);

        return result.Success ? Ok(result) : BadRequest();
    }

}
