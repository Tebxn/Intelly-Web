
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Intelly_Web.Entities
{
    public class SecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("UserToken") == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller","Authentication"},
                    { "action","Login"}
                });
            }

            base.OnActionExecuting(context);
        }

        public class SecurityFilterIsAdmin : ActionFilterAttribute
        {

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                string var = context.HttpContext.Session.GetString("UserIsAdmin");
                if (var != "True")
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                {"controller", "Home" },
                {"action", "Index" }
            });
                }

                base.OnActionExecuting(context);
            }


        }

    }
}
