using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Milestone4.Filters
{
    // Active games are stored within sessions on a per-user basis;
    // this filter checks if the game exists before proceedding with the action.
    public class GameCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Game") == null)
            {
                context.Result = new RedirectResult("/Minesweeper/StartGame");
            }
        }
    }
}
