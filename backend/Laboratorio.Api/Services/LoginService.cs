using Laboratorio.Api.Interfaces;
using Laboratorio.Api.Models;

namespace Laboratorio.Api.Services;

public class LoginService : ILoginService
{
    // Autenticación simulada: solo existe el usuario "demo".
    public UserLoginResponseDto? Login(UserLoginDto loginUser)
    {
        if (loginUser.User != "demo" || loginUser.Password != "1234")
            return null;

        return new UserLoginResponseDto { Token = Guid.NewGuid().ToString("N") };
    }
}