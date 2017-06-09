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

        public ActionResult Index()
        {
            //kijk of er een character is gekozen
            if (Session["Character"] == null)
            {
                return RedirectToAction("Index", "Menu");
            }
            Session["Battle"] = null;
            character = (Character)Session["Character"];
            //laad alle stats, items, moves van het character
            sql.LoadStats(character);
            sql.LoadBag(character);
            sql.LoadMoves(character);
            Session["Character"] = character;
            return View(character);
        }

        [HttpGet]
        public ActionResult Battle()
        {
            //check of er een character is en of er al een battle is
            if(Session["Character"] == null)
            {
                return RedirectToAction("Index", "Menu");
            }
            if (Session["Battle"] != null)
            {
                battle = (Battle)Session["Battle"];
            }
            else
            {
                //genereer een tegenstander en maak de battle
                character = (Character)Session["Character"];
                Random rng = new Random();
                int minLvl = 2;
                if (character.Lvl > 4)
                {
                    minLvl = character.Lvl - 2;
                }
                int lvl = rng.Next(minLvl, character.Lvl + 2);
                string type = Enum.GetName(typeof(Types), rng.Next(0, 3));
                EvilCreature enemy = new EvilCreature(lvl, type);
                sql.LoadMoves(enemy);
                battle = new Battle(character, enemy);
                Session["Battle"] = battle;
            }
            return View(battle);

        }
        [HttpPost]
        public ActionResult Battle(string submit)
        {
            //kijk welke handeling de gebruiker doet
            battle = (Battle)Session["Battle"];
            if (submit == "Run")
            {
                //ren weg van het gevecht
                battle.LoseDurability(sql);
                return RedirectToAction("Index", "Game");
            }
            else if (submit == "Bag")
            {
                //ga naar je bag
                return RedirectToAction("BattleBag", "Game");
            }
            else
            {
                //kijk wie er sneller is, die maakt zijn move eerst
                if (battle.You.Spe > battle.Enemy.Spe)
                {
                    battle.Move(battle.You, submit);
                    if (battle.BattleWon == true)
                    {
                        character = (Character)Session["Character"];
                        character.GainXP(battle.Enemy.XpGain, sql);
                        battle.LoseDurability(sql);
                        battle.DropItem(sql);
                        sql.LoadBag(character);
                        TempData["BattleMessage"] = "GG, You won the battle. You gained " + battle.Enemy.XpGain + "XP. You also picked up the item your enemy dropped and put it in your bag.";
                        return RedirectToAction("Index", "Game");
                    }
                    battle.Move(battle.Enemy);
                    if (battle.BattleLost == true)
                    {
                        battle.LoseDurability(sql);
                        TempData["BattleMessage"] = "You lost.";
                        return RedirectToAction("Index", "Game");
                    }
                }
                else
                {
                    battle.Move(battle.Enemy);
                    if (battle.BattleLost == true)
                    {
                        battle.LoseDurability(sql);
                        TempData["BattleMessage"] = "You lost.";
                        return RedirectToAction("Index", "Game");
                    }
                    battle.Move(battle.You, submit);
                    if (battle.BattleWon == true)
                    {
                        character = (Character)Session["Character"];
                        battle.LoseDurability(sql);
                        character.GainXP(battle.Enemy.XpGain, sql);
                        battle.DropItem(sql);
                        TempData["BattleMessage"] = "GG, You won the battle. You gained " + battle.Enemy.XpGain + "XP. You also picked up the item your enemy dropped and put it in your bag.";
                        return RedirectToAction("Index", "Game");
                    }
                }
            }
            Session["Battle"] = battle;
            return View("Battle", battle);
        }
        [HttpGet]
        public ActionResult BattleBag()
        {
            battle = (Battle)Session["Battle"];
            return View(battle);
        }

        [HttpPost]
        public ActionResult BattleBag(string submit)
        {
            //kijk welk item er gebruikt wordt
            battle = (Battle)Session["Battle"];
            if (submit != "Back to battle")
            {
                string itemName = submit.Substring(4);
                battle.UseItem(itemName, sql);
                battle.Move(battle.Enemy);
            }
            Session["Battle"] = battle;
            return RedirectToAction("Battle", "Game");
        }

        public ActionResult Craft()
        {
            //laad alle craftable items die beschickbaar zijn voor het character
            List<Item> craftableItems = sql.LoadCraftableItems((Character)Session["Character"]);
            TempData["Craftables"] = craftableItems;
            return View(craftableItems);
        }

        [HttpPost]
        public ActionResult Craft(string craft)
        {
            //kijk welk item er wordt gecraft en check of dat ook kan
            character = (Character)Session["Character"];
            List<Item> craftables = (List<Item>)TempData["Craftables"];
            bool craftable = false;
            Item craftItem = null, craftedItem = null;
            string itemName = craft.Substring(8);
            foreach (Item item in craftables)
            {
                if (item.Name == itemName)
                {
                        foreach (Item x in character.bag)
                        {
                            if (item.MadeOf.Name == x.Name && x.Durability >= item.MadeOf.Durability)
                            {
                                craftable = true;
                                x.Durability = x.Durability - item.MadeOf.Durability;
                                craftItem = x;
                                craftedItem = item;
                            }
                        }
                }
            }
            //als het craften mogelijk is voer het dan uit, stuur anders een bericht terug dat het niet kan
            if(craftable == true)
            {
                if (craftItem.Durability != 0)
                {
                    sql.LoseDurability(craftItem, character);
                }
                else
                {
                    sql.DeleteItem(craftItem, character);
                }
                TempData["CraftMessage"] = "You succesfully crafted a " + itemName;
                sql.CraftItem(craftedItem.Id, character);
            }
            else
            {
                TempData["CraftMessage"] = "You can only craft items if you have the items necesary!";
            }
            TempData["Craftables"] = craftables;
            return View(craftables);
        }
    }
}