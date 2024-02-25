using DataAPI.Models;
using DataAPI.Services.Interfaces;
using System.Net.Http.Headers;

namespace DataAPI.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        #region Private fields & Constructor

        private readonly HttpClient client;

        public AuthenticateService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        #endregion Private fields & Constructor

        #region Public Methods

        public async Task<HttpResponseMessage?> AuthAdmin(string? token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/Authenticate/AuthenticateAdmin");

            return response;
        }

        #endregion Public Methods

    }
}
