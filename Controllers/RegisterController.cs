using InnoGotchi_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InnoGotchi_frontend.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPost(User newUser)
        {
            var User = newUser;
            return RedirectToAction("Index");
        }
    }
}
