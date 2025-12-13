using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Milestone4.Services;
using Milestone4.Filters;
using Milestone4.Models;
using System.Security.Cryptography;

namespace Milestone4.Controllers
{
    public class UserController : Controller
    {
        static UserDAO userDAO = new UserDAO();
        private IPasswordHasher<UserModel> passwordHasher = new PasswordHasher<UserModel>();

        // Logging in
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View("Index");
        }

        public IActionResult ProcessLogin(string UserName, string password)
        {
            UserModel userData = userDAO.getUserByUserName(UserName);
            if (userData.Id == 0)
            {
                // User was not found
                return View("LoginFailure", userData);
            }

            if (CheckCredentials(userData, password))
            {
                // save the user data in the session
                string userJson = ServiceStack.Text.JsonSerializer.SerializeToString(userData);
                HttpContext.Session.SetString("User", userJson);
                return View("LoginSuccess", userData);
            }
            else
            {
                // password did not verify
                return View("LoginFailure", userData);
            }
        }

        private bool CheckCredentials(UserModel userData, string password)
        {
            return
                passwordHasher.VerifyHashedPassword(userData, userData.Password, password) == PasswordVerificationResult.Success;
        }

        // Registering

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        public IActionResult ProcessRegister(RegisterViewModel registerViewModel)
        {
            UserModel user = new UserModel();
            user.FirstName = registerViewModel.FirstName;
            user.LastName = registerViewModel.LastName;
            user.Sex = registerViewModel.Sex;
            user.StateOfResidence = 
                registerViewModel.States[registerViewModel.StateOfResidence - 1].StateName;
            user.Email = registerViewModel.Email;
            user.Username = registerViewModel.Username;
            user.Password = passwordHasher.HashPassword(user, registerViewModel.Password);
            userDAO.addUser(user);

            return View("Index");
        }

        public IActionResult Test()
        {
            return View("Index");
        }

        // Sessions

        [SessionCheckFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return View("Index");
        }
    }
}
