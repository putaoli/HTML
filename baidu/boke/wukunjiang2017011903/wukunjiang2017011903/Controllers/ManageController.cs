using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wukunjiang2017011903.App_Code;
using wukunjiang2017011903.Models;
using wukunjiang2017011903.ViewsModels;

namespace wukunjiang2017011903.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
                if (!model.Code.Equals(sessionValidCode))
                {
                    return Content("验证码不对");
                }

                var user = new User();
                user.AddTime = DateTime.Now;
                user.Name = model.Name;
                user.Pwd = Tool.GetMD5(model.Pwd);//经过md5的加密
                user.Status = true;
                int res = 0;
                using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
                {
                    dbContext.Users.Add(user);
                    res = dbContext.SaveChanges();//才是真正保存到数据库
                }
                if (res > 0)
                {
                    return Redirect("/");
                }
                else
                {
                    return Content("注册失败");
                }
            }
            else
            {
                return Content("验证失败");
            }

        }//注册页

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
                if (!model.Code.Equals(sessionValidCode))
                {
                    return Content("验证码不对");
                }

                //根据用户名查找实体
                using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
                {
                    var nameUser = dbContext.Users.FirstOrDefault(m => m.Name == model.Name);
                    if (nameUser == null)
                    {
                        return Content("账号或者密码不对");//这里不要提示太明显
                    }
                    else
                    {
                        if (Tool.GetMD5(model.Pwd).Equals(nameUser.Pwd))//让用户输入的密码加密后和查找的实体的密码比较
                        {
                            Session["loginuser"] = nameUser;
                            return Redirect("/");
                        }
                        else
                        {
                            return Content("账号或者密码不对");
                        }
                    }
                }
            }
            else
            {
                return Content("验证失败");
            }
        }//登录页

        [HttpGet]
        public ActionResult Config()
        {
            var model = new Tool().DeSerialize<BlogConfig>();
            return View(model);
            //return View();
        }

        [HttpPost]
        public ActionResult Config(BlogConfig model)
        {
            new Tool().Serialize<BlogConfig>(model);
            return View();
        }//博客配置    
        [HttpGet]
        public ActionResult Catalogadd()
        {
           return View();
        }

        [HttpPost]
        public ActionResult Catalogadd(CatalogAdd model)
        {
            if (ModelState.IsValid)
            {
                var catalog = new Catalog();
                catalog.AddTime = DateTime.Now;
                catalog.Name = model.Name;
                catalog.Status = true;
                int res = 0;
                using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
                {
                    dbContext.Catalogs.Add(catalog);
                    res = dbContext.SaveChanges();//才是真正保存到数据库
                }
                if (res > 0)
                {
                    return Content("添加成功");
                }
                else
                {
                    return Content("添加失败");
                }
            }
            else
            {
                return Content("验证失败");
            }
        }
        [HttpGet]
        public ActionResult CatalogManage()
        {
            return View();
        }

        public JsonResult LoadCatalog()
        {
            using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
            {
                var list = dbContext.Catalogs.ToList().Select(m => new { Id = m.Id, Name = m.Name, AddTime = m.AddTime }).ToList();
                return Json(list);
            }
        }

        [HttpGet]
        public ActionResult CatalogModify(int id)
        {
            using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
            {
                var model = dbContext.Catalogs.FirstOrDefault(m => m.Id == id);
                CatalogModify cm = new ViewsModels.CatalogModify();
                cm.Id = model.Id;
                cm.Name = model.Name;
                return View(cm);
            }
        }

        [HttpPost]
        public ActionResult CatalogModify(CatalogModify model)
        {
            using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
            {

                var odlModel = dbContext.Catalogs.FirstOrDefault(m => m.Id == model.Id);
                odlModel.Name = model.Name;
                DbEntityEntry entry = dbContext.Entry(odlModel);
                entry.State = EntityState.Modified;
                int res = dbContext.SaveChanges();
                if (res > 0)
                {
                    return Content("修改成功");
                }
                else
                {
                    return Content("修改失败");
                }
            }
        }

        public ActionResult CatalogDel(int id)
        {
            using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
            {
                var odlModel = dbContext.Catalogs.FirstOrDefault(m => m.Id == id);
                DbEntityEntry entry = dbContext.Entry(odlModel);
                entry.State = EntityState.Deleted;
                int res = dbContext.SaveChanges();
                if (res > 0)
                {
                    return Content("删除成功");
                }
                else
                {
                    return Content("删除失败");
                }
            }
        }
        public ActionResult BlogAdd()
        {
            using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
            {
                List<SelectListItem> listCatalog = dbContext.Catalogs.ToList().Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
                ViewBag.catalogList = listCatalog;
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BlogAdd(BlogAdd model)
        {
            if (ModelState.IsValid)
            {

                var b = new Blog();
                b.AddTime = DateTime.Now;
                b.Title = model.Title;
                b.catalogId = model.catalogId;
                b.Content = model.Content;
                b.Status = true;
                int res = 0;
                using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
                {
                    dbContext.Blogs.Add(b);
                    res = dbContext.SaveChanges();//才是真正保存到数据库
                }
                if (res > 0)
                {
                    return Content("添加成功");
                }
                else
                {
                    return Content("添加失败");
                }
            }
            else
            {
                return Content("验证不通过");
            }
        }

        //public ActionResult Upload()
        //{
        //    //文件保存目录路径
        //    String savePath = "/attached/";
        //    //文件保存目录URL
        //    String saveUrl = "/attached/";
        //    //定义允许上传的文件扩展名
        //    Hashtable extTable = new Hashtable();
        //    extTable.Add("image", "gif,jpg,jpeg,png,bmp");
        //    extTable.Add("flash", "swf,flv");
        //    extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
        //    extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
        //    //最大文件大小
        //    int maxSize = 1000000;
        //    HttpPostedFileBase imgFile = Request.Files["imgFile"];
        //    if (imgFile == null)
        //    {
        //        return Content("请选择文件。");
        //    }
        //    String dirPath = Server.MapPath(savePath);
        //    if (!Directory.Exists(dirPath))
        //    {
        //        return Content("上传目录不存在。");
        //    }
        //    String dirName = Request.QueryString["dir"];
        //    if (String.IsNullOrEmpty(dirName))
        //    {
        //        dirName = "image";
        //    }
        //    if (!extTable.ContainsKey(dirName))
        //    {
        //        return Content("目录名不正确。");
        //    }
        //    String fileName = imgFile.FileName;
        //    String fileExt = Path.GetExtension(fileName).ToLower();
        //    if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
        //    {
        //        return Content("上传文件大小超过限制。");
        //    }
        //    if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
        //    {
        //        return Content("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
        //    }
        //    //创建文件夹
        //    dirPath += dirName + "/";
        //    saveUrl += dirName + "/";
        //    if (!Directory.Exists(dirPath))
        //    {
        //        Directory.CreateDirectory(dirPath);
        //    }
        //    String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
        //    dirPath += ymd + "/";
        //    saveUrl += ymd + "/";
        //    if (!Directory.Exists(dirPath))
        //    {
        //        Directory.CreateDirectory(dirPath);
        //    }
        //    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
        //    String filePath = dirPath + newFileName;
        //    imgFile.SaveAs(filePath);
        //    String fileUrl = saveUrl + newFileName;
        //    Hashtable hash = new Hashtable();
        //    hash["error"] = 0;
        //    hash["url"] = fileUrl;
        //    return Json(hash);
        //}
        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Admin(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                string sessionValidCode = Session["validatecode"] == null ? string.Empty : Session["validatecode"].ToString();
                if (!model.Code.Equals(sessionValidCode))
                {
                    return Content("验证码不对");
                }

                //根据用户名查找实体
                using (wukunjiang2017011903.App_Code.BlogDbContext dbContext = new App_Code.BlogDbContext())
                {
                    var nameUser = dbContext.Admin.FirstOrDefault(m => m.Name == model.Name);
                    if (nameUser == null)
                    {
                        return Content("账号或者密码不对");//这里不要提示太明显
                    }
                    else
                    {
                        if (Tool.GetMD5(model.Pwd).Equals(nameUser.Pwd))//让用户输入的密码加密后和查找的实体的密码比较
                        {
                            Session["loginuser"] = nameUser;
                            return Redirect("/");
                        }
                        else
                        {
                            return Content("账号或者密码不对");
                        }
                    }
                }
            }
            else
            {
                return Content("验证失败");
            }
        }

    }
}