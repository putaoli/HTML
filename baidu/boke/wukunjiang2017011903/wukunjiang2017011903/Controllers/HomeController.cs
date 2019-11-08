using wukunjiang2017011903.App_Code;
using wukunjiang2017011903.Models;
using wukunjiang2017011903.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wukunjiang2017011903.Controllers
{
    public class HomeController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var model = new Tool().DeSerialize<BlogConfig>();
            ViewBag.Config = model;
        }

        // GET: Home
        public ActionResult Index()
        {
            var currentLoginUser = Session["loginuser"] == null ? null : (User)Session["loginuser"];
            ViewBag.CurrentUser = currentLoginUser;
            return View();
        }

        public ActionResult Blog(int id)
        {
            using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
            {
                var model = dbContext.Blogs.FirstOrDefault(m => m.Id == id);
                return View(model);
            }
        }

        public FileResult ValidCode()
        {
            ValidateCode vc = new ValidateCode();
            string code = vc.CreateValidateCode(4);
            Session["validatecode"] = code;
            byte[] bytes = vc.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}