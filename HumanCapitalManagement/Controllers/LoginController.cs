using HumanCapitalManagement.Models;
using HumanCapitalManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HumanCapitalManagement.Controllers
{
    public class LoginController : Controller
    {
        #region Private fields & Constructor

        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService loginService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            this.loginService = loginService;
        }

        #endregion Private fields & Constructor

        #region Action Results

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation($"At start of Login action: {DateTimeOffset.UtcNow}");

            var loginModel = new LoginModel();

            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginModel model)
        {
            if (model != null)
            {
                var result = await loginService.Login(model);

                if (!string.IsNullOrEmpty(result.Token))
                {
                    HttpContext.Session.SetString("JWToken", result.Token);

                    return Json(new { Message = "Login successful." });
                }

                return BadRequest(new { Message = "Login failed!" });
            }

            return BadRequest(new { Message = "User input cannot be empty!" });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //Response.Headers.Remove("Authorization");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var registerModel = new RegisterModel();

            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            if (model != null)
            {
                var registerResponse = await loginService.Register(model);

                if (registerResponse == null)
                {
                    return BadRequest("Error registering user!");
                }
                else if (registerResponse.StatusCode != HttpStatusCode.OK)
                {
                    return BadRequest(new { Message = $"API responded with status {(int)registerResponse.StatusCode} {registerResponse.StatusCode}" });
                }

                return Json(new { Message = "Login successful." });
            }

            return BadRequest(new { Message = "User input cannot be empty!" });
        }

        #endregion Action Results
    }
}