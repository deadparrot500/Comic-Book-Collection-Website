using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class Comic
    {
        public int ComicId { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime PublishDate { get; set;}
        public string Description { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public List<string> Characters { get; set; }
        public string SearchCharacter { get; set; }
        public Collection Collection { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
    }
}
