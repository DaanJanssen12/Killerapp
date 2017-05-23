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

        public Battle(Character c, EvilCreature e)
        {
            You = c;
            Enemy = e;
            YourHP = c.HP;
            EnemyHP = e.HP;
        }

        public Battle()
        {

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
    }
}