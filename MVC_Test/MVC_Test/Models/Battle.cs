using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class Battle
    {
        public Character You { get; private set; }
        public EvilCreature Enemy { get; private set; }

        public int YourHP { get; set; }
        public int EnemyHP { get; set; }
        public List<string> BattleLog;
        public bool BattleWon { get; set; }
        public bool BattleLost { get; set; }

        public Battle(Character c, EvilCreature e)
        {
            You = c;
            Enemy = e;
            StatsAfterItems();
            YourHP = You.HP;
            EnemyHP = Enemy.HP;
            BattleLog = new List<string>();
        }

        public Battle()
        {

        }

        public void StatsAfterItems()
        {
            foreach (Item item in You.bag)
            {
                if (item.Permanent == true)
                {
                    string stat = item.Stat;
                    int amount = item.Amount;

                    switch (stat)
                    {
                        case "HP":
                            You.HP = You.HP + amount;
                            break;
                        case "Atk":
                            You.Atk = You.Atk + amount;
                            break;
                        case "SpAtk":
                            You.SpAtk = You.SpAtk + amount;
                            break;
                        case "SpDef":
                            You.SpDef = You.SpDef + amount;
                            break;
                        case "Def":
                            You.Def = You.Def + amount;
                            break;
                        case "Spe":
                            You.Spe = You.Spe + amount;
                            break;
                    }
                }
            }
        }

        public void Move(Character You, string submit)
        {
            foreach (Move move in You.Moves)
            {
                if (submit == move.Name)
                {
                    EnemyHP = EnemyHP - DamageCalc(You, Enemy, move);
                    if (EnemyHP <= 0)
                    {
                        BattleWon = true;
                    }
                    BattleLog.Add(You.Name + " used " + move.Name);
                }
            }
        }

        public void Move(EvilCreature Enemy)
        {
            Random rng = new Random();
            int i = rng.Next(0, Enemy.Moves.Count);
            Move move = Enemy.Moves[i];
            YourHP = YourHP - DamageCalc(Enemy, You, move);
            if (YourHP <= 0)
            {
                BattleLost = true;
            }
            BattleLog.Add("The opponent used " + move.Name);
        }

        public int DamageCalc(Creature offense, Creature defense, Move move)
        {
            int atk = 0, def = 0;

            if (move.MoveType == "Physical")
            {
                atk = offense.Atk;
                def = defense.Def;
            }
            else if (move.MoveType == "Special")
            {
                atk = offense.SpAtk;
                def = defense.SpDef;
            }

            double a = (double)(2 * offense.Lvl + 10) / 250;
            double b = (double)atk / def;
            int damage = Convert.ToInt32(a*b*move.Power); 
            return damage;
        }

        public void UseItem(string itemName, ISql sql)
        {
            foreach (Item item in You.bag)
            {
                if (item.Name == itemName)
                {
                    string stat = item.Stat;
                    int amount = item.Amount;

                    switch (stat)
                    {
                        case "HP":
                            YourHP = YourHP + amount;
                            if (YourHP > You.HP)
                            {
                                YourHP = You.HP;
                            }
                            break;
                        case "Atk":
                            You.Atk = You.Atk * amount;
                            break;
                        case "SpAtk":
                            You.SpAtk = You.SpAtk * amount;
                            break;
                        case "SpDef":
                            You.SpDef = You.SpDef * amount;
                            break;
                        case "Def":
                            You.Def = You.Def * amount;
                            break;
                        case "Spe":
                            You.Spe = You.Spe * amount;
                            break;
                    }
                    sql.UseBattleItem(itemName, You);
                    item.Durability = item.Durability - 1;
                    BattleLog.Add("You used a " + itemName);
                }
            }            
        }

        public void DropItem(ISql sql)
        {
            Random rng = new Random();
            sql.DropBattleItem(You, rng.Next(1, 100));
        }
    }
}