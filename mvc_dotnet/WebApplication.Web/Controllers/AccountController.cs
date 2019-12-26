using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceStack;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthProvider authProvider;
        public ICollectionDAL collectionDAL;
        public IComicDAL comicDAL;
        public IStatisticsDAL statisticsDAL;

        public AccountController(IAuthProvider authProvider, ICollectionDAL collectionDAL, IComicDAL comicDAL, IStatisticsDAL statisticsDAL)
        {
            this.authProvider = authProvider;
            this.collectionDAL = collectionDAL;
            this.comicDAL = comicDAL;
            this.statisticsDAL = statisticsDAL;
        }

        //[AuthorizationFilter] // actions can be filtered to only those that are logged in
        [AuthorizationFilter("Admin", "Author", "UserP", "UserS")]  //<-- or filtered to only those that have a certain role
        [HttpGet]
        public IActionResult Index()
        {
            var user = authProvider.GetCurrentUser();
            HttpContext.Session.SetInt32("userid", user.Id);
            HttpContext.Session.SetString("userRole", user.Role);
            List<Collection> collections = collectionDAL.GetAllCollectionsFromUser(user.Id);
            return View(collections);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            // Ensure the fields were filled out
            if (ModelState.IsValid)
            {
                // Check that they provided correct credentials
                bool validLogin = authProvider.SignIn(loginViewModel.Email, loginViewModel.Password);
                if (validLogin)
                {
                    var user = authProvider.GetCurrentUser();
                    HttpContext.Session.SetInt32("userid", user.Id);
                    HttpContext.Session.SetString("userRole", user.Role);
                    // Redirect the user where you want them to go after successful login
                    return RedirectToAction("Index", "Account");
                }
            }

            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult LogOff()
        {
            // Clear user from session
            authProvider.LogOff();
            HttpContext.Session.SetInt32("userid", 0);
            HttpContext.Session.SetString("userRole", "");
            // Redirect the user where you want them to go after logoff
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Register them as a new user (and set default role)
                // When a user registeres they need to be given a role. If you don't need anything special
                // just give them "UserS".
                authProvider.Register(registerViewModel.Email, registerViewModel.Password, registerViewModel.UserType);



                // Redirect the user where you want them to go after registering
                return RedirectToAction("Index", "Account");
            }

            return View(registerViewModel);
        }

        public IActionResult AccountCollectionDetails(int collectionId)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                List<Comic> comics = collectionDAL.GetAllComicsInACollection(collectionId);
                Collection collection = collectionDAL.GetCollectionData(collectionId);
                collection.ComicsInCollection = comics;
                HttpContext.Session.SetInt32("collectionid", collection.CollectionId);

                ViewBag.NumOfComics = statisticsDAL.NumberOfComics(collectionId);
                ViewBag.TopHeroes = statisticsDAL.TopHeroes(collectionId);
                ViewBag.TopPublishers = statisticsDAL.TopPublishers(collectionId);
                ViewBag.TopWriters = statisticsDAL.TopWriters(collectionId);

                int collectionAmount = comicDAL.CheckNumberOfComicsInCollection(collectionId);

                string userRole = HttpContext.Session.GetString("userRole");

                ViewBag.collectionAmount = collectionAmount;
                ViewBag.userRole = userRole;


                return View(collection);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult AccountComicDetails(int comicId)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                List<string> characters = comicDAL.GetAllCharactersInAComic(comicId);
                Comic comic = comicDAL.GetComicData(comicId);
                comic.Characters = characters;
                comic.ImageUrl = SearchAPI(comic.Title);
                return View(comic);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult NewCollection()
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {

                Collection collection = new Collection();
                return View(collection);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewCollection(string collectionName, bool publicStatus)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                int? userid = HttpContext.Session.GetInt32("userid");
                collectionDAL.CreateCollection(userid.Value, collectionName, publicStatus);
                return RedirectToAction("Index", new { userId = userid.Value });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult NewComic()
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {

                Comic comic = new Comic();
                return View(comic);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewComic(string author, string title, DateTime publishDate, string description, string publisher, HashSet<string> characters)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {

                int? userId = HttpContext.Session.GetInt32("userid");
                int? collectionId = HttpContext.Session.GetInt32("collectionid");


                comicDAL.CreateComic(userId.Value, collectionId.Value, author, title, publishDate, description, publisher, characters);
                return RedirectToAction("AccountCollectionDetails", new { collectionId });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult DeleteCharacter(string characterName, int comicId)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                int characterId = comicDAL.CheckForValue("characters", "character_name", characterName);
                comicDAL.DeleteCharacter(characterId, comicId);
                return RedirectToAction("AccountComicDetails", new { comicId });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpGet]
        public IActionResult AddCharacter(int comicId)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                Comic comic = comicDAL.GetComicData(comicId);
                return View(comic);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult AddCharacter(string searchCharacter, int comicId, string publisher)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                int publisherId = comicDAL.CheckForValue("publisher", "publisher_name", publisher);
                int characterId = comicDAL.CreateCharacter(searchCharacter, publisherId);
                int checkChar = comicDAL.CheckForCharacterInComic(characterId, comicId);
                if (checkChar == 0)
                {
                    comicDAL.AddCharacterToComic(characterId, comicId);
                }
                return RedirectToAction("AccountComicDetails", new { comicId });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult DeleteComic(int comicId, int collectionId)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                List<string> characters = comicDAL.GetAllCharactersInAComic(comicId);
                foreach (string character in characters)
                {
                    int characterId = comicDAL.CheckForValue("characters", "character_name", character);
                    comicDAL.DeleteCharacter(characterId, comicId);
                }

                comicDAL.DeleteComicFromCollection(comicId, collectionId);

                return RedirectToAction("AccountCollectionDetails", new { collectionId });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult DeleteCollection(int collectionId)
        {
            var user = authProvider.GetCurrentUser();

            int? check = HttpContext.Session.GetInt32("userid");

            if (user != null && check.Value == user.Id)
            {
                IList<Comic> comics = collectionDAL.GetAllComicsInACollection(collectionId);
                foreach (Comic comic in comics)
                {
                    List<string> characters = comicDAL.GetAllCharactersInAComic(comic.ComicId);
                    foreach (string character in characters)
                    {
                        int characterId = comicDAL.CheckForValue("characters", "character_name", character);
                        comicDAL.DeleteCharacter(characterId, comic.ComicId);
                    }

                    comicDAL.DeleteComicFromCollection(comic.ComicId, collectionId);
                }

                collectionDAL.DeleteCollection(collectionId);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
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
            catch (Exception ex)
            {
                image = "../images/comic-book-guy-d0bf1078-f9f0-418c-a726-bda4f7929b7-resize-750.jpg";
            }
            return image;

        }
    }
}