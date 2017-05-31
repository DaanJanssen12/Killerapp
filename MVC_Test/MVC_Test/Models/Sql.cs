﻿using System;
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

        public bool UserExists(string username)
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

        public int GetUserId(string username)
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


        public void LoadCharacters(User user)
        {
            user.characters.Clear();
                string sql = @"SELECT [CharacterId], [Name], [Gender], [Class] FROM Character " +
                       @"WHERE [UserId] = @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@id", SqlDbType.Int))
                    .Value = user.Id;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
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
                reader.Dispose();
                cmd.Dispose();
            conn.Close();
        }

        public bool CreateCharacter(User user, Character character)
        {
            try
            {
                string sql = @"CreateCharacter @Name, @UserId, @Class, @Gender";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@Name", SqlDbType.NVarChar))
                    .Value = character.Name;
                cmd.Parameters
                    .Add(new SqlParameter("@UserId", SqlDbType.Int))
                    .Value = user.Id;
                cmd.Parameters
                    .Add(new SqlParameter("@Class", SqlDbType.NVarChar))
                    .Value = character.Class;
                cmd.Parameters
                    .Add(new SqlParameter("@Gender", SqlDbType.NVarChar))
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
            string sql = @"SELECT* FROM Stats " +
                       @"WHERE [CharacterId] = @id";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters
                .Add(new SqlParameter("@id", SqlDbType.Int))
                .Value = c.CharacterId;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
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
            reader.Dispose();
            cmd.Dispose();
            conn.Close();
        }

        public void UpdateStats(Character c)
        {
            string sql = "Update Stats" +
                "Set HP = @HP, Atk = @Atk, Def = @Def, SpAtk = @SpAtk, SpDef = @SpDef, Spe = @Spe, XP = @XP, Lvl = @Lvl" +
                "Where CharacterId = @id";
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
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void LoadBag(Character c)
        {
            try
            {
                c.bag.Clear();
                string sql = @"Select Name, Stat, Amount, Permanent, Class, Durability
                           From Item i
                           Join Bag b on i.ItemId = b.ItemId
                           Where b.CharacterId = @id";
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
                            c.bag.Add(new Item(reader[0].ToString(), reader[1].ToString(), Convert.ToInt32(reader[2]), Convert.ToBoolean(reader[3]), reader[4].ToString(), Convert.ToInt32(reader[5])));
                        }
                    }
                }
                conn.Close();
            }
            catch { }
            
        }

        public void DropBattleItem(Character c, int random)
        {
            try
            {
                string sql = @"BattleItemDrop @random, @Class, @Lvl, @CharacterId";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters
                    .Add(new SqlParameter("@random", SqlDbType.Int))
                    .Value = random;
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
            }
            catch
            { }
        }

        public void LoadMoves(Character c)
        {
            try
            {
                string sql = @"SELECT Move, Power, Type FROM Moves m Join Moveset ms on m.Move = ms.Move1 or m.Move = ms.Move2 or m.Move = ms.Move3 or m.Move = ms.Move4 WHERE CharacterId = @id";
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
                            c.Moves.Add(new Move(reader[0].ToString(), (Int32)reader[1], reader[2].ToString()));
                        }
                    }
                }
            }
            catch { }
        }

        public void LoadMoves(EvilCreature e)
        {
            try
            {
                string sql = @"SELECT TOP 4 Move, Power, Type FROM Moves WHERE Class is null and lvl < @lvl";
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
                            e.Moves.Add(new Move(reader[0].ToString(), (Int32)reader[1], reader[2].ToString()));
                        }
                    }
                }
            }
            catch { }
        }
    }
}