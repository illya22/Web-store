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
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_Pagination()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id=1, Name="Part1"},
                new Part{Part_Id=2, Name="Part2"},
                new Part{Part_Id=3, Name="Part3"},
                new Part{Part_Id=4, Name="Part4"},
                new Part{Part_Id=5, Name="Part5"}
            });
            PartController controller = new PartController(mock.Object);
            controller.pageSize = 2;

            //act
            PartListViewModel rez = (PartListViewModel)controller.List(null, 2).Model;

            //assert
            List<Part> parts = rez.Parts.ToList();
            Assert.IsTrue(parts.Count == 2);
            Assert.AreEqual(parts[0].Name, "Part3");
            Assert.AreEqual(parts[1].Name, "Part4");

        }

        [TestMethod]
        public void TestMethod_PageLinks()
        {
            //arrange
            HtmlHelper helper = null;
            PaginInfo paginInfo = new PaginInfo
            {
                Current_Page = 2,
                Total_Items = 28,
                Items_On_Page = 10
            };

            Func<int, string> func = i => "Page" + i;

            //act
            MvcHtmlString rez = helper.Page_Links(paginInfo, func);

            //assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>", rez.ToString());
        }

        [TestMethod]
        public void TestMethod_PaginationViewModel()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id=1, Name="Part1"},
                new Part{Part_Id=2, Name="Part2"},
                new Part{Part_Id=3, Name="Part3"},
                new Part{Part_Id=4, Name="Part4"},
                new Part{Part_Id=5, Name="Part5"}
            });
            PartController controller = new PartController(mock.Object);
            controller.pageSize = 3;

            //act
            PartListViewModel rez = (PartListViewModel)controller.List(null, 2).Model;

            //assert
            PaginInfo paginInfo = rez.PaginInfo;
            Assert.AreEqual(paginInfo.Current_Page, 2);
            Assert.AreEqual(paginInfo.Items_On_Page, 3);
            Assert.AreEqual(paginInfo.Total_Items, 5);
            Assert.AreEqual(paginInfo.Total_Pages, 2);
        }


        [TestMethod]
        public void TestMethod_Filter()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id=1, Name="Part1", Type="Type1" },
                new Part{Part_Id=2, Name="Part2", Type="Type2"},
                new Part{Part_Id=3, Name="Part3", Type="Type1"},
                new Part{Part_Id=4, Name="Part4", Type="Type2"},
                new Part{Part_Id=5, Name="Part5", Type="Type3"}
            });
            PartController controller = new PartController(mock.Object);
            controller.pageSize = 3;

            //action
            List<Part> rez = ((PartListViewModel)controller.List("Type2", 1).Model).Parts.ToList();

            //asset
            Assert.AreEqual(rez.Count(), 2);
            Assert.IsTrue(rez[0].Name == "Part2" && rez[0].Type == "Type2");
            Assert.IsTrue(rez[1].Name == "Part4" && rez[1].Type == "Type2");
        }


        [TestMethod]
        public void TestMethod_Type()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id=1, Name="Part1", Type="A" },
                new Part{Part_Id=2, Name="Part2", Type="A"},
                new Part{Part_Id=3, Name="Part3", Type="B"},
                new Part{Part_Id=4, Name="Part4", Type="1"},

            });

            NavController nav = new NavController(mock.Object);

            //action
            List<string> rez = ((IEnumerable<string>)nav.Menu().Model).ToList();

            //asset
            Assert.AreEqual(rez.Count(), 3);
            Assert.AreEqual(rez[0], "1");
            Assert.AreEqual(rez[1], "A");
            Assert.AreEqual(rez[2], "B");
        }


        [TestMethod]
        public void TestMethod_SelectedType()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id=1, Name="Part1", Type="A" },
                new Part{Part_Id=3, Name="Part3", Type="B"},
            });

            NavController nav = new NavController(mock.Object);

            string selected_type = "B";

            //action
            string rez = nav.Menu(selected_type).ViewBag.Selected_Type;

            //asset
            Assert.AreEqual(selected_type, rez);
        }

        [TestMethod]
        public void TestMethod_TypeCount()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id=1, Name="Part1", Type="Type1" },
                new Part{Part_Id=2, Name="Part2", Type="Type2"},
                new Part{Part_Id=3, Name="Part3", Type="Type1"},
                new Part{Part_Id=4, Name="Part4", Type="Type2"},
                new Part{Part_Id=5, Name="Part5", Type="Type3"}
            });
            PartController controller = new PartController(mock.Object);
            controller.pageSize = 3;

            //action
            int rez1 = ((PartListViewModel)controller.List("Type1").Model).PaginInfo.Total_Items;
            int rez2 = ((PartListViewModel)controller.List("Type2").Model).PaginInfo.Total_Items;
            int rez3 = ((PartListViewModel)controller.List("Type3").Model).PaginInfo.Total_Items;
            int rezTotal = ((PartListViewModel)controller.List(null).Model).PaginInfo.Total_Items;

            //assert
            Assert.AreEqual(rez1, 2);
            Assert.AreEqual(rez2, 2);
            Assert.AreEqual(rez3, 1);
            Assert.AreEqual(rezTotal, 5);
        }
    }
}
