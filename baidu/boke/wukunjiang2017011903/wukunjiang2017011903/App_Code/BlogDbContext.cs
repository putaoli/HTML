using wukunjiang2017011903.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace wukunjiang2017011903.App_Code
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext ():base("name=dblink")
        {

        }
        public DbSet<User> Users { set; get; }
        public DbSet<Catalog> Catalogs { set; get; }
        public DbSet<Blog> Blogs { set; get; }
        public DbSet<User> Admin { set; get; }
    }
}