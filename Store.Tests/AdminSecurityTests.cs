 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using Store.Web.Controllers;
using Store.Web.Infrastructure.Abstract;
using Store.Web.Models;


namespace Store.Tests
{
     
    [TestClass]
    public class AdminSecurityTests
    {
        
        [TestMethod]
        public void Login_With_Correct_Data()
        {
            //arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "123")).Returns(true);

            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "123"
            };

            AccountController controller = new AccountController(mock.Object);

            //act
            ActionResult result = controller.Login(model, "/MyURL");

            //assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Login_With_Incorrect_Data()
        {
            //arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("user", "228")).Returns(false);

            LoginViewModel model = new LoginViewModel
            {
                UserName = "user",
                Password = "228"
            };

            AccountController controller = new AccountController(mock.Object);

            //act
            ActionResult result = controller.Login(model, "/MyURL");

            //assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
