using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Tests.DAL
{
    [TestClass]
    public class CollectionSqlDALTests
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
        public void Get_All_Comics_In_A_Collection_Test()
        {
            ICollectionDAL dal = new CollectionSqlDAL(ConnectionString);
            List<Comic> result = dal.GetAllComicsInACollection(2);
            Assert.AreEqual("Image", result[0].Publisher);
        }

        [TestMethod]
        public void Get_All_Collections_For_A_User_Test()
        {
            ICollectionDAL dal = new CollectionSqlDAL(ConnectionString);
            List<Collection> result = dal.GetAllCollectionsFromUser(1);
            Assert.AreEqual("Comics from Middle School", result[1].CollectionName);

        }

        [TestMethod]
        public void Create_Collection_Test()
        {
            ICollectionDAL dal = new CollectionSqlDAL(ConnectionString);
            int result = dal.CreateCollection(1, "Test Collection", true);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Check_For_Value_Test()
        {
            bool expected = true;
            ICollectionDAL dal = new CollectionSqlDAL(ConnectionString);
            bool result = dal.CheckForValue("characters", "character_name", "Spawn");
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void Get_Collection_Data_Test()
        {
            ICollectionDAL dal = new CollectionSqlDAL(ConnectionString);
            Collection result = dal.GetCollectionData(1);
            Assert.AreEqual("testUser", result.UserName);
            result = dal.GetCollectionData(9);
            Assert.AreEqual("What The F David Blaine", result.CollectionName);

        }

        [TestMethod]
        public void Create_Test_Collection_Test()
        {
            ICollectionDAL dal = new CollectionSqlDAL(ConnectionString);
            Collection result = dal.CreateTestCollection();
            Assert.AreEqual("Image", result.ComicsInCollection);

        }
    }
}

