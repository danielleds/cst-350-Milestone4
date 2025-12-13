using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Milestone4.Filters
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("User") == null)
            {
                context.Result = new RedirectResult("/User/Index");
            }
        }
    }
}
