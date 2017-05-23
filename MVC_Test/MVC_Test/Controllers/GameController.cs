using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Test.Models;

namespace MVC_Test.Controllers
{
    public class GameController : Controller
    {
        ISql sql = new Sql();
        Character character;
        // GET: Game
        public ActionResult Index()
        {
            character = (Character)TempData["Character"];
            sql.LoadStats(character);
            sql.LoadBag(character);
            TempData["Character"] = character;
            return View(character);
        }

        public ActionResult Battle()
        {
            Character character = (Character)TempData["Character"];
            Random rng = new Random();
            int minLvl = 1;
            if (character.Lvl > 2)
            {
                minLvl = character.Lvl - 2;
            }
            int lvl = rng.Next(minLvl, character.Lvl + 2);
            string type = Enum.GetName(typeof(Types), rng.Next(0, 3));
            EvilCreature enemy = new EvilCreature(lvl, type);
            TempData["Character"] = character;
            return View(enemy);
        }
    }
}