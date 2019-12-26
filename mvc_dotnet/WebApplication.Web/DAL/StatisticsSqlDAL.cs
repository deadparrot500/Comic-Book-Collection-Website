using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class StatisticsSqlDAL : IStatisticsDAL
    {
        /*-----------------SETUP--------------------*/
        private readonly string connectionString;

        public StatisticsSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        /*--------METHODS---COLLECTION STATS---------*/

        public string NumberOfComics(int collectionId)
        {
            int count = 0;

            string sqlCommand = "SELECT * FROM comicsInCollection WHERE collection_id = @collectionId";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@collectionId", collectionId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    count++;
                }
            }

            string result = "There are " + count + " comics in this collection.";
            return result;
        }

        public List<string> TopHeroes(int collectionId)
        {
            List<Character> characters = new List<Character>();
            List<string> results = new List<string>();

            string sqlCommand = "SELECT TOP 5 character_name, COUNT(character_name) AS 'NumOfAC' " + 
                "FROM comicsInCollection " +
                "JOIN comic ON comicsInCollection.comic_id = comic.comic_id " +
                "JOIN charactersInComic ON charactersInComic.comic_id = comic.comic_id " +
                "JOIN characters ON characters.character_id = charactersInComic.character_id " +
                "WHERE comicsInCollection.collection_id = @collectionId " +
                "GROUP BY character_name;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@collectionId", collectionId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Character character = new Character();

                    character.Name = Convert.ToString(reader["character_name"]);
                    character.NumOfAC = Convert.ToInt32(reader["NumOfAC"]);
                    characters.Add(character);
                }
            }

            foreach(Character item in characters)
            {
                string result = item.Name + ": appears " + item.NumOfAC + " times in this collection.";
                results.Add(result);
            }
            return results;
        }

        public List<string> TopPublishers(int collectionId)
        {
            List<Character> publishers = new List<Character>();
            List<string> results = new List<string>();

            string sqlCommand = "SELECT TOP 5 publisher_name, COUNT(publisher_name) AS 'NumOfAC' " +
                "FROM comicsInCollection " +
                "JOIN comic ON comic.comic_id = comicsInCollection.comic_id " +
                "JOIN publisher ON publisher.publisher_id = comic.publisher_id " +
                "WHERE collection_id = @collectionId " +
                "GROUP BY publisher_name;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@collectionId", collectionId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Character publisher = new Character();

                    publisher.Name = Convert.ToString(reader["publisher_name"]);
                    publisher.NumOfAC = Convert.ToInt32(reader["NumOfAC"]);
                    publishers.Add(publisher);
                }
            }

            foreach (Character item in publishers)
            {
                string result = item.Name + ": published " + item.NumOfAC + " of the comics in this collection.";
                results.Add(result);
            }
            return results;
        }

        public List<string> TopWriters(int collectionId)
        {
            List<Character> writers = new List<Character>();
            List<string> results = new List<string>();

            string sqlCommand = "SELECT TOP 5 author_name, COUNT(author_name) AS 'NumOfAC' " +
                "FROM comicsInCollection " +
                "JOIN comic ON comic.comic_id = comicsInCollection.comic_id " +
                "JOIN author ON comic.author_id = author.author_id " +
                "WHERE collection_id = @collectionId " +
                "GROUP BY author_name;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@collectionId", collectionId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Character writer = new Character();

                    writer.Name = Convert.ToString(reader["author_name"]);
                    writer.NumOfAC = Convert.ToInt32(reader["NumOfAC"]);
                    writers.Add(writer);
                }
            }

            foreach (Character item in writers)
            {
                string result = item.Name + " has written " + item.NumOfAC + " of the comics in this collection.";
                results.Add(result);
            }
            return results;
        }


        /*---------METHODS---AGGREGATE STATS----------*/

        public string NumberofComicsALL()
        {
            int count = 0;

            string sqlCommand = "SELECT * FROM comicsInCollection";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    count += 1;
                }
            }

            string result = "There are " + count + " copies of comic books on this website.";
            return result;
        }

        public List<string> TopHeroesALL()
        {
            List<Character> characters = new List<Character>();
            List<string> results = new List<string>();

            string sqlCommand = "SELECT TOP 5 character_name, COUNT(character_name) AS 'NumOfAC' " +
                "FROM comicsInCollection " +
                "JOIN comic ON comicsInCollection.comic_id = comic.comic_id " +
                "JOIN charactersInComic ON charactersInComic.comic_id = comic.comic_id " +
                "JOIN characters ON characters.character_id = charactersInComic.character_id " +
                "GROUP BY character_name;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Character character = new Character();

                    character.Name = Convert.ToString(reader["character_name"]);
                    character.NumOfAC = Convert.ToInt32(reader["NumOfAC"]);
                    characters.Add(character);
                }
            }

            foreach (Character item in characters)
            {
                string result = item.Name + ": " + item.NumOfAC + " Comic Issues.";
                results.Add(result);
            }
            return results;
        }

        public List<string> TopPublishersALL()
        {
            List<Character> publishers = new List<Character>();
            List<string> results = new List<string>();

            string sqlCommand = "SELECT TOP 5 publisher_name, COUNT(publisher_name) AS 'NumOfAC' " +
                "FROM comicsInCollection " +
                "JOIN comic ON comic.comic_id = comicsInCollection.comic_id " +
                "JOIN publisher ON publisher.publisher_id = comic.publisher_id " +
                "GROUP BY publisher_name;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Character publisher = new Character();

                    publisher.Name = Convert.ToString(reader["publisher_name"]);
                    publisher.NumOfAC = Convert.ToInt32(reader["NumOfAC"]);
                    publishers.Add(publisher);
                }
            }

            foreach (Character item in publishers)
            {
                string result = item.Name + ": published " + item.NumOfAC + " comics on this website.";
                results.Add(result);
            }
            return results;
        }

        public List<string> TopWritersALL()
        {
            List<Character> writers = new List<Character>();
            List<string> results = new List<string>();

            string sqlCommand = "SELECT TOP 5 author_name, COUNT(author_name) AS 'NumOfAC' " +
                "FROM comicsInCollection " +
                "JOIN comic ON comic.comic_id = comicsInCollection.comic_id " +
                "JOIN author ON comic.author_id = author.author_id " +
                "GROUP BY author_name;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Character writer = new Character();

                    writer.Name = Convert.ToString(reader["author_name"]);
                    writer.NumOfAC = Convert.ToInt32(reader["NumOfAC"]);
                    writers.Add(writer);
                }
            }

            foreach (Character item in writers)
            {
                string result = item.Name + ": wrote " + item.NumOfAC + " comics on this website.";
                results.Add(result);
            }
            return results;
        }
    }
}
