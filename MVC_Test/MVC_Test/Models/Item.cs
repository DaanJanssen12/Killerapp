using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Stat { get; set; }
        public int Amount { get; set; }
        public bool Permanent { get; set; }
        public int Durability { get; set; }
        public Item MadeOf { get; set; }

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

        }

        public void LoadItems(string madeOf, Sql sql)
        {
            string[] itemProperties = madeOf.Split('x');
            MadeOf = sql.LoadItem(Convert.ToInt32(itemProperties[0]), Convert.ToInt32(itemProperties[1]));
        }
    }
}