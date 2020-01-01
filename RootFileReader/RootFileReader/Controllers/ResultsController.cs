using Microsoft.AspNetCore.Mvc;
using RootFileReader.DAL;


namespace RootFileReader.Controllers
{
    public class ResultsController : Controller
    {
        [HttpPost]
        public IActionResult Index(string filePath)
        {
            IFileReader reader = new FileReader();
            if (filePath == null)
            {
                return NotFound();
            }
            else
            {
                return View(reader.CheckFile(filePath));
            }
        }
    }
}