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
        Battle battle;
        // GET: Game
        public ActionResult Index()
        {
            if (Session["Character"] == null)
            {
                return RedirectToAction("Index", "Menu");
            }
            character = (Character)Session["Character"];
            sql.LoadStats(character);
            sql.LoadBag(character);
            int count = character.bag.Count;
            sql.LoadMoves(character);
            Session["Character"] = character;
            return View(character);
        }

        [HttpGet]
        public ActionResult Battle()
        {
            if(Session["Character"] == null)
            {
                return RedirectToAction("Index", "Menu");
            }
            character = (Character)Session["Character"];
            Random rng = new Random();
            int minLvl = 1;
            if (character.Lvl > 2)
            {
                minLvl = character.Lvl - 2;
            }
            int lvl = rng.Next(minLvl, character.Lvl + 2);
            string type = Enum.GetName(typeof(Types), rng.Next(0, 3));
            EvilCreature enemy = new EvilCreature(lvl, type);
            sql.LoadMoves(enemy);
            battle = new Battle(character, enemy);
            TempData["Battle"] = battle;
            return View(battle);
        }

        [HttpPost]
        public ActionResult Battle(string submit)
        {
            battle = (Battle)TempData["Battle"];
            if (submit == "Run")
            {
                TempData["Character"] = battle.You;
                return RedirectToAction("Index", "Game");
            }
            else if (submit == "Bag")
            {
                TempData["Character"] = battle.You;
                return RedirectToAction("Index", "Game");
            }
            else
            {
                if (battle.You.Spe > battle.Enemy.Spe)
                {
                    battle.Move(battle.You, submit);
                    if (battle.BattleWon == true)
                    {
                        character = (Character)Session["Character"];
                        character.GainXP(battle.Enemy.XpGain, sql);
                        battle.DropItem(sql);
                        sql.LoadBag(character);
                        TempData["BattleMessage"] = "GG, You won the battle. You gained " + battle.Enemy.XpGain + "XP. You also picked up the " + character.bag.Last().Name + " your enemy dropped.";
                        return RedirectToAction("Index", "Game");
                    }
                    battle.Move(battle.Enemy);
                    if (battle.BattleLost == true)
                    {
                        TempData["Character"] = battle.You;
                        TempData["BattleMessage"] = "You lost.";
                        return RedirectToAction("Index", "Game");
                    }
                }
                else
                {
                    battle.Move(battle.Enemy);
                    if (battle.BattleLost == true)
                    {
                        TempData["Character"] = battle.You;
                        TempData["BattleMessage"] = "You lost.";
                        return RedirectToAction("Index", "Game");
                    }
                    battle.Move(battle.You, submit);
                    if (battle.BattleWon == true)
                    {
                        battle.You.GainXP(battle.Enemy.XpGain, sql);
                        battle.DropItem(sql);
                        TempData["Character"] = battle.You;
                        TempData["BattleMessage"] = "GG, You won the battle. You gained " + battle.Enemy.XpGain + "XP. You also picked up the " + battle.You.bag.Last().Name + " your enemy dropped.";
                        return RedirectToAction("Index", "Game");
                    }
                }
            }
            TempData["Battle"] = battle;
            return View(battle);
        }
    }
}