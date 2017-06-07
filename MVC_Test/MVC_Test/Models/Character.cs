using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    enum Classes
    {
        Warior,
        Mage,
        Hunter,
        Druid
    }

    public class Character : Creature
    {
        public int CharacterId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Gender { get; set; }
        public int XP { get; set; }

        public List<Item> bag;

        public Character(User user, int characterId, string name, string Class, string gender)
        {
            UserId = user.Id;
            CharacterId = characterId;
            Name = name;
            this.Class = Class;
            Gender = gender;
            bag = new List<Item>();
            switch (this.Class)
            {
                case "Warior":
                    Image = "~/Content/warrior.png";
                    break;
                case "Mage":
                    Image = "~/Content/wizard.png";
                    break;
                case "Hunter":
                    Image = "~/Content/hunter.jpg";
                    break;
                case "Druid":
                    Image = "~/Content/druid.png";
                    break;
            }
        }

        public Character()
        {

        }

        public override string ToString()
        {
            return this.Name + " - " + this.Gender + " " + this.Class;
        }

        public void GainXP(int xp, ISql sql)
        {
            sql.LoadStats(this);
            XP = XP + xp;
            double lvl = Math.Sqrt(XP);
            int myLvl = Convert.ToInt32(Math.Floor(lvl));
            if (myLvl > Lvl)
            {
                int levels = myLvl - Lvl;
                Lvl = myLvl;
                LvlUp(levels);
            }
            sql.UpdateStats(this);
        }

        public void LvlUp(int amount)
        {
            switch (Class)
            {
                case "Mage":
                    HP = HP + (3*amount);
                    Atk = Atk + (1*amount);
                    Def = Def + (2*amount);
                    SpAtk = SpAtk + (4*amount);
                    SpDef = SpDef + (2*amount);
                    Spe = Spe + (3*amount);
                    break;

                case "Warior":
                    HP = HP + (5 * amount);
                    Atk = Atk + (3 * amount);
                    Def = Def + (2 * amount);
                    SpAtk = SpAtk + (1 * amount);
                    SpDef = SpDef + (1 * amount);
                    Spe = Spe + (3 * amount);
                    break;

                case "Hunter":
                    HP = HP + (2 * amount);
                    Atk = Atk + (4 * amount);
                    Def = Def + (2 * amount);
                    SpAtk = SpAtk + (1 * amount);
                    SpDef = SpDef + (2 * amount);
                    Spe = Spe + (4 * amount);
                    break;

                case "Druid":
                    HP = HP + (6 * amount);
                    Atk = Atk + (0 * amount);
                    Def = Def + (3 * amount);
                    SpAtk = SpAtk + (2 * amount);
                    SpDef = SpDef + (3 * amount);
                    Spe = Spe + (1 * amount);
                    break;
            }
        }
    }
}