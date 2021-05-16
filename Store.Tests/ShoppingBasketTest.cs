using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Store.Lib.Abstract;
using Store.Lib.Entities;
using Store.Web.Controllers;
using Store.Web.Models;
using Store.Web.HtmlHelpers;
using System.Web.Mvc;
using System;

namespace Store.Tests
{
    [TestClass]
    public class ShoppingBasketTest
    {
        [TestMethod]
        public void Add_Item()
        {
            //arrange
            Part part1 = new Part { Part_Id = 1, Name = "Part1" };
            Part part2 = new Part { Part_Id = 2, Name = "Part2" };

            ShoppingBasket basket_lines = new ShoppingBasket();

            //action
            basket_lines.Add_Item(part1, 1);
            basket_lines.Add_Item(part2, 1);
            List<Basket_Line> rez = basket_lines.Lines.ToList();

            //assert
            Assert.AreEqual(rez.Count(), 2);
            Assert.AreEqual(rez[0].Part, part1);
            Assert.AreEqual(rez[1].Part, part2);
        }

        [TestMethod]
        public void Add_Item_to_Existing()
        {
            //arrange
            Part part1 = new Part { Part_Id = 1, Name = "Part1" };
            Part part2 = new Part { Part_Id = 2, Name = "Part2" };

            ShoppingBasket basket_lines = new ShoppingBasket();

            //act
            basket_lines.Add_Item(part1, 1);
            basket_lines.Add_Item(part2, 1);
            basket_lines.Add_Item(part1, 5);
            List<Basket_Line> rez = basket_lines.Lines.OrderBy(b => b.Part.Part_Id).ToList();

            //assert
            Assert.AreEqual(rez.Count(), 2);
            Assert.AreEqual(rez[0].Quantity, 6);
            Assert.AreEqual(rez[1].Quantity, 1);

        }

        [TestMethod]
        public void Remove_Item()
        {
            //arrange
            Part part1 = new Part { Part_Id = 1, Name = "Part1" };
            Part part2 = new Part { Part_Id = 2, Name = "Part2" };
            Part part3 = new Part { Part_Id = 3, Name = "Part3" };

            ShoppingBasket basket_lines = new ShoppingBasket();

            basket_lines.Add_Item(part1, 1);
            basket_lines.Add_Item(part2, 4);
            basket_lines.Add_Item(part3, 2);
            basket_lines.Add_Item(part2, 1);

            //act
            basket_lines.Remove_Item(part2);

            //assert
            Assert.AreEqual(basket_lines.Lines.Where(b => b.Part == part2).Count(), 0);
            Assert.AreEqual(basket_lines.Lines.Count(), 2);
        }

        [TestMethod]
        public void Total_Value()
        {
            //arrange
            Part part1 = new Part { Part_Id = 1, Price =30 };
            Part part2 = new Part { Part_Id = 2, Price =100 };

            ShoppingBasket basket_lines = new ShoppingBasket();

            //act
            basket_lines.Add_Item(part1, 1);
            basket_lines.Add_Item(part2, 1);
            basket_lines.Add_Item(part1, 3);
            decimal rez = basket_lines.Total_Value();

            //assert
            Assert.AreEqual(rez, 220);
        }

        [TestMethod]
        public void Clear()
        {

            //arrange
            Part part1 = new Part { Part_Id = 1, Price = 30 };
            Part part2 = new Part { Part_Id = 2, Price = 100 };

            ShoppingBasket basket_lines = new ShoppingBasket();

            //act
            basket_lines.Add_Item(part1, 1);
            basket_lines.Add_Item(part2, 1);
            basket_lines.Add_Item(part1, 3);
            basket_lines.Clear();

            //assert
            Assert.AreEqual(basket_lines.Lines.Count(), 0);
        }
        [TestMethod]
        public void Add_To_Basket()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part {Part_Id = 1, Name = "Part1", Description = "Des1"},
            }.AsQueryable());

            ShoppingBasket shoppingBasket = new ShoppingBasket();
            ShoppingBasketController controller = new ShoppingBasketController(mock.Object, null);

            //act
            controller.AddToBacket(shoppingBasket, 1, null);

            //assert
            Assert.AreEqual(shoppingBasket.Lines.Count(), 1);
            Assert.AreEqual(shoppingBasket.Lines.ToList()[0].Part.Part_Id, 1);
        }
        //After adding the game to the cart, there should be a redirect to the cart page
        [TestMethod]
        public void AddingPart_Goes_To_Screen()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part {Part_Id = 1, Name = "Part1", Description = "Des1"},
            }.AsQueryable());

            ShoppingBasket shoppingBasket = new ShoppingBasket();
            ShoppingBasketController controller = new ShoppingBasketController(mock.Object, null);

            //act
            RedirectToRouteResult rez = controller.AddToBacket(shoppingBasket, 2, "myUrl");

            //assert
            Assert.AreEqual(rez.RouteValues["action"], "Index");
            Assert.AreEqual(rez.RouteValues["return_Url"], "myUrl");
        }
        //Checking URL
        [TestMethod]
        public void View_Shopping_Contents()
        {
            //arrange
            ShoppingBasket shoppingBasket = new ShoppingBasket();

            ShoppingBasketController controller = new ShoppingBasketController(null,null);

            //act
            ShoppingBasketIndexViewModel rez = (ShoppingBasketIndexViewModel)controller.Index(shoppingBasket, "myUrl")
                .ViewData.Model;

            //assert
            Assert.AreSame(rez.Basket, shoppingBasket);
            Assert.AreEqual(rez.ReturnUrl, "myUrl");
        }

        // Check if user filled all data for  shipping
        [TestMethod]
        public void Checkout_Empty_Basket()
        {
            //arrange
            Mock<IOrder> mock = new Mock<IOrder>();

            ShoppingBasket basket = new ShoppingBasket();

            ShippingDetails details = new ShippingDetails();

            ShoppingBasketController controller = new ShoppingBasketController(null, mock.Object);

            //act
            ViewResult result = controller.Checkout(basket, details);

            //assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShoppingBasket>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        //Check invalid shipping details
        [TestMethod]
        public void Checkout_Invalid_ShippingDetails()
        {
            //arrange
            Mock<IOrder> mock = new Mock<IOrder>();

            ShoppingBasket basket = new ShoppingBasket();
            basket.Add_Item(new Part(), 1);

            ShoppingBasketController controller = new ShoppingBasketController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            //act
            ViewResult result = controller.Checkout(basket, new ShippingDetails());

            //assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShoppingBasket>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        //Check order
        [TestMethod]
        public void Checkout_And_Sumbit_Order()
        {
            //arrange
            Mock<IOrder> mock = new Mock<IOrder>();

            ShoppingBasket basket = new ShoppingBasket();
            basket.Add_Item(new Part(), 1);

            ShoppingBasketController controller = new ShoppingBasketController(null, mock.Object);

            //act
            ViewResult result = controller.Checkout(basket, new ShippingDetails());

            //arrange
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShoppingBasket>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}
