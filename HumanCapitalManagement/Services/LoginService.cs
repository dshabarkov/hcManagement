using HumanCapitalManagement.Helpers;
using HumanCapitalManagement.Models;
using HumanCapitalManagement.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace HumanCapitalManagement.Services
{
    public class LoginService : ILoginService
    {
        #region Private fields & Constructor

        private readonly HttpClient client;

        public LoginService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        #endregion Private fields & Constructor

        #region Public Methods

        public async Task<TokenModel> Login(LoginModel model)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var response = await client.PostAsJsonAsync("api/Authenticate/Login", model);

                return await response.ReadContentAsync<TokenModel>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<HttpResponseMessage?> Register(RegisterModel model)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var response = new HttpResponseMessage();

            if (model.IsAdmin)
            {
                response = await client.PostAsJsonAsync("api/Authenticate/RegisterAdmin", model);
            }
            else
            {
                response = await client.PostAsJsonAsync("api/Authenticate/Register", model);
            }

            return response;
        }

        public async Task<HttpResponseMessage?> Authenticate(string? token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/Authenticate/AuthenticateIsUserLogged");

            return response;
        }

        #endregion Public Methods
    }
}