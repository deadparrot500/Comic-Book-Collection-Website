using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RootFileReader.Models;

namespace RootFileReader.DAL
{
    public class FileReader : IFileReader
    {
        public IList<Driver> DriverList { get { return driverList; } set { value = driverList; } }
        public IList<string> DriverNameList { get { return driverNameList; } set { value = driverNameList; } }

        public IList<Driver> driverList = new List<Driver>();
        public IList<string> driverNameList = new List<string>();


        public IList<Driver> CheckFile(string filePath)
        {
            
            IList<string> lines = ReadFile(filePath);
            foreach (string line in lines)
            {
                string command = FirstWord(line);
                string second = SecondWord(line);
                switch (command)
                {
                    case "Driver":
                        AddDriver(second);
                        break;

                    case "Trip":
                        int driverPosition = driverNameList.IndexOf(second);
                        double time = TimeOfTrip(line);
                        double distance = DistanceOfTrip(line);
                        AddTrip(second, driverPosition, time, distance);
                        break;

                    default:
                        break;
                }
            }

            IList<Driver> sortedDriverList = driverList.OrderByDescending(driver => driver.IntDistanceDriven).ToList();

            return sortedDriverList;
        }


        public IList<string> ReadFile(string filePath)
        {
            IList<string> lines = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        lines.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lines;
        }


        public string FirstWord(string line)
        {
            string[] wordsInLine = line.Split(' ');
            return wordsInLine[0];

        }


        public string SecondWord(string line)
        {
            string[] wordsInLine = line.Split(' ');
            return wordsInLine[1];

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


        public double TimeOfTrip(string line)
        {
            string[] wordsInLine = line.Split(' ');
            return (TimeSpan.Parse(wordsInLine[3]) - TimeSpan.Parse(wordsInLine[2])).TotalHours;
        }


        public double DistanceOfTrip(string line)
        {
            string[] wordsInLine = line.Split(' ');
            return Convert.ToDouble(wordsInLine[4]);
        }


        public void AddTrip(string second, int position, double time, double distance)
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
