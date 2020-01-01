using RootFileReader.Models;
using System.Collections.Generic;

namespace RootFileReader.DAL
{
    public interface IFileReader
    {
        IList<Driver> DriverList { get; set; } 
        IList<string> DriverNameList { get; set; }

        IList <Driver> CheckFile(string filePath);

        IList<string> ReadFile(string filePath);

        string FirstWord(string line);

        string SecondWord(string line);

        void AddDriver(string driverName);

        double TimeOfTrip(string line);

        double DistanceOfTrip(string line);

        void AddTrip(string second, int position, double time, double distance);


    }
}
