using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public class CollectionSqlDAL : ICollectionDAL
    {
        private readonly string connectionString;

        public CollectionSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }


        public Collection GetCollectionData(int collectionId)
        {
            Collection collection = new Collection();

            string sqlCommand = "SELECT * FROM collections JOIN users ON users.id = collections.id WHERE collection_id = @collectionId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        collection.CollectionId = Convert.ToInt32(reader["collection_id"]);
                        collection.CollectionName = Convert.ToString(reader["collection_name"]);
                        collection.PublicStatus = Convert.ToBoolean(reader["public_status"]);
                        collection.UserName = Convert.ToString(reader["username"]);
                    }
                }

            }

            catch (SqlException ex)
            {
                throw ex;
            }

            return collection;
        }

        public List<Comic> GetAllComicsInACollection(int collectionId)
        {
            List<Comic> comics = new List<Comic>();

            string sqlCommand = "SELECT * FROM comicsInCollection JOIN comic ON comicsInCollection.comic_id = comic.comic_id " +
                "JOIN publisher ON comic.publisher_id = publisher.publisher_id " +
                "JOIN author ON comic.author_id = author.author_id " +
                "WHERE collection_id = @collectionid; ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@collectionId", collectionId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comics.Add(new Comic

                        {
                            ComicId = Convert.ToInt32(reader["comic_id"]),
                            Author = Convert.ToString(reader["author_name"]),
                            Title = Convert.ToString(reader["title"]),
                            PublishDate = Convert.ToDateTime(reader["publish_date"]),
                            Description = Convert.ToString(reader["description"]),
                            Publisher = Convert.ToString(reader["publisher_name"]),

                        });


                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return comics;
        }

        public List<Collection> GetAllCollectionsFromUserAnon(int userId)
        {

            List<Collection> collections = new List<Collection>();

            string sqlCommand = "SELECT * FROM collections " +
                                "JOIN users ON collections.id = users.id " +
                                "WHERE users.id = @userid AND public_status = 1";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@userid", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        collections.Add(new Collection

                        {
                            UserName = Convert.ToString(reader["username"]),
                            CollectionName = Convert.ToString(reader["collection_name"]),
                            CollectionId = Convert.ToInt32(reader["collection_id"]),
                            PublicStatus = Convert.ToBoolean(reader["public_status"])
                        });

                    }

                }

                return collections;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public List<Collection> GetAllCollectionsFromUser(int userId)
        {

            List<Collection> collections = new List<Collection>();

            string sqlCommand = "SELECT * FROM collections " +
                                "JOIN users ON collections.id = users.id " +
                                "WHERE users.id = @userid";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@userid", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        collections.Add(new Collection

                        {
                            UserName = Convert.ToString(reader["username"]),
                            CollectionName = Convert.ToString(reader["collection_name"]),
                            CollectionId = Convert.ToInt32(reader["collection_id"]),
                            PublicStatus = Convert.ToBoolean(reader["public_status"])
                        });

                    }

                }

                return collections;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public List<Collection> GetTopPublicCollections()
        {
            List<Collection> collections = new List<Collection>();

            string sqlCommand = "SELECT TOP 10 * FROM collections JOIN users ON collections.id = users.id WHERE public_status = '1' ORDER BY collection_id DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        collections.Add(new Collection

                        {
                            UserName = Convert.ToString(reader["username"]),
                            CollectionName = Convert.ToString(reader["collection_name"]),
                            CollectionId = Convert.ToInt32(reader["collection_id"]),
                            PublicStatus = Convert.ToBoolean(reader["public_status"])
                        });

                    }

                }

                return collections;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int GetUserId(string userName)
        {
            int userId = 0;
            string sqlCommand = "SELECT id FROM users WHERE username = @userName";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@userName", userName);


                    userId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return userId;
        }

        public int CreateCollection(int userId, string collectionName, bool publicStatus)
        {

            int newCollectionId = 0;

            
            string sqlCommand = "INSERT INTO collections (collection_name, id, public_status) VALUES (@collectionName, @userId, @publicStatus) " +
                            "SELECT collection_id FROM collections WHERE collection_name = @collectionName AND id = @userId";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@collectionName", collectionName);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@publicStatus", publicStatus);


                    newCollectionId = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return newCollectionId;
        }

        public bool CheckForValue(string tableName, string columnName, string value)
        {
            bool result = false;

            string sqlChecking = "SELECT COUNT (*) FROM " + @tableName +
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
                        result = true;
                    }

                }

                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public Collection CreateTestCollection()
        {
            //------------------------SETUP----------------------------//
            Collection collection = new Collection();
            int r = RandomNumber(1, 100);
            int collectionCount = 0;
            int comicCount = 0;
            List<Comic> comics = collection.ComicsInCollection;

            //--------Determines # of Rows in Collections Table--------//
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM collections", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    collectionCount++;
                }
            }

            //------------Determines # of Rows in Comics Table------------//
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM comic", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comicCount++;
                }
            }

            //--------Determines # of Comic Books in Collection--------//
            for (int i = 1; i < r; i++)
            {
                Comic comic = new Comic();
                comic.ComicId = RandomNumber(1, comicCount);

                //-----------------Fills Out Comic Info---------------------//
                string sqlCommand = "SELECT * FROM comic " +
                                    "JOIN author ON comic.author_id = author.author_id " +
                                    //"JOIN charactersInComic ON comic.comic_id = charactersInComic.comic_id " +
                                    "WHERE comic.comic_id = @comicid";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                    cmd.Parameters.AddWithValue("@comicid", comic.ComicId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comics.Add(new Comic

                        {
                            ComicId = Convert.ToInt32(reader["comic_id"]),
                            Title = Convert.ToString(reader["title"]),
                            Author = Convert.ToString(reader["author_name"]),
                            PublishDate = Convert.ToDateTime(reader["publish_date"]),
                            Description = Convert.ToString(reader["description"]),

                        });
                    }
                }
            }
            return collection;
        }

        public void DeleteCollection(int collectionId)
        {
            string sqlDelete = "DELETE FROM collections WHERE collection_id = @collection_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlDelete, conn);
                    cmd.Parameters.AddWithValue("@collection_id", collectionId);
                    
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

