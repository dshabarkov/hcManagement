using HumanCapitalManagement.Models;

namespace HumanCapitalManagement.Services.Interfaces
{
    public interface IDataService
    {
        Task<HttpResponseMessage?> DeletePerson(long personId, string token);
        Task<List<PersonModel>> GetPeopleList();
        Task<PersonModel> GetPersonData(long personId);
        Task<bool> SavePersonData(PersonModel model);
    }
}
