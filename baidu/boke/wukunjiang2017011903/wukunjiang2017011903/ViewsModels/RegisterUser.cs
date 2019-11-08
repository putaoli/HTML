using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wukunjiang2017011903.ViewsModels
{
    public class RegisterUser
    {
        [Required]
        [StringLength(maximumLength: 16, MinimumLength = 6)]
        public string Name { set; get; }

        [StringLength(maximumLength: 16, MinimumLength = 6)]
        public string Pwd { set; get; }

        [StringLength(maximumLength: 16, MinimumLength = 6)]
        [Compare("Pwd")]
        public string CPwd { set; get; }
        [Required]
        [StringLength(4)]
        public string Code { set; get; }
    }
}