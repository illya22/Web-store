using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Web.Mvc;
using Store.Lib.Abstract;
using Store.Lib.Entities;
using Store.Web.Controllers;


namespace Store.Tests
{
     
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Index_Contains_All_Parts()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id =1, Name = "Part1"},
                new Part{Part_Id =2, Name = "Part2"},
                new Part{Part_Id =3, Name = "Part3"},
                new Part{Part_Id =4, Name = "Part4"},
                new Part{Part_Id =5, Name = "Part5"}
            });

            AdminController controller = new AdminController(mock.Object);

            //act
            List<Part> result = ((IEnumerable<Part>)controller.Index().ViewData.Model).ToList();

            //assert
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Part1", result[0].Name);
            Assert.AreEqual("Part2", result[1].Name);
            Assert.AreEqual("Part3", result[2].Name);
        }

         [TestMethod]
         public void Edit_Part()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id =1, Name = "Part1"},
                new Part{Part_Id =2, Name = "Part2"},
                new Part{Part_Id =3, Name = "Part3"},
                new Part{Part_Id =4, Name = "Part4"},
                new Part{Part_Id =5, Name = "Part5"}
            });

            AdminController controller = new AdminController(mock.Object);

            //act
            Part part1 = controller.Edit(1).ViewData.Model as Part;
            Part part2 = controller.Edit(2).ViewData.Model as Part;
            Part part3 = controller.Edit(3).ViewData.Model as Part;

            //assert
            Assert.AreEqual(1, part1.Part_Id);
            Assert.AreEqual(2, part2.Part_Id);
            Assert.AreEqual(3, part3.Part_Id);
         }
        
        [TestMethod]
        public void Edit_Nonexistent_Part()
        {
            //arrange
            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id =1, Name = "Part1"},
                new Part{Part_Id =2, Name = "Part2"},
                new Part{Part_Id =3, Name = "Part3"},
                new Part{Part_Id =4, Name = "Part4"},
                new Part{Part_Id =5, Name = "Part5"}
            });

            AdminController controller = new AdminController(mock.Object);

            //act
            Part result = controller.Edit(6).ViewData.Model as Part;

        }

        [TestMethod]
        public void Save_Valid_Changes()
        {
            //assert
            Mock<IPartRepository> mock = new Mock<IPartRepository>();

            AdminController controller = new AdminController(mock.Object);

            Part part = new Part { Name = "Test" };

            //act
            ActionResult result = controller.Edit(part);

            //assert
            mock.Verify(m => m.SavePart(part));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Save_Invalid_Changes()
        {
            //assert
            Mock<IPartRepository> mock = new Mock<IPartRepository>();

            AdminController controller = new AdminController(mock.Object);

            Part part = new Part { Name = "Test" };

            controller.ModelState.AddModelError("error", "error");

            //act
            ActionResult result = controller.Edit(part);

            //assert
            mock.Verify(m => m.SavePart(It.IsAny<Part>()), Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Delete_Parts()
        {
            //assert
            Part part = new Part { Part_Id = 2, Name = "Part2" };

            Mock<IPartRepository> mock = new Mock<IPartRepository>();
            mock.Setup(m => m.Parts).Returns(new List<Part>
            {
                new Part{Part_Id =1, Name = "Part1"},
                new Part{Part_Id =2, Name = "Part2"},
                new Part{Part_Id =3, Name = "Part3"},
                new Part{Part_Id =4, Name = "Part4"},
                new Part{Part_Id =5, Name = "Part5"}
            });

            AdminController controller = new AdminController(mock.Object);

            //act
            controller.Delete(part.Part_Id);

            //assert
            mock.Verify(m => m.DeletePart(part.Part_Id));
        }
    }
}
