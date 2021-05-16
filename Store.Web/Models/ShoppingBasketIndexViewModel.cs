using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Store.Lib.Entities;

namespace Store.Web.Models
{
    public class ShoppingBasketIndexViewModel
    {
        public ShoppingBasket Basket { get; set; }
        public string ReturnUrl { get; set; }
    }
}