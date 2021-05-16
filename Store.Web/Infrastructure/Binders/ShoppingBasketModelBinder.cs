using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Lib.Entities;

namespace Store.Web.Infrastructure.Binders
{
    public class ShoppingBasketModelBinder : IModelBinder
    {
        private const string session_key = "Shopping";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            //getting object from session
            ShoppingBasket shoppingBasket = null;
            if(controllerContext.HttpContext.Session != null)
            {
                shoppingBasket = (ShoppingBasket)controllerContext.HttpContext.Session[session_key];
            }

            //making object if there is no object int the session
            if(shoppingBasket == null)
            {
                shoppingBasket = new ShoppingBasket();
                if(controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[session_key] = shoppingBasket;
                }
            }
            return shoppingBasket;
        }
    }
}