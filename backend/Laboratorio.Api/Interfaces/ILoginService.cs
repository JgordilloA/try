using Laboratorio.Api.Models;

namespace Laboratorio.Api.Interfaces;

public interface ILoginService
{
    // Devuelve null si las credenciales son incorrectas.
    UserLoginResponseDto? Login(UserLoginDto loginUser);
}