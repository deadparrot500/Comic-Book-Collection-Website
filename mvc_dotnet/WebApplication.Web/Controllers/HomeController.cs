using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;

namespace WebApplication.Web.Controllers
{
    public class HomeController : Controller
    {
        public ICollectionDAL collectionDAL;
        public IStatisticsDAL statisticsDAL;
        public IComicDAL comicDAL;
    
        public HomeController(ICollectionDAL collectionDAL, IStatisticsDAL statisticsDAL, IComicDAL comicDAL)
        {
            this.collectionDAL = collectionDAL;
            this.statisticsDAL = statisticsDAL;
            this.comicDAL = comicDAL;
            
        }

        public IActionResult Index()
        {
            IList<Collection> homeList = collectionDAL.GetTopPublicCollections();

            ViewBag.NumOfComicsALL = statisticsDAL.NumberofComicsALL();
            ViewBag.TopHeroesALL = statisticsDAL.TopHeroesALL();
            ViewBag.TopPublishersALL = statisticsDAL.TopPublishersALL();
            ViewBag.TopWritersALL = statisticsDAL.TopWritersALL();

            return View(homeList);
        }

        public IActionResult SearchComic(string search)
        {
            IList<Comic> searchResult = comicDAL.SearchCharacter(search);

            return View(searchResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
