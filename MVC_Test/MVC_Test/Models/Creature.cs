﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    //abstracte classe creature
    public abstract class Creature
    {
        public int Atk { get; set; }
        public int Def { get; set; }
        public int SpAtk { get; set; }
        public int SpDef { get; set; }
        public int Spe { get; set; }
        public int HP { get; set; }
        public int Lvl { get; set; }
        public string Image { get; internal set; }

        public List<Move> Moves = new List<Move>();

    }
}