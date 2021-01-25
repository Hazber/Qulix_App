using System.Web.Mvc;

namespace QulixApp.HtmlAttribute
{
    public class ReferrerHoldAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var referrer = filterContext.RequestContext.HttpContext.Request.UrlReferrer;
            if (referrer != null) filterContext.RouteData.Values.Add("referrer", referrer);
            base.OnActionExecuting(filterContext);
           
        }
    }
}