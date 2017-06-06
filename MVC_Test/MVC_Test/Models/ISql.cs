using System;
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
        void UpdateStats(Character c);

        //Item
        void DropBattleItem(Character c, int random);
        void UseBattleItem(string item, Character c);

        //Bag
        void LoadBag(Character c);

        //Moves
        void LoadMoves(Character c);
        void LoadMoves(EvilCreature e);
    }
}
