using Microsoft.AspNetCore.Http.HttpResults;
using mock.depart.Controllers;
using mock.depart.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;   // pour NotFoundResult
using mock.depart.Models;
using mock.Exceptions;

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

                Id = "1111"            };
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

        [TestMethod]
        public void DeleteCatLevel()
        {
            CatOwner catOwner = new CatOwner()
            {

                Id = USER_ID

            };
            Cat cat = new Cat()
            {
                Id = 1,
                Name = "patate",
                CuteLevel = Cuteness.BarelyOk,
                CatOwner = catOwner

            };
            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((cat));
            serviceMock.Setup(s => s.Delete(It.IsAny<int>())).Returns((cat));
            var actionResult = controller.Object.DeleteCat(1);
            var result = actionResult.Result as OkObjectResult;
            Assert.AreEqual( cat ,result.Value);
            
        }

        [TestMethod]
        public void DeleteCatCatIsToCute()
        {
            CatOwner catOwner = new CatOwner()
            {

                Id = USER_ID

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
            Assert.AreEqual( "Cat is too cute" ,result.Value);
        }

        [TestMethod]
        public void DeleteCatCode18Exception()
        {
            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Throws(new Code18Exception());
            var actionResult = controller.Object.DeleteCat(1);
            var result = actionResult.Result as UnauthorizedResult;
            Assert.IsNotNull(result);
            
        }

        [TestMethod]
        public void ChiffreChanceux()
        {
            serviceMock.SetupSequence(s => s.ChiffreChanceux()).Returns(33).Returns(42).Returns(67);
            var actionResult1 = controller.Object.ChiffreChanceux();
            var result1 = actionResult1.Result as OkObjectResult;
            Assert.IsNotNull(result1);
            Assert.AreEqual(33, result1.Value);

            // Appel 2
            var actionResult2 = controller.Object.ChiffreChanceux();
            var result2 = actionResult2.Result as OkObjectResult;
            Assert.IsNotNull(result2);
            Assert.AreEqual(42, result2.Value);

            // Appel 3
            var actionResult3 = controller.Object.ChiffreChanceux();
            var result3 = actionResult3.Result as OkObjectResult;
            Assert.IsNotNull(result3);
            Assert.AreEqual(67, result3.Value);
        }

    }
}
