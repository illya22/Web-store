using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Web.Models
{
    public class PaginInfo
    {
        public int Total_Items { get; set; }
        public int Items_On_Page { get; set; }
        public int Current_Page { get; set; }
        public int Total_Pages
        { get { return (int)Math.Ceiling((decimal)Total_Items / Items_On_Page); } }
    }
}