﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxo.BLL.ViewModels
{
    public class BlogsVM
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
