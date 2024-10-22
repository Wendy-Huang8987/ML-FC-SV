using System.Globalization;
using System.Threading;
using System.Web.Mvc;

public class LanguageFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var lang = filterContext.HttpContext.Session["Language"] as string;
        if (!string.IsNullOrEmpty(lang))
        {
            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
