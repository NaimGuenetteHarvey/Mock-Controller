using mock.depart.Controllers;
using mock.depart.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;   // pour NotFoundResult
using mock.depart.Models;
namespace TestProject1
{
    [TestClass]
    public sealed class Test1
    {
        private Mock<CatsService> serviceMock = null!;
        private Mock<CatsController> controller = null!;
        private const string USER_ID = "11111";
        [TestInitialize]
        public void Init()
        {
            serviceMock = new Mock<CatsService>();
            controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };
       
            controller.Setup(c => c.UserId).Returns(USER_ID);
        }
        [TestMethod]
        public void DeleteCatNULL()
        {

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((Cat?)null);

            var actionResult = controller.Object.DeleteCat(1);

            var result = actionResult.Result as NotFoundResult;
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void DeleteCatNotID() {

            CatOwner catOwner = new CatOwner()
            {

                Id = "1111"

            };
            Cat cat = new Cat()
            {
                Id = 1,
                Name = "patate",
                CuteLevel = Cuteness.Amazing,
                CatOwner = catOwner

            };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((cat));
            var actionResult = controller.Object.DeleteCat(1);
            var result = actionResult.Result as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Cat is not yours", result.Value);
        }
       
    }
}
