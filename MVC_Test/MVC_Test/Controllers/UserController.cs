using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVC_Test.Models;

namespace MVC_Test.Controllers
{
    public class UserController : Controller
    {
        ISql sql = new Sql();
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (sql.Login(user.UserName, user.Password))
             {
                 FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                 user.Id = sql.GetUserId(user.UserName);
                 TempData["User"] = user;
                 return RedirectToAction("Index", "Menu");
             }
             else
             {
                 ModelState.AddModelError("", "Login data is incorrect!");
             }
         return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if(user.UserName != null && user.Password != null)
            {
                if(sql.UserExists(user.UserName) == true)
                {
                    ViewBag.Message = "Er bestaat al een gebruiker met die naam!";
                }
                else
                {
                    ViewBag.Message = "Account succesvol aangemaakt.";
                }
            }
            else
            {
                ViewBag.Message = "Vul a.u.b. alle velden in!";
            }
            
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}