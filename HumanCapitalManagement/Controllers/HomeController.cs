using HumanCapitalManagement.Helpers;
using HumanCapitalManagement.Models;
using HumanCapitalManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace HumanCapitalManagement.Controllers
{
    public class HomeController : Controller
    {
        #region Private fields & Constructor

        private readonly ILogger<HomeController> _logger;
        private readonly IDataService dataService;
        private readonly ILoginService loginService;

        public HomeController(ILogger<HomeController> logger, IDataService dataService, ILoginService loginService)
        {
            _logger = logger;
            this.dataService = dataService;
            this.loginService = loginService;
        }

        #endregion Private fields & Constructor

        #region Action Results

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (token == null)
            {
                return BadRequest(new { Message = "Token NOT found!" });
            }

            var authResponse = await loginService.Authenticate(token);

            if (authResponse == null)
            {
                return BadRequest("Error authenticating user!");
            }
            else if (authResponse.StatusCode != HttpStatusCode.OK)
            {
                return this.StatusCode((int)authResponse.StatusCode);
            }

            var peopleModel = await dataService.GetPeopleList();

            return View(peopleModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddEditPersonModal(long personId)
        {
            var personModel = new PersonModel();

            if (personId > 0)
            {
                personModel = await dataService.GetPersonData(personId);
            }

            return PartialView("_AddEditPersonModalPartial", personModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditPersonData(PersonModel model)
        {
            if (model != null)
            {
                var result = await dataService.SavePersonData(model);

                if (result) 
                {
                    return Json(new { Message = "Person successfully added/updated." });
                }

                return BadRequest(new { Message = "Person add/update failed!" });
            }

            return BadRequest(new { Message = "User input cannot be empty!"});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelectedPerson(long personId)
        {
            if (personId > 0)
            {
                var token = HttpContext.Session.GetString("JWToken");

                if (token == null)
                {
                    return BadRequest(new { Message = "Token NOT found!" });
                }

                var result = await dataService.DeletePerson(personId, token);

                if (result == null)
                {
                    return BadRequest(new { Message = "API Response error!" });
                }
                else if (result.StatusCode == HttpStatusCode.Forbidden)
                {
                    return BadRequest(new { Message = "User is not authorized for that action!" });
                }
                else if (result.StatusCode != HttpStatusCode.OK)
                {
                    return BadRequest(new { Message = $"API responded with status {(int)result.StatusCode} {result.StatusCode}" });
                }

                return Json(new { Message = "Successfully deleted person." });
            }

            return BadRequest(new { Message = "Incorrect person ID!" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion Action Results
    }
}