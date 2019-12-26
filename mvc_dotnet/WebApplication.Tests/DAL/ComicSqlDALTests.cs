using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Tests.DAL
{
    [TestClass]
    public class ComicSqlDALTests
    {
        protected string ConnectionString { get; } = "Data Source=.\\sqlexpress;Initial Catalog=ComicCollector;Integrated Security=True;MultipleActiveResultSets=True";

        private TransactionScope transaction;

        [TestInitialize]
        public void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Roll back the transaction
            transaction.Dispose();
        }

        [TestMethod]
        public void Get_All_Characters_In_A_Comic_Test()
        {
            IComicDAL dal = new ComicSqlDAL(ConnectionString);
            List<string> result = dal.GetAllCharactersInAComic(1);
            Assert.AreEqual("Spawn", result[0]);
        }

        [TestMethod]
        public void Get_Comic_Data_Test()
        {
            IComicDAL dal = new ComicSqlDAL(ConnectionString);
            Comic result = dal.GetComicData(1);
            Assert.AreEqual("Image", result.Publisher);

        }


        [TestMethod]
        public void Create_Author_Test()
        {
            IComicDAL dal = new ComicSqlDAL(ConnectionString);
            int result = dal.CreateAuthor("Tom Jones");
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void Create_Publisher_Test()
        {
            IComicDAL dal = new ComicSqlDAL(ConnectionString);
            int result = dal.CreatePublisher("Garage Comics");
            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void Create_Character_Test()
        {
            IComicDAL dal = new ComicSqlDAL(ConnectionString);
            int result = dal.CreateCharacter("Tom Jones", 1);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void Create_Comic_Test()
        {
            IComicDAL comicDal = new ComicSqlDAL(ConnectionString);
            ICollectionDAL collectionDal = new CollectionSqlDAL(ConnectionString);
            int userID = 1;
            int collectionID = 1;
            string author = "Tom Jones";
            string title = "Whatever";
            DateTime publishDate = Convert.ToDateTime("11-05-2006");
            string publisher = "Garage Comics";
            string description = "Seriously, like, whatever";
            List<string> characters = new List<string>() { "Mean Girl", "Mean Mom", "Nice Dad", "Out-of-control Kid"};

            int result = comicDal.CreateComic(userID, collectionID, author, title, publishDate, description, publisher, characters);
            Comic comic = comicDal.GetComicData(result);
            comic.Characters = comicDal.GetAllCharactersInAComic(comic.ComicId);
            Assert.AreEqual(title, comic.Title);

        }

    }
}
