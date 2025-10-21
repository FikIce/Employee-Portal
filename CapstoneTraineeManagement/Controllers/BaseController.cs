using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CapstoneTraineeManagement.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // This is your existing security check to ensure the user is logged in.
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "User" },
                        { "Action", "Login" }
                    });
            }
            else
            {
                // THIS IS THE NEW CODE TO PREVENT BROWSER CACHING
                // It adds headers to the response that tell the browser not to cache the page.
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
            }

            base.OnActionExecuting(filterContext);
        }
    }
}