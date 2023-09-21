using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;
using Org.BouncyCastle.Crypto.Digests;
using System.Text;
using System.Security.Cryptography;

namespace SummerCamp.Controllers
{
    public class AccountController : Controller

    {
        private readonly IMapper _mapper;
        private readonly IUserCredentialRepository _userCredentialRepository;

        public AccountController(IMapper mapper, IUserCredentialRepository userCredentialRepository)
        {
            _mapper = mapper;
            _userCredentialRepository = userCredentialRepository;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(UserCredentialViewModel userCredentialViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userCredentialRepository.Get(uC => uC.Username == userCredentialViewModel.Username && uC.PasswordHash == HashPassword(userCredentialViewModel.PasswordHash));

                if (user.Count() != 0)
                {
                    HttpContext.Session.SetString("Username", userCredentialViewModel.Username);
                    return RedirectToAction("Welcome");
                }
                else
                {
                    ViewBag.ErrorMessage = "Nume de utilizator sau parola incorecte!";
                    return View();
                }
            }
            return View(userCredentialViewModel);
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserCredentialViewModel userCredentialViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userCredentialRepository.Get(uC => uC.Username == userCredentialViewModel.Username);
                // Check if the username is already taken
                if (existingUser.Count() != 0)
                {
                    ViewBag.ErrorMessage = "Username is already taken";
                    return View(userCredentialViewModel);
                }

                userCredentialViewModel.PasswordHash = HashPassword(userCredentialViewModel.PasswordHash);
                _userCredentialRepository.Add(_mapper.Map<UserCredential>(userCredentialViewModel));
                _userCredentialRepository.Save();
                return RedirectToAction("Welcome");
            }
            return View(userCredentialViewModel);
        }

        public static string HashPassword(string input)
        {

            if (input == null)
            {
                return null;
            }
            // Convert the input string to a byte array
            byte[] data = Encoding.UTF8.GetBytes(input);

            // Compute the hash value of the input byte array
            byte[] hashBytes = SHA256.HashData(data);

            // Convert the byte array to a hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }

            String result = sb.ToString();
            return result ;
        }
    }
}