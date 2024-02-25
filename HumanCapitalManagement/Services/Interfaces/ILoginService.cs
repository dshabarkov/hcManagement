using HumanCapitalManagement.Models;

namespace HumanCapitalManagement.Services.Interfaces
{
    public interface ILoginService
    {
        Task<HttpResponseMessage?> Authenticate(string? token);
        Task<TokenModel> Login(LoginModel model);
        Task<HttpResponseMessage?> Register(RegisterModel model);
    }
}
