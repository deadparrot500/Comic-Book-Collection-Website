using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class SearchSqlDAL
    {
        /*-----------------SETUP--------------------*/
        private readonly string connectionString;

        public SearchSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /*----------------METHODS-------------------*/

        public IList<User> SearchAllUsers(string input)
        {
            IList<User> users = new List<User>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandText = "SELECT username FROM users WHERE username LIKE '%@input%'";
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User();

                    user.Username = Convert.ToString(reader["username"]);

                    users.Add(user);
                }
            }

            return users;
        }

        public IList<Character> SearchAllCharacters(string input)
        {
            IList<Character> characters = new List<Character>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandText = "SELECT characters.character_name, charactersInComic.comic_id FROM characters " + "JOIN charactersInComic ON characters.character_id = charactersInComic.character_id " + "WHERE character_name LIKE '%@input%';";
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Character character = new Character();

                    character.Name = Convert.ToString(reader["character_name"]);
                    

                    characters.Add(character);
                }
            }





            return characters;
        }
    }
}
