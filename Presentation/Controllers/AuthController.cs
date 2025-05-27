using Business.Dtos;
using Business.Models;
using Business.Services;
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

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm(VerifiedDtoRequest request)
    {
        var result = await _authService.VerifyCodeAndConfirmEmail(request);

        return result.Success ? Ok(result) : BadRequest();
    }

}
