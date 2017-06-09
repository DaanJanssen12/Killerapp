using System;
using MVC_Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RPGUnitTest
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void Login()
        {
            User user = new User(1, "Daan", "test");
            Assert.AreEqual("Daan", user.UserName);
            Assert.AreEqual("test", user.Password);

            Assert.AreNotEqual("Daan123", user.UserName);
            Assert.AreNotEqual("test123", user.Password);
        }

        [TestMethod]
        public void CharactersForUser()
        {
            User user = new User(1, "admin", "admin");
            Character c = new Character(user, 1, "Henk", "Hunter", "Male");
            Character c2 = new Character(user, 2, "Sjaak", "Mage", "Male");

            Assert.AreEqual(user.Id, c.UserId);
            Assert.AreEqual(user.Id, c2.UserId);
        }
    }
}
