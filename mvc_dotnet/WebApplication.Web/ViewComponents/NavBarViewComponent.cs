using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.ViewComponents
{
    /// <summary>
    /// A view component is a reusable or "isolated" piece of our app.
    /// It cannot be navigated to via URL like a controller.
    /// </summary>
    public class NavBarViewComponent : ViewComponent
    {
        // Components allow dependency injection just like controllers.
        private IAuthProvider authProvider;
        public NavBarViewComponent(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        /// <summary>
        /// This is the method that is invoked when the component is told to "render".
        /// </summary>
        /// <returns></returns>
        public IViewComponentResult Invoke()
        {
            var user = authProvider.GetCurrentUser();
            return View("_NavBar", user);
        }
    }
}
