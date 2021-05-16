using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Lib.Entities;

namespace Store.Web.Models
{
    public class PartListViewModel
    {
        public IEnumerable<Part> Parts { get; set; }
        public PaginInfo PaginInfo { get; set; }
        public string Current_Type { get; set; }
        
    }
}