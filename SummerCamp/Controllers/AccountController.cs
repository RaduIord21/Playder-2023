using Microsoft.AspNetCore.Mvc;
using System;

namespace SummerCamp.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Perform authentication here (e.g., check credentials against a database)
            // For simplicity, we'll use a hardcoded username and password for demonstration purposes
            if (username == "Radu" && password == "12")
            {
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: /Account/Welcome
        public IActionResult Welcome()
        {
            // Check if the user is logged in (session exists)
            var username = HttpContext.Session.GetString("Username");
            if (!string.IsNullOrEmpty(username))
            {
                ViewBag.Username = username;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}