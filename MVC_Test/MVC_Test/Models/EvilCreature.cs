using System;
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
    //erft over van creature
    public class EvilCreature : Creature
    {
        public string Type { get; private set; }
        public int XpGain { get; set; }

        public EvilCreature(int lvl, string type)
        {
            Lvl = lvl;
            Type = type;
            XpGain = (lvl * lvl) / 4;
            this.GetStats();
            switch (Type)
            {
                case "Dragon":
                    Image = "~/Content/dragon.png";
                    break;
                case "Orc":
                    Image = "~/Content/Orc.png";
                    break;
                case "Gollum":
                    Image = "~/Content/gollum.png";
                    break;
            }
        }

        public void GetStats()
        {
            //bereken de stats van het creature
            int totalStats = 50 + ((Lvl-1) * 14);
            Random rng = new Random();
            HP = rng.Next(Convert.ToInt32(totalStats / 4), Convert.ToInt32(totalStats / 2));
            totalStats = totalStats - HP;
            Atk = CalculateStat(totalStats, rng);
            Def = CalculateStat(totalStats, rng);
            SpAtk = CalculateStat(totalStats, rng);
            SpDef = CalculateStat(totalStats, rng);
            Spe = CalculateStat(totalStats, rng);
        }

        public int CalculateStat(int totalStats, Random rng)
        {
            //bereken een stat
            int min = Convert.ToInt32(totalStats / 6);
            int stat = rng.Next(Convert.ToInt32(totalStats / 6), Convert.ToInt32(totalStats / 4));
            return stat;
        }
    }
}