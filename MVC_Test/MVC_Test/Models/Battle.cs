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

        public Battle(Character c, EvilCreature e)
        {
            You = c;
            Enemy = e;
        }
    }
}