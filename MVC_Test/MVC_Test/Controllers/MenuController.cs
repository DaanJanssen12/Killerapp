using MVC_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Test.Controllers
{    
    public class MenuController : Controller
    {
        ISql sql = new Sql();
        // GET: Menu
        public ActionResult Index()
        {
            if(Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            User user = (User)Session["User"];
            user.loadCharacters(sql);
            Session["User"] = user;
            if (user.characters.Count > 0)
            {
                ViewBag.C1 = user.characters[0].ToString();
            }
            else
            {
                ViewBag.C1 = "New character";
            }

            if (user.characters.Count > 1)
            {
                ViewBag.C2 = user.characters[1].ToString();
            }
            else
            {
                ViewBag.C2 = "New character";
            }

            if (user.characters.Count > 2)
            {
                ViewBag.C3 = user.characters[2].ToString(); ;
            }
            else
            {
                ViewBag.C3 = "New character";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string submit)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            User user = (User)Session["User"];

            if (submit == "New character")
            {
                return RedirectToAction("NewCharacter", "Menu");
            }
            else
            {
                foreach (Character c in user.characters)
                {
                    if(submit == c.ToString())
                    {
                        Session["Character"] = c;
                        return RedirectToAction("Index", "Game");
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult NewCharacter()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            List<SelectListItem> items = new List<SelectListItem>();

            foreach(string c in Enum.GetNames(typeof(Classes)))
            {
                items.Add(new SelectListItem { Text = c, Value = c });
            }

            ViewBag.Class = items;

            return View();
        }

        [HttpPost]
        public ActionResult NewCharacter(Models.Character character)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            User user = (User)Session["User"];
            sql.CreateCharacter(user, character);
            Session["User"] = user;
            return RedirectToAction("Index", "Menu");
        }
    }
}