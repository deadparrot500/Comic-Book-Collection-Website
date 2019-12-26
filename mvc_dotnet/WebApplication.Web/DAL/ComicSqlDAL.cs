using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication.Web.Models;
using WebApplication.Web.DAL;
using System.Linq;
using Microsoft.AspNetCore.Http;


namespace WebApplication.Web.DAL
{
    public class ComicSqlDAL : IComicDAL
    {
        //---------------SETUP-----------------//
        private readonly string connectionString;

        public ComicSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /*----------------METHODS-------------------*/

        public int CreateComic(int userId, int collectionId, string author, string title, DateTime publishDate, string description, string publisher, HashSet<string> characters)
        {
            Comic comic = new Comic();
          

            string sqlCommandInsert = "INSERT INTO comic (author_id, title, publish_date, publisher_id, description) VALUES " +
                                "(@authorId, @title, @publishDate, @publisherId, @description); " +
                                "SELECT comic_id FROM comic ORDER BY comic_id DESC";

            int authorId = CreateAuthor(author);
            int publisherId = CreatePublisher(publisher);


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommandInsert, conn);
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@publishDate", publishDate);
                    cmd.Parameters.AddWithValue("@publisherId", publisherId);
                    cmd.Parameters.AddWithValue("@description", description);


                    comic.ComicId = Convert.ToInt32(cmd.ExecuteScalar());

                }


            }
            catch (SqlException ex)
            {
                throw ex;
            }




            foreach (string item in characters)
            {
                if (item != null)
                {
                    int characterId = CreateCharacter(item, publisherId);
                    AddCharacterToComic(characterId, comic.ComicId);
                }
            }

            AddComicToCollection(collectionId, comic.ComicId);

            return comic.ComicId;
        }

        public int CreateAuthor(string authorName)
        {
            int authorId = CheckForValue("author", "author_name", authorName);

            if (authorId == 0)
            {

                string sqlCommand = "INSERT INTO author (author_name) VALUES (@authorName); " +
                            "SELECT author_id FROM author WHERE author_name = @authorName";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                        cmd.Parameters.AddWithValue("@authorName", authorName);

                        authorId = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }

            }

            return authorId;
        }

        public int CreatePublisher(string publisherName)
        {
            int publisherId = CheckForValue("publisher", "publisher_name", publisherName);

            if (publisherId == 0)
            {

                string sqlCommand = "INSERT INTO publisher (publisher_name) VALUES (@publisherName); " +
                            "SELECT publisher_id FROM publisher WHERE publisher_name = @publisherName";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                        cmd.Parameters.AddWithValue("@publisherName", publisherName);

                        publisherId = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }

            }

            return publisherId;
        }

        public int CreateCharacter(string characterName, int publisherId)
        {

            int characterId = CheckForValue("characters", "character_name", characterName);

            if (characterId == 0)
            {

                string sqlCommand = "INSERT INTO characters (character_name, publisher_id) VALUES (@characterName, @publisherid); " +
                            "SELECT character_id FROM characters WHERE character_name = @characterName and publisher_id = @publisherid";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                        cmd.Parameters.AddWithValue("@characterName", characterName);
                        cmd.Parameters.AddWithValue("@publisherid", publisherId);

                        characterId = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }
                catch (SqlException ex)
                {
                    throw ex;
                }

            }

            return characterId;

        }

        public Comic GetComicData(int comicId)
        {
            Comic comic = new Comic();

            string sqlCommand = "SELECT * FROM comic " +
                "JOIN publisher ON comic.publisher_id = publisher.publisher_id " +
                "JOIN author ON comic.author_id = author.author_id " +
                "WHERE comic_id = @comicId;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@comicId", comicId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comic.Title = Convert.ToString(reader["title"]);
                        comic.Publisher = Convert.ToString(reader["publisher_name"]);
                        comic.Author = Convert.ToString(reader["author_name"]);
                        comic.ComicId = Convert.ToInt32(reader["comic_id"]);
                        comic.Description = Convert.ToString(reader["description"]);
                        comic.PublishDate = Convert.ToDateTime(reader["publish_date"]);

                    }
                }

            }

            catch (SqlException ex)
            {
                throw ex;
            }

            return comic;
        }

        public List<string> GetAllCharactersInAComic(int comicId)
        {
            List<string> character = new List<string>();

            string sqlCommand = "SELECT * FROM charactersInComic JOIN characters ON charactersInComic.character_id = characters.character_id " + "WHERE comic_id = @comicId ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@comicId", comicId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        character.Add(Convert.ToString(reader["character_name"]));

                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return character;
        }

        public void AddCharacterToComic(int characterId, int comicId)
        {
            string sqlCommand = "INSERT INTO charactersinComic (character_id, comic_id) VALUES (@characterId, @comicId) ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@characterId", characterId);
                    cmd.Parameters.AddWithValue("@comicId", comicId);

                    cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public void AddComicToCollection(int collectionId, int comicId)
        {

            string sqlCommand = "INSERT INTO comicsInCollection (collection_id, comic_id) VALUES (@collectionId, @comicId) ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);
                    cmd.Parameters.AddWithValue("@comicId", comicId);

                    cmd.ExecuteNonQuery();

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public IList<Comic> SearchCharacter(string search)
        {
            IList<Comic> results = new List<Comic>();


            string sqlSearch = "SELECT * FROM characters " +
                                "JOIN charactersInComic ON characters.character_id = charactersInComic.character_id " +
                                "JOIN comic ON charactersInComic.comic_id = comic.comic_id " +
                                "JOIN comicsInCollection ON comic.comic_id = comicsInCollection.comic_id " +
                                "JOIN collections ON comicsInCollection.collection_id = collections.collection_id " +
                                "JOIN users ON collections.id = users.id " +
                                "JOIN publisher ON comic.publisher_id = publisher.publisher_id " +
                                "WHERE character_name LIKE @search ORDER BY character_name";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlSearch, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Comic comic = new Comic();
                        comic.Collection = new Collection();

                        comic.SearchCharacter = Convert.ToString(reader["character_name"]);
                        comic.Title = Convert.ToString(reader["title"]);
                        comic.Publisher = Convert.ToString(reader["publisher_name"]);
                        comic.Collection.CollectionName = Convert.ToString(reader["collection_name"]);
                        comic.Collection.CollectionId = Convert.ToInt32(reader["collection_id"]);
                        comic.UserName = Convert.ToString(reader["username"]);
                        comic.ComicId = Convert.ToInt32(reader["comic_id"]);

                        results.Add(comic);
                    }

                }
            }

            catch (SqlException ex)
            {
                throw ex;
            }

            return results;

        }


        public int CheckForValue(string tableName, string columnName, string value)
        {
            int result = 0;

            string sqlChecking = "SELECT * FROM " + @tableName +
                                " WHERE " + @columnName + " = @value";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlChecking, conn);
                    cmd.Parameters.AddWithValue("@tableName", tableName);
                    cmd.Parameters.AddWithValue("@columnName", columnName);
                    cmd.Parameters.AddWithValue("@value", value);


                    if (Convert.ToInt32(cmd.ExecuteScalar()) >= 1)
                    {
                        result = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                }

                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int CheckNumberOfComicsInCollection(int collectionId)
        {
            int result = 0;

            string sqlChecking = "SELECT COUNT (*) FROM comicsInCollection WHERE collection_id = @collectionId";


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlChecking, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                }

                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteCharacter(int characterId, int comicId)
        {
            string sqlDelete = "DELETE FROM charactersInComic WHERE character_id = @characterId AND comic_id = @comicId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlDelete, conn);
                    cmd.Parameters.AddWithValue("@characterId", characterId);
                    cmd.Parameters.AddWithValue("@comicId", comicId);

                    cmd.ExecuteNonQuery();

                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int CheckForCharacterInComic(int characterId, int comicId)
        {
            int result = 0;

            string sqlChecking = "SELECT comic_id FROM charactersInComic WHERE character_id = @character_id AND comic_id = @comic_id";


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlChecking, conn);
                    cmd.Parameters.AddWithValue("@character_id", characterId);
                    cmd.Parameters.AddWithValue("@comic_id", comicId);

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return result;
        }

        public void DeleteComicFromCollection(int comicId, int collectionId)
        {

            string sqlDelete = "DELETE FROM comicsInCollection WHERE comic_id = @comicId AND collection_Id = @collectionId; " +
                                 "DELETE FROM comic WHERE comic_id = @comicId" ; 
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlDelete, conn);
                    cmd.Parameters.AddWithValue("@comicId", comicId);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);

                    cmd.ExecuteNonQuery();

                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }


        }
        

    }


}
