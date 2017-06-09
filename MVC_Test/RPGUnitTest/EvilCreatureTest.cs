using System;
using MVC_Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RPGUnitTest
{
    [TestClass]
    public class EvilCreatureTest
    {
        [TestMethod]
        public void GetStats()
        {
            EvilCreature e = new EvilCreature(3, "Orc");
            Assert.IsNotNull(e.HP);
            Assert.IsNotNull(e.Atk);
            Assert.IsNotNull(e.SpAtk);
            Assert.IsNotNull(e.Def);
            Assert.IsNotNull(e.SpDef);
            Assert.IsNotNull(e.Spe);
        }

        [TestMethod]
        public void XpGain()
        {
            EvilCreature e = new EvilCreature(3, "Orc");
            Assert.AreEqual(2, e.XpGain);

            e = new EvilCreature(12, "Orc");
            Assert.AreEqual(36, e.XpGain);
        }
    }
}
