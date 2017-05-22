using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVC_Test.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        public List<Character> characters;

        public User(int id, string username, string password)
        {
            UserName = username;
            Password = password;
            Id = id;
            characters = new List<Character>();
        }

        public User()
        {
            characters = new List<Character>();
        }

        public void loadCharacters(ISql sql)
        {
            sql.LoadCharacters(this);
        }
    }
}