using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RootFileReader.Models;

namespace RootFileReader.DAL
{
    public class FileReader
    {

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


        public string TestReadFile()
        {
            string result = "";
            string filePath = "C:\\Users\\deadp\\TE-Work\\After Graduation\\RootFileReader\\TestInput.txt";
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {

                    result = sr.ReadLine();

                }

            }
            catch { }

            return result;

        }
        
    }
}
