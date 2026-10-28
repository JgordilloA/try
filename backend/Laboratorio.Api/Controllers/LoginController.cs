using Laboratorio.Api.Interfaces;
using Laboratorio.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Laboratorio.Api.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController(ILoginService loginService) : ControllerBase
{
    // POST /api/login
    [HttpPost]
    public ActionResult<UserLoginResponseDto> Login([FromBody] UserLoginDto userLogin)
    {
        var result = loginService.Login(userLogin);
        return result is null
            ? Unauthorized(new { message = "Usuario o contraseña incorrectos." })
            : Ok(result);
    }
}