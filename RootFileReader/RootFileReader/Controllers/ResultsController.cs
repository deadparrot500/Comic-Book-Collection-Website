using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RootFileReader.DAL;
using RootFileReader.Models;

namespace RootFileReader.Controllers
{
    public class ResultsController : Controller
    {
        IList<Driver> driverList = new List<Driver>();
        IList<string> driverNameList = new List<string>();

        [HttpPost]
        public IActionResult Index(string filePath)
        {
            if (filePath == null)
            {
                return NotFound();
            }
            else
            {           
            CheckFile(filePath);

            IList<Driver> sortedDriverList = driverList.OrderByDescending(driver => driver.IntDistanceDriven).ToList();

            return View(sortedDriverList);
            }
        }

        public void CheckFile(string filePath)
        {
            FileReader reader = new FileReader();
            IList<string> lines = reader.ReadFile(filePath);
            foreach (string line in lines)
            {
                string command = reader.FirstWord(line);
                string second = reader.SecondWord(line);
                switch (command)
                {
                    case "Driver":
                        AddDriver(second);
                        break;

                    case "Trip":
                        int driverPosition = driverNameList.IndexOf(second);
                        double time = reader.TimeOfTrip(line);
                        double distance = reader.DistanceOfTrip(line);
                        AddTrip(line, second, driverPosition, time, distance);
                        break;

                    default:
                        break;
                }
            }
        }

        public void AddDriver(string driverName)
        {
            int driverPosition = 0;
            if (driverNameList.Contains(driverName))
            {
                driverPosition = driverNameList.IndexOf(driverName);
            }
            else
            {
                driverNameList.Add(driverName);
                Driver newDriver = new Driver();
                newDriver.DriverPosition = driverNameList.IndexOf(driverName);
                newDriver.DriverName = driverName;
                driverList.Add(newDriver);

            }

        }

        public void AddTrip(string line, string second, int position, double time, double distance)
        {
            if (!driverNameList.Contains(second))
            {
                AddDriver(second);
                position = driverNameList.IndexOf(second);
            }

            double avgMPHThisTrip = distance / time;

            if (avgMPHThisTrip >= 5 && avgMPHThisTrip <= 100)
            {
                driverList[position].TimeDriven += time;
                driverList[position].DistanceDriven += distance;
            }

        }


    }
}