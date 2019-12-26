using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Character
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Comic> Appearances { get; set; }

        //CA = "Appearances in Collection"
        public int NumOfAC { get; set; }
    }
}
