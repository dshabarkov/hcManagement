using DataAPI.Models;

namespace DataAPI.Services.Interfaces
{
    public interface IAuthenticateService
    {
        Task<HttpResponseMessage?> AuthAdmin(string? token);
    }
}
