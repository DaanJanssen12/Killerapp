using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class Sql : ISql
    {
        string connectionString = "Server=mssql.fhict.local;Database=dbi365864;User Id=dbi365864;Password=Daan12041999;";

        SqlConnection conn;

        public Sql()
        {
            conn = new SqlConnection(connectionString);
        }

        public bool Login(string username, string password)
        {
            try
            {
                string sql = @"SELECT [UserId],[Username],[Password] FROM [User] " +
                       @"WHERE [Username] = @u AND [Password] = @p";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = username;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = password;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Dispose();
                    cmd.Dispose();
                    conn.Close();
                    return true;
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    conn.Close();
                    return false;
                }
            }
            catch { return false; }
                
        }

        public bool UserExists(string username)
        {
            try
            {
                string sql = @"SELECT [Username] FROM [User] " +
                       @"WHERE [Username] = @u";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = username;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Dispose();
                    cmd.Dispose();
                    conn.Close();
                    return true;
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    conn.Close();
                    return false;
                }
            }
            catch { return false; }
                
        }

        public int GetUserId(string username)
        {
            try
            {
                string sql = @"SELECT [UserId] FROM [User] " +
                       @"WHERE [Username] = @u";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = username;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader[0]);
                    conn.Close();
                    return id;
                }
                else
                {
                    conn.Close();
                    return 505;
                }
            }
            catch { return 505; }
                
        }

        public void CreateUser(User user)
        {
            try
            {
                string sql = "INSERT INTO [User](Username, Password)" +
                             "VALUES(@username, @password)";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@username", SqlDbType.VarChar))
                    .Value = user.UserName;
                cmd.Parameters
                    .Add(new SqlParameter("@password", SqlDbType.VarChar))
                    .Value = user.Password;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch { }

        }

        public void LoadCharacters(User user)
        {
            try
            {
                user.characters.Clear();
                string sql = @"SELECT [CharacterId], [Name], [Gender], [Class] FROM Character " +
                       @"WHERE [UserId] = @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = user.Id;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader[0]);
                            string name = reader[1].ToString();
                            string gender = reader[2].ToString();
                            string Class = reader[3].ToString();
                            user.characters.Add(new Character(user, id, name, Class, gender));
                        }
                    }
                }                  
            }
            catch { }
            
        }

        public bool CreateCharacter(User user, Character character)
        {
            try
            {
                string sql = @"CreateCharacter @Name, @UserId, @Class, @Gender";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@Name", SqlDbType.VarChar))
                    .Value = character.Name;
                cmd.Parameters
                    .Add(new SqlParameter("@UserId", SqlDbType.Int))
                    .Value = user.Id;
                cmd.Parameters
                    .Add(new SqlParameter("@Class", SqlDbType.VarChar))
                    .Value = character.Class;
                cmd.Parameters
                    .Add(new SqlParameter("@Gender", SqlDbType.VarChar))
                    .Value = character.Gender;
                conn.Open();
                cmd.ExecuteNonQuery();
                user.characters.Add(character);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public void LoadStats(Character c)
        {
            try
            {
                string sql = @"SELECT* FROM Stats " +
                       @"WHERE [CharacterId] = @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            c.Atk = Convert.ToInt32(reader[1]);
                            c.Def = Convert.ToInt32(reader[2]);
                            c.SpAtk = Convert.ToInt32(reader[3]);
                            c.SpDef = Convert.ToInt32(reader[4]);
                            c.Spe = Convert.ToInt32(reader[5]);
                            c.HP = Convert.ToInt32(reader[6]);
                            c.XP = Convert.ToInt32(reader[7]);
                            c.Lvl = Convert.ToInt32(reader[8]);
                        }
                    }
                }
                conn.Close();
            }
            catch { }           
        }

        public void UpdateStats(Character c)
        {
            try
            {
                string sql = "Update Stats " +
                "Set HP = @HP, Atk = @Atk, Def = @Def, SpAtk = @SpAtk, SpDef = @SpDef, Spe = @Spe, XP = @XP, Lvl = @Lvl" +
                " Where CharacterId = @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@HP", SqlDbType.Int))
                    .Value = c.HP;
                cmd.Parameters
                    .Add(new SqlParameter("@Atk", SqlDbType.Int))
                    .Value = c.Atk;
                cmd.Parameters
                    .Add(new SqlParameter("@Def", SqlDbType.Int))
                    .Value = c.Def;
                cmd.Parameters
                    .Add(new SqlParameter("@SpAtk", SqlDbType.Int))
                    .Value = c.SpAtk;
                cmd.Parameters
                    .Add(new SqlParameter("@SpDef", SqlDbType.Int))
                    .Value = c.SpDef;
                cmd.Parameters
                    .Add(new SqlParameter("@Spe", SqlDbType.Int))
                    .Value = c.Spe;
                cmd.Parameters
                    .Add(new SqlParameter("@XP", SqlDbType.Int))
                    .Value = c.XP;
                cmd.Parameters
                    .Add(new SqlParameter("@Lvl", SqlDbType.Int))
                    .Value = c.Lvl;
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch { }          
        }

        public void LoadBag(Character c)
        {
            c.bag.Clear();
            try
            {
                string sql ="Select Name, Stat, Amount, Permanent, Durability"+
                           " From Item i"+
                           " Join Bag b on i.ItemId = b.ItemId"+
                           " Where b.CharacterId = @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            c.bag.Add(new Item(reader[0].ToString(), reader[1].ToString(), Convert.ToInt32(reader[2]), Convert.ToBoolean(reader[3]), Convert.ToInt32(reader[4])));
                        }
                    }
                }
                conn.Close();
            }
            catch { }
            
        }

        public List<Item> LoadCraftableItems(Character c)
        {
            try
            {
                string sql = @"SELECT MadeOf, Name, ItemId FROM Item WHERE MadeOf is not null AND Lvl <= @lvl AND (Class = @class or Class is null)";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@lvl", SqlDbType.Int))
                    .Value = c.Lvl;
                cmd.Parameters
                    .Add(new SqlParameter("@class", SqlDbType.VarChar))
                    .Value = c.Class;
                conn.Open();
                List<Item> craftableItems = new List<Item>();
                List<string> madeOf = new List<string>();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {                  
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            madeOf.Add(reader[0].ToString());
                            Item item = new Item();
                            item.Name = reader[1].ToString();
                            item.Id = (Int32)reader[2];
                            craftableItems.Add(item);
                        }
                    }
                    conn.Close();
                }
                for (int i = 0; i < craftableItems.Count; i++)
                {
                    craftableItems[i].LoadItems(madeOf[i], this);
                }
                return craftableItems;
            }
            catch { return null; }
        }

        public Item LoadItem(int id, int amount)
        {
            try
            {
                string sql = @"Select Name, Stat, Amount, Permanent" +
                           " From Item" +
                           " Where ItemId = @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = id;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            Item item = new Item(reader[0].ToString(), reader[1].ToString(), Convert.ToInt32(reader[2]), Convert.ToBoolean(reader[3]), amount);
                            conn.Close();
                            return item;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }               
            }
            catch { return null; }
        }

        public void LoseDurability(Item i, Character c)
        {
            try
            {
                string sql = "Update Bag " +
                "Set Durability = @Durability" +
                " Where CharacterId = @id AND ItemId = (SELECT ItemId FROM Item WHERE Name = @item)";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@Durability", SqlDbType.Int))
                    .Value = i.Durability;
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                cmd.Parameters
                    .Add(new SqlParameter("@item", SqlDbType.VarChar))
                    .Value = i.Name;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch { }           
        }

        public void CraftItem(int id, Character c)
        {
            try
            {
                string sql = @"CraftItem @item, @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@item", SqlDbType.Int))
                    .Value = id;
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            { }
        }

        public void DeleteItem(Item i, Character c)
        {
            try
            {
                string sql = "DELETE FROM Bag " +
                                " Where CharacterId = @id AND ItemId = (SELECT ItemId FROM Item WHERE Name = @item)";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                cmd.Parameters
                    .Add(new SqlParameter("@item", SqlDbType.VarChar))
                    .Value = i.Name;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch { }
        }

        public void DropBattleItem(Character c)
        {
            try
            {
                string sql = @"BattleItemDrop @Class, @Lvl, @CharacterId";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@Class", SqlDbType.VarChar))
                    .Value = c.Class;
                cmd.Parameters
                    .Add(new SqlParameter("@Lvl", SqlDbType.Int))
                    .Value = c.Lvl;
                cmd.Parameters
                    .Add(new SqlParameter("@CharacterId", SqlDbType.Int))
                    .Value = c.CharacterId;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            { }
        }

        public void UseBattleItem(string item, Character c)
        {
            try
            {
                string sql = @"UseItem @item, @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@item", SqlDbType.VarChar))
                    .Value = item;
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            { }
        }

        public void LoadMoves(Character c)
        {
            c.Moves.Clear();
            try
            {
                string sql = @"SELECT Move, Power, Type, Effect FROM Moves m Join Moveset ms on m.Move = ms.Move1 or m.Move = ms.Move2 or m.Move = ms.Move3 or m.Move = ms.Move4 WHERE CharacterId = @id Order by m.Class desc";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = c.CharacterId;
                conn.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            c.Moves.Add(new Move(reader[0].ToString(), (Int32)reader[1], reader[2].ToString(), reader[3].ToString()));
                        }
                    }
                }
                conn.Close();
            }
            catch { }
        }

        public void LoadMoves(EvilCreature e)
        {
            try
            {
                string sql = @"SELECT TOP 4 Move, Power, Type, Effect FROM Moves WHERE Class is null and lvl < @lvl";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@lvl", SqlDbType.Int))
                    .Value = e.Lvl;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            e.Moves.Add(new Move(reader[0].ToString(), (Int32)reader[1], reader[2].ToString(), reader[3].ToString()));
                        }
                    }
                }
            }
            catch { }
        }
    }
}