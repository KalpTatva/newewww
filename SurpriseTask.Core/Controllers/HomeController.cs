using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SurpriseTask.Core.Models;
using SurpriseTask.Repository.Models;
using SurpriseTask.Repository.ViewModels;
using SurpriseTask.Service.Interfaces;

namespace SurpriseTask.Core.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    // Method for setting up cookie
    private void SetJwtCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(30), // Session cookie if not persistent
        };
        Response.Cookies.Append("auth_token", token, cookieOptions);
    }

    // login index page 
    // redirects to dashboard if cookie is available
    public async Task<IActionResult> Index()
    {
       if (
            HttpContext.Request.Cookies["auth_token"] != null
            && HttpContext.Items["UserRole"] != null
        )
        {
            string? role = HttpContext.Items["UserRole"] as string;
            if (role == "Admin")
            {
                TempData["success"] = "logged in";
                return RedirectToAction("Index","Dashboard");
            }else if(role == "User"){
                TempData["success"] = "logged in";
                return RedirectToAction("UserDashBoard", "Dashboard");
            }
        }
        return View();
    }

    // post method for login 
    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        string res = "";
        try{
            if(!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid user credentials";
                return View(model);
            }

            ResponseTokenViewModel? response = await _userService.UserLogin(model);
            if (response != null && response.token.Length > 0)
            {
                SetJwtCookie(response.token);
                TempData["success"] = "User logged in successfully!";
                if(response.Role == "User")
                {
                    return RedirectToAction("UserDashBoard","Dashboard");

                }
                return RedirectToAction("Index", "Dashboard");
               
            }
            res = response != null ? response.response :  "";
            TempData["Error"] = res;
            return View(model);
        }
        catch
        {  
            TempData["Error"] = res;
            return View(model);
        }
    }


    // Logout 
    public IActionResult Logout()
    {
        Response.Cookies.Delete("auth_token");
        TempData["logout"] = "Logout Successful!";
        return RedirectToAction("Index", "Home");
    }


    // error 404 view
    public IActionResult Error404()
    {
        return View();
    }

    // error 403 view
    public IActionResult Error403()
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
