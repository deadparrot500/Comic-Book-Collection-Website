using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Providers.Auth
{
    /// <summary>
    /// The authorization filter is used to indicate whether a controller action needs
    /// to have the user authenticated and if they need to meet certain roles.
    /// </summary>
    public class AuthorizationFilterAttribute : Attribute, IActionFilter
    {
        private string[] roles;

        public AuthorizationFilterAttribute(params string[] roles)
        {
            this.roles = roles;
        }

        /// <summary>
        /// Called after the action executes.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        { }

        /// <summary>
        /// Called before the action executes.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the authentication provider. Attributes don't support constructor injection
            var authProvider = context.HttpContext.RequestServices.GetService<IAuthProvider>();

            // If they aren't logged in, force them to login first.
            if (!authProvider.IsLoggedIn)
            {                
                context.Result = new RedirectToRouteResult(new
                {
                    controller = "account",
                    action = "login",                    
                });
                return;
            }

            // If they are logged in and the user doesn't have any of the roles
            // give them a 403
            if (roles.Length > 0 && !authProvider.UserHasRole(roles))
            {
                // User shouldn't have access
                context.Result = new StatusCodeResult(403);
                return;
            }

            // If our code gets this far then the filter "lets them through".
        }
    }
}
