using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RootFileReader.Controllers;
using RootFileReader.DAL;
using RootFileReader.Models;

namespace RootFileReaderTests
{
    [TestClass]
    public class FileReaderTests
    {
        private string filePath = "C:\\Users\\deadp\\TE-Work\\After Graduation\\RootFileReader\\TestInput.txt";

        [TestMethod]
        public void TestReader()
        {
            string check = "";

            FileReader reader = new FileReader();

            check = reader.TestReadFile();

            Assert.AreEqual("Driver Dan", check);
        }

        [TestMethod]
        public void TestReturnFirstWord()
        {
            FileReader reader = new FileReader();

            string line = reader.TestReadFile();

            string check = reader.FirstWord(line);

            Assert.AreEqual("Driver", check);
        }


        [TestMethod]
        public void TestReturnSecondWord()
        {
            FileReader reader = new FileReader();

            string line = reader.TestReadFile();

            string check = reader.SecondWord(line);

            Assert.AreEqual("Dan", check);
        }

        [TestMethod]
        public void TestTimeOfTrip()
        {
            FileReader reader = new FileReader();

            string line = reader.TestReadFile();

            double check = reader.TimeOfTrip("Trip Lauren 12:01 13:16 42.0");

            Assert.AreEqual(1.25, check);
        }

        [TestMethod]
        public void TestDistanceOfTrip()
        {
            FileReader reader = new FileReader();

            string line = reader.TestReadFile();

            double check = reader.DistanceOfTrip("Trip Lauren 12:01 13:16 42.0");

            Assert.AreEqual(42, check);
        }


        [TestMethod]
        public void TestReadFile()
        {
            FileReader reader = new FileReader();
            IList<string> test = reader.ReadFile(filePath);
            Assert.AreEqual("Driver Kumi", test[2]);
        }



    }
}
