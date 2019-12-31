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
    public class ModelTests
    {

        [TestMethod]
        public void TestDriverModelAverageMPH()
        {
            Driver test = new Driver();

            test.DistanceDriven = 203.4;
            test.TimeDriven = 3.25;
            double result = test.AvgMPH;

            Assert.AreEqual(63, result);
        }

        [TestMethod]
        public void TestIntDistanceDriven()
        {
            Driver test = new Driver();
            test.DistanceDriven = 35.6667;
            int result = test.IntDistanceDriven;

            Assert.AreEqual(36, result);
        }

        [TestMethod]
        public void TestDriverModelRounding()
        {
            Driver test = new Driver();

            test.DistanceDriven = 100.00;
            test.TimeDriven = 1.00;
            int result = test.AvgMPH;

            Assert.AreEqual(100, result);
        }

    }
}
