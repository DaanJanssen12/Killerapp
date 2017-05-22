﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public enum Types
    {
        Dragon,
        Orc,
        Gollum
    }
    public class EvilCreature : Creature
    {
        public string Type { get; private set; }

        public EvilCreature(int lvl, string type)
        {
            Lvl = lvl;
            Type = type;
            this.GetStats();
        }

        public void GetStats()
        {
            int totalStats = 50 + (Lvl * 5);
            Random rng = new Random();
            int hp = rng.Next(Convert.ToInt32(totalStats / 4), Convert.ToInt32(totalStats / 2));
            totalStats = totalStats - hp;
            Atk = CalculateStat(totalStats, rng);
            Def = CalculateStat(totalStats, rng);
            SpAtk = CalculateStat(totalStats, rng);
            SpDef = CalculateStat(totalStats, rng);
            Spe = CalculateStat(totalStats, rng);
        }

        public int CalculateStat(int totalStats, Random rng)
        {
            int min = Convert.ToInt32(totalStats / 6);
            int stat = rng.Next(Convert.ToInt32(totalStats / 6), Convert.ToInt32(totalStats / 4));
            return stat;
        }
    }
}