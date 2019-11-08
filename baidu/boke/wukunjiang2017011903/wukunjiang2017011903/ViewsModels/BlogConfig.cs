using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wukunjiang2017011903.ViewsModels
{
    public class BlogConfig
    {
        [Required]
        [StringLength(50)]
        public string Name { set; get; }

        [StringLength(200)]
        public string Sign { set; get; }

        [Required]
        [StringLength(500)]
        public string Notic { set; get; }
    }
}