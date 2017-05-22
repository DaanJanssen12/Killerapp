using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class Item
    {
        public string Name { get; private set; }
        public string Stat { get; private set; }
        public int Amount { get; private set; }
        public bool Permanent { get; private set; }
        public string Class { get; private set; }
        public int Durability { get; private set; }

        public Item(string name, string stat, int amount, bool permanent, string Class, int durability)
        {
            Name = name;
            Stat = stat;
            Amount = amount;
            Permanent = permanent;
            this.Class = Class;
            Durability = durability;
        }
    }
}