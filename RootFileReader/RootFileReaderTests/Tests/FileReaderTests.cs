using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RootFileReader.DAL;
using RootFileReader.Models;

namespace RootFileReaderTests
{
    [TestClass]
    public class FileReaderTests
    {
        private string filePath = "C:\\Users\\deadp\\TE-Work\\BitBucket Projects\\RootFileReader\\TestInput.txt";

        [TestMethod]
        public void TestCheckFile()
        {

            IFileReader reader = new FileReader();
            IList<Driver> test = reader.CheckFile(filePath);

            Assert.AreEqual("Lauren", test[0].DriverName);
            Assert.AreEqual(78, test[1].IntDistanceDriven);
        }


        [TestMethod]
        public void TestReadFile()
        {
            IFileReader reader = new FileReader();
            IList<string> test = reader.ReadFile(filePath);
            Assert.AreEqual("Driver Kumi", test[2]);
        }


        [TestMethod]
        public void TestReturnFirstWord()
        {
            IFileReader reader = new FileReader();

            string line = reader.ReadFile(filePath)[0];

            string check = reader.FirstWord(line);

            Assert.AreEqual("Driver", check);
        }


        [TestMethod]
        public void TestReturnSecondWord()
        {
            IFileReader reader = new FileReader();

            string line = reader.ReadFile(filePath)[0];

            string check = reader.SecondWord(line);

            Assert.AreEqual("Dan", check);
        }


        [TestMethod]
        public void TestAddDriver()
        {
            IFileReader reader = new FileReader();

            reader.AddDriver("TestDriver");
                       
            IList<Driver> test = reader.DriverList;
            IList<string> testName = reader.DriverNameList;
            int driverPosition = testName.IndexOf("TestDriver");

            Assert.AreEqual(0, driverPosition);
            Assert.AreEqual("TestDriver", test[0].DriverName);

        }

        
        [TestMethod]
        public void TestTimeOfTrip()
        {
            IFileReader reader = new FileReader();

            string line = reader.ReadFile(filePath)[0];

            double check = reader.TimeOfTrip("Trip Lauren 12:01 13:16 42.0");

            Assert.AreEqual(1.25, check);
        }

        
        [TestMethod]
        public void TestDistanceOfTrip()
        {
            IFileReader reader = new FileReader();

            string line = reader.ReadFile(filePath)[0];

            double check = reader.DistanceOfTrip("Trip Lauren 12:01 13:16 42.0");

            Assert.AreEqual(42, check);
        }


        [TestMethod]
        public void TestAddTrip()
        {

            IFileReader reader = new FileReader();

            reader.AddTrip("Moe", 0, .5, 17.3);
            IList<Driver> test = reader.DriverList;
            IList<string> testName = reader.DriverNameList;
            int driverPosition = testName.IndexOf("Moe");

            Assert.AreEqual(0, driverPosition);
            Assert.AreEqual("Moe", test[0].DriverName);
            Assert.AreEqual(35, test[0].AvgMPH);

        }
    }
}
