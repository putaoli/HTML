using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using wukunjiang2017011903.App_Code;
using wukunjiang2017011903.Models;

namespace wukunjiang2017011903
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            using (var blogDbContext = new BlogDbContext())
            {
                if (blogDbContext.Database.CreateIfNotExists())
                {
                    //���ݿ���Ϊ������Ȼ�����´�����
                    //����������Ϳ���ʵ�����ݿ�ĳ�ʼ��
                    for (int i = 0; i < 10; i++)
                    {
                        var catalog = new Catalog();
                        catalog.AddTime = DateTime.Now;
                        catalog.Name = "��ʼ��" + i;
                        catalog.Status = true;
                        int res = 0;
                        blogDbContext.Catalogs.Add(catalog);
                        res = blogDbContext.SaveChanges();
                    }
                }
            }
        }

    }
}
