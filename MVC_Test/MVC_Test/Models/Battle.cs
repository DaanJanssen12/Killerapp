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

        public Battle(Character c, EvilCreature e)
        {
            You = c;
            Enemy = e;
            YourHP = c.HP;
            EnemyHP = e.HP;
            BattleLog = new List<string>();
        }

        public Battle()
        {

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
                BattleWon = false;
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

        public void DropItem(ISql sql)
        {
            Random rng = new Random();
            sql.DropBattleItem(You, rng.Next(1, 100));
        }
    }
}