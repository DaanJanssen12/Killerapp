﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Test.Models
{
    public interface ISql
    {
        //User
        bool Login(string username, string password);
        bool UserExists(string username);
        int GetUserId(string username);

        //Character
        void LoadCharacters(User user);
        bool CreateCharacter(User user, Character character);

        //Stats
        void LoadStats(Character c);

        void LoadBag(Character c);
    }
}