namespace Laboratorio.Api.Models;

public class UserLoginDto
{
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
}

public class UserLoginResponseDto
{
    public string Token { get; set; } = "";
}