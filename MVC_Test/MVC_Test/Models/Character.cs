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
        }

        public Character()
        {

        }

        public override string ToString()
        {
            return this.Name + " - " + this.Gender + " " + this.Class;
        }

        public override void LoadMoves()
        {
            throw new NotImplementedException();
        }

        public void GainXP(int xp)
        {
            XP = XP + xp;
            double lvl = Math.Sqrt(XP);
            int myLvl = Convert.ToInt32(Math.Floor(lvl));
            Lvl = myLvl;
        }
    }
}