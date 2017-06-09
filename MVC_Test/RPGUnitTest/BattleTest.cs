using System;
using MVC_Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RPGUnitTest
{
    [TestClass]
    public class BattleTest
    {
        [TestMethod]
        public void Battle()
        {
            Character c = new Character(new User(1, "test", "test"), 1, "Ja", "Mage", "Male")
            {
                Atk = 4,
                Def = 5,
                SpAtk = 12,
                SpDef = 4,
                Spe = 8,
                HP = 15
            };
            EvilCreature e = new EvilCreature(3, "Orc");
            Battle battle = new Battle(c, e);

            string s = "Battle - "+battle.You.Name+" vs lvl " + battle.Enemy.Lvl +" "+ battle.Enemy.Type;
            Assert.AreEqual("Battle - Ja vs lvl 3 Orc", s);
        }

    }
}
