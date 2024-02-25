using DataAPI.Models;
using DataAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace DataAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        #region Private fields & Constructor

        private readonly HcmDataContext context;
        private readonly IAuthenticateService authenticateService;

        public PeopleController(HcmDataContext context, IAuthenticateService authenticateService)
        {
            this.context = context;
            this.authenticateService = authenticateService;
        }

        #endregion Private fields & Constructor

        #region Action Results

        [HttpGet]
        [Route("GetPeople")]
        public async Task<IActionResult> GetPeople()
        {
            return Ok(await context.People.ToListAsync());
        }

        [HttpPost]
        [Route("GetPersonData")]
        public async Task<IActionResult> GetPersonData(string jsonRequest)
        {
            var personId = Convert.ToInt64(JsonConvert.DeserializeObject(jsonRequest));

            return Ok(await context.People.Where(x => x.Id == personId).FirstOrDefaultAsync());
        }

        [HttpPost]
        [Route("SavePersonData")]
        public async Task<IActionResult> SavePersonData(Person model)
        {
            var person = await context.People.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (person == null)
            {
                var newPerson = new Person()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Salary = model.Salary,
                    Department = model.Department
                };

                await context.AddAsync(newPerson);
            }
            else
            {
                person.FirstName = model.FirstName;
                person.LastName = model.LastName;
                person.Salary = model.Salary;
                person.Department = model.Department;
            }

            if (await context.SaveChangesAsync() < 0)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }

        [HttpPost]
        [Route("DeletePerson")]
        public async Task<IActionResult> DeletePerson(DeletePersonModel deletePersonModel)
        {
            var response = await authenticateService.AuthAdmin(deletePersonModel.Token);

            if (response == null)
            {
                return BadRequest("Error authenticating user!");
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                return this.StatusCode((int)response.StatusCode);
            }

            var person = await context.People.Where(x => x.Id == deletePersonModel.PersonId).FirstOrDefaultAsync();

            if (person != null)
            {
                context.Remove(person);

                if (await context.SaveChangesAsync() < 0)
                {
                    return BadRequest("Error deleting person!");
                }

                return Ok("Successfully deleted person.");
            }

            return BadRequest("Person ID NOT found!");
        }

        #endregion Action Results
    }
}