﻿using System;
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
            character = (Character)TempData["Character"];
            sql.LoadStats(character);
            sql.LoadBag(character);
            sql.LoadMoves(character);
            TempData["Character"] = character;
            return View(character);
        }

        [HttpGet]
        public ActionResult Battle()
        {
            character = (Character)TempData["Character"];
            Random rng = new Random();
            int minLvl = 1;
            if (character.Lvl > 2)
            {
                minLvl = character.Lvl - 2;
            }
            int lvl = rng.Next(minLvl, character.Lvl + 2);
            string type = Enum.GetName(typeof(Types), rng.Next(0, 3));
            EvilCreature enemy = new EvilCreature(lvl, type);
            battle = new Battle(character, enemy);
            TempData["Battle"] = battle;
            return View(battle);
        }

        [HttpPost]
        public ActionResult Battle(string submit)
        {
            battle = (Battle)TempData["Battle"];
            foreach(Move move in battle.You.Moves)
            {
                if (submit == move.Name)
                {
                    battle.EnemyHP = battle.EnemyHP - battle.DamageCalc(battle.You, battle.Enemy, move);
                }
                else if (submit == "Run")
                {
                    TempData["Character"] = battle.You;
                    return RedirectToAction("Index", "Game");
                }
            }
            TempData["Battle"] = battle;
            return View(battle);
        }
    }
}