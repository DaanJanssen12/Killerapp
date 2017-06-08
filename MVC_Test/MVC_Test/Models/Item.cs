using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class Item
    {
        public string Name { get; set; }
        public string Stat { get; set; }
        public int Amount { get; set; }
        public bool Permanent { get; set; }
        public int Durability { get; set; }
        public List<Item> MadeOf;

        public Item(string name, string stat, int amount, bool permanent, int durability)
        {
            Name = name;
            Stat = stat;
            Amount = amount;
            Permanent = permanent;
            Durability = durability;
        }

        public Item()
        {
            MadeOf = new List<Item>();
        }

        public void LoadItems(string madeOf, Sql sql)
        {
            int itemId = Convert.ToInt32(madeOf.Substring(0, 1));
            int amount = Convert.ToInt32(madeOf.Substring(2, 1));
            Item item = sql.LoadItem(itemId, amount);
            MadeOf.Add(item);
        }
    }
}