using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Http;
using System.Web;
using ServiceStack;
using Newtonsoft.Json;
using System.Net;

namespace WebApplication.Web.Controllers
{
    public class ComicBookController : Controller
    {
        public ICollectionDAL collectionDAL;
        public IStatisticsDAL statisticsDAL;
        public IComicDAL comicDAL;
        public IUserDAL userDAL;


        public ComicBookController(ICollectionDAL collectionDAL, IStatisticsDAL statisticsDAL, IComicDAL comicDAL, IUserDAL userDAL)
        {
            this.collectionDAL = collectionDAL;
            this.statisticsDAL = statisticsDAL;
            this.comicDAL = comicDAL;
            this.userDAL = userDAL;

        }

        public IActionResult Index(string userName)
        {
            int userId = userDAL.GetUser(userName).Id;
            IList<Collection> collections = collectionDAL.GetAllCollectionsFromUserAnon(userId);


            return View(collections);
        }

        [HttpGet]
        public IActionResult NewCollection()
        {
            Collection collection = new Collection();
            return View(collection);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewCollection(string collectionName, bool publicStatus)
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            collectionDAL.CreateCollection(userid.Value, collectionName, publicStatus);
            return RedirectToAction("Index", new { userId = userid.Value });
        }


        public IActionResult CollectionDetails(int collectionId)
        {
            List<Comic> comics = collectionDAL.GetAllComicsInACollection(collectionId);
            Collection collection = collectionDAL.GetCollectionData(collectionId);
            collection.ComicsInCollection = comics;
            HttpContext.Session.SetInt32("collectionid", collection.CollectionId);

            ViewBag.NumOfComics = statisticsDAL.NumberOfComics(collectionId);
            ViewBag.TopHeroes = statisticsDAL.TopHeroes(collectionId);
            ViewBag.TopPublishers = statisticsDAL.TopPublishers(collectionId);
            ViewBag.TopWriters = statisticsDAL.TopWriters(collectionId);

            return View(collection);
        }

        public IActionResult ComicDetails(int comicId)
        {
            List<string> characters = comicDAL.GetAllCharactersInAComic(comicId);
            Comic comic = comicDAL.GetComicData(comicId);
            comic.Characters = characters;
            comic.ImageUrl = SearchAPI(comic.Title);
            return View(comic);
        }


        [HttpGet]
        public IActionResult NewComic()
        {
            Comic comic = new Comic();
            return View(comic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewComic(string author, string title, DateTime publishDate, string description, string publisher, HashSet<string> characters)
        {
            int? userId = HttpContext.Session.GetInt32("userid");
            int? collectionId = HttpContext.Session.GetInt32("collectionid");


            comicDAL.CreateComic(userId.Value, collectionId.Value, author, title, publishDate, description, publisher, characters);
            return RedirectToAction("AccountCollectionDetails", new { collectionId });
        }



        public string SearchAPI(string searchName)
        {
            var image = "";
            try
            {
                string search = "https://comicvine.gamespot.com/api/volumes/?api_key=305fcf12dc67d4234ae52b92f6beddc9856f9fca&format=json&sort=name:asc&filter=name:" + searchName;

                var result = search.GetJsonFromUrl();
                dynamic jsonresult = JsonConvert.DeserializeObject(result);
                 image = jsonresult.results[0].image.small_url;
            }
            catch(Exception ex)
            {
                image = "../images/comic-book-guy-d0bf1078-f9f0-418c-a726-bda4f7929b7-resize-750.jpg";
            }
            return image;

        }

    }
}