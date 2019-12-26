using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Collection
    {
        public string UserName { get; set; }
        public string CollectionName { get; set; }
        public List<Comic> ComicsInCollection {get;set;}
        //tell if the collection is private or public. set to public by default
        public bool PublicStatus { get; set; }
        public int CollectionId { get; set; }
    }
}
