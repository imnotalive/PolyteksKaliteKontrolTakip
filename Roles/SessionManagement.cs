using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolyteksKaliteKontrolTakip.Roles
{
    public class SessionManagement
    {
    }
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = filterContext.Controller as Controller;
            HttpContext httpContext = HttpContext.Current;
            if (controller != null)
            {
                if (HttpContext.Current.Session["Giris"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Home/Giris");
                    return;
                }
            }
            base.OnActionExecuting(filterContext);

        }
    }
}