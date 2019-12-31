using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RootFileReader.Models
{
    public class Driver
    {
        public string DriverName { get; set; }
        public int DriverPosition { get; set; }
        public double TimeDriven { get; set; } = 0.00;
        public double DistanceDriven { get; set; } = 0.00;
        public int IntDistanceDriven
        {
            get { return Convert.ToInt32(Math.Round(DistanceDriven)); }
            set { value = Convert.ToInt32(Math.Round(DistanceDriven)); }
        }
        public int AvgMPH
        {
            get
            {
                int value;
                if (TimeDriven == 0)
                {
                    value = 0;
                }
                else
                {
                    value = Convert.ToInt32(Math.Round(DistanceDriven / TimeDriven));
                }
                return value;
            }
            set
            {
                if (TimeDriven == 0)
                {
                    value = 0;
                }
                else
                {
                    value = Convert.ToInt32(Math.Round(DistanceDriven / TimeDriven));
                }
            }
        }
     


    }
}
