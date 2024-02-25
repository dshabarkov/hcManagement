using HumanCapitalManagement.Helpers;
using HumanCapitalManagement.Models;
using HumanCapitalManagement.Services.Interfaces;
using System.Reflection;

namespace HumanCapitalManagement.Services
{
    public class DataService : IDataService
    {
        #region Private fields & Constructor

        private readonly HttpClient client;

        public DataService(HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        #endregion Private fields & Constructor

        #region Public Methods

        public async Task<List<PersonModel>> GetPeopleList()
        {
            var response = await client.GetAsync("api/People/GetPeople");

            return await response.ReadContentAsync<List<PersonModel>>();
        }
        
        public async Task<PersonModel> GetPersonData(long personId)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, $"api/People/GetPersonData?jsonRequest={personId}");

            var response = await client.SendAsync(message);

            return await response.ReadContentAsync<PersonModel>();
        }
        
        public async Task<bool> SavePersonData(PersonModel model)
        {
            var response = await client.PostAsJsonAsync("api/People/SavePersonData", model);

            return await response.ReadContentAsync<bool>();
        }

        public async Task<HttpResponseMessage?> DeletePerson(long personId, string token)
        {
            var deleteModel = new DeleteModel
            {
                PersonId = personId,
                Token = token
            };

            var response = await client.PostAsJsonAsync("api/People/DeletePerson", new { personId, token});

            return response;
        }

        #endregion Public Methods
    }
}
