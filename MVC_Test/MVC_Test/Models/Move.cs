using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class Move
    {
        public string Name { get; private set; }
        public int Power { get; private set; }
        public string MoveType { get; private set; }

        public Move(string name, int power, string moveType)
        {
            Name = name;
            Power = power;
            MoveType = moveType;
        }
    }
}