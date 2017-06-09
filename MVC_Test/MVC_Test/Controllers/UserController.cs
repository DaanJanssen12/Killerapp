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
            //check of de logindata correct is
            if (sql.Login(user.UserName, user.Password))
             {
                 FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                 user.Id = sql.GetUserId(user.UserName);
                 Session["User"] = user;
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
            //check of username en password niet leeg zijn
            if(user.UserName != null && user.Password != null)
            {
                //check of er al een gebruiker is met de ingevulde naam
                if(sql.UserExists(user.UserName) == true)
                {
                    ViewBag.Message = "Er bestaat al een gebruiker met die naam!";
                }
                //zo niet creeer een nieuwe user
                else
                {
                    sql.CreateUser(user);
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
            //uitloggen
            FormsAuthentication.SignOut();
            Session["User"] = null;
            Session["Character"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}