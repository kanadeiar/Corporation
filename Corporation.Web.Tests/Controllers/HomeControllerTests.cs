using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Corporation.Services;
using Corporation.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Corporation.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Call_ShouldCorrect()
        {
            var factory1InfoStub = Mock
                .Of<IFactory1InfoService>();
            var controller = new HomeController();

            var actual = controller.Index(factory1InfoStub).Result;

            Assert
                .IsInstanceOfType(actual, typeof(IActionResult));
        }

        [TestMethod]
        public void Index_CorrectCall_ShouldCorrect()
        {
            var value = new Factory1Warehouse2WebModel
            {
                Id = 99,
                PackName = "test",
                Value = 12,
            };
            var factory1InfoMock = new Mock<IFactory1InfoService>();
            factory1InfoMock.Setup(_ => _.GetInfoFactory1Warehouse2()).Returns(Task.FromResult(value));
            var controller = new HomeController();

            var actual = controller.Index(factory1InfoMock.Object).Result;

            Assert
                .IsInstanceOfType(actual, typeof(ViewResult));
            var viewResult = (ViewResult)actual;
            Assert
                .IsInstanceOfType(viewResult.ViewData["Factory1Warehouse2"], typeof(Factory1Warehouse2WebModel));
            var data = viewResult.ViewData["Factory1Warehouse2"] as Factory1Warehouse2WebModel;
            Assert.AreEqual(99, data.Id);
            Assert.AreEqual("test", data.PackName);
            Assert.AreEqual(12, data.Value);
        }

        [TestMethod]
        public void Products_Call_ShouldCorrect()
        {
            var productsInfoStub = Mock.Of<IProductsInfoService>();
            var controller = new HomeController();

            var actual = controller.Products(productsInfoStub).Result;

            Assert
                .IsInstanceOfType(actual, typeof(IActionResult));
        }

        [TestMethod]
        public void Products_CorrectCall_ShouldCorrect()
        {
            var expectedCount = 10;
            var values = Enumerable.Range(1, expectedCount).Select(i => new ProductInfoWebModel
            {
                Id = i,
                Name = $"Тест_{i}",
                Number = i + 1,
                Units = i * 10,
            });
            var productsInfoMock = new Mock<IProductsInfoService>();
            productsInfoMock.Setup(_ => _.GetInfoForProducts()).Returns(Task.FromResult(values));
            var controller = new HomeController();

            var actual = controller.Products(productsInfoMock.Object).Result;

            Assert
                .IsInstanceOfType(actual, typeof(ViewResult));
            var viewResult = (ViewResult)actual;
            Assert
                .IsInstanceOfType(viewResult.ViewData["ProductInfos"], typeof(IEnumerable<ProductInfoWebModel>));
            var data = viewResult.ViewData["ProductInfos"] as IEnumerable<ProductInfoWebModel>;
            Assert.AreEqual(expectedCount, data.Count());
        }
    }
}
