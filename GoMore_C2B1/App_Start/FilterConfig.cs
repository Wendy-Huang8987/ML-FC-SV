using System.Web;
using System.Web.Mvc;

namespace GoMore_C2B1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LanguageFilterAttribute());
        }
    }
}
