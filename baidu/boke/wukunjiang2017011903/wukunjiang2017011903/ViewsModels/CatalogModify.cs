﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wukunjiang2017011903.ViewsModels
{
    public class CatalogModify
    {
        public int Id { set; get; }
        [Required]
        [StringLength(50)]
        public string Name { set; get; }
    }
}