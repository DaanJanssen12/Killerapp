using System;
using MVC_Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RPGUnitTest
{
    [TestClass]
    public class CharacterTest
    {
        [TestMethod]
        public void CheckStats()
        {
            Character c = new Character()
            {
                Class = "Warior",
                Atk = 1,
                Def = 1,
                SpAtk = 1,
                SpDef = 1,
                Spe = 1,
                HP = 1
            };
            Assert.AreNotEqual(6, c.HP);
        }

        [TestMethod]
        public void CheckProperties()
        {
            Character c = new Character(new User(1, "test", "test"), 1, "Ja", "Mage", "Male");
            Assert.AreEqual("~/Content/wizard.png", c.Image);
            Assert.AreEqual("Male", c.Gender);
            Assert.AreEqual("Ja", c.Name);
            Assert.AreEqual("Mage", c.Class);
        }
    }
}
