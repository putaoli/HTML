using System.Web;
using System.Web.Mvc;

namespace wukunjiang2017011903
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
