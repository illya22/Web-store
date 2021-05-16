using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Lib.Entities;
using Store.Lib.Abstract;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class ShoppingBasketController : Controller
    {
        private IPartRepository repository;
        private IOrder order;

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(ShoppingBasket basket, ShippingDetails details)
        {
            if(basket.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Your shopping basket is empty");
            }

            if(ModelState.IsValid)
            {
                order.ProcessOrder(basket, details);
                basket.Clear();
                return View("Completed");
            }
            else
            {
                return View(details);
            }
        }

        public ViewResult Index(ShoppingBasket shoppingBasket, string return_Url)
        {
            return View(new ShoppingBasketIndexViewModel
            {
                Basket = shoppingBasket,
                ReturnUrl = return_Url
            });
        }

        public ShoppingBasketController(IPartRepository repo, IOrder processor)
        {
            repository = repo;
            order = processor;
        }

        public RedirectToRouteResult AddToBacket(ShoppingBasket shoppingBasket,int part_id, string return_Url)
        {
            Part part = repository.Parts
                .FirstOrDefault(p => p.Part_Id == part_id);

            if(part != null)
            {
                shoppingBasket.Add_Item(part, 1);
            }
            return RedirectToAction("Index", new { return_Url });
        }

        public RedirectToRouteResult RemoveFromBasket(ShoppingBasket shoppingBasket, int part_id, string return_Url)
        {
            Part part = repository.Parts
                .FirstOrDefault(p => p.Part_Id == part_id);

            if( part != null)
            {
                shoppingBasket.Remove_Item(part);
            }
            return RedirectToAction("Index", new { return_Url });
        }

         public PartialViewResult Summary(ShoppingBasket shoppingBasket)
        {
            return PartialView(shoppingBasket);
        }

    }
}