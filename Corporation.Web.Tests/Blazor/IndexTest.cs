using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Corporation.Web.Blazor;
using Bunit;
using Corporation.Dal.Data;
using Corporation.Domain.Company1;
using Corporation.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Corporation.Web.Tests.Blazor
{
    [TestClass]
    public class IndexTest
    {
        [TestMethod]
        public void Index_InitCorrect_ShouldCorrect()
        {
            var expectedUser = new User { Id = "123", SurName = "Testov", FirstName = "Test", Patronymic = "Testovich" };
            using var context = new Bunit.TestContext();
            context.Services.AddDbContext<CorporationContext>(options =>
            {
                options.UseInMemoryDatabase(nameof(Index_InitCorrect_ShouldCorrect));
            });
            var testContext1 = context
                .Services.CreateScope().ServiceProvider.GetService<CorporationContext>();
            testContext1!.Com1Warehouse1ShiftDatas.Add(
                new Com1Warehouse1ShiftData
                {
                    Time = DateTime.Today.AddHours(-16),
                    Com1Shift = new Com1Shift {Name = "Test"},
                    UserId = expectedUser.Id,
                    Com1Tank1LooseRaw = new Com1LooseRaw{Name = "Test1"},
                    Com1Tank1LooseRawValue = 220.0,
                    Com1Tank2LooseRaw = new Com1LooseRaw { Name = "Test2" },
                    Com1Tank2LooseRawValue = 320.0,
                });
            testContext1!.SaveChangesAsync().Wait();

            var index = context.RenderComponent<Web.Blazor.Index>();

            Assert.IsTrue( index.Markup.Contains("<h6>Завод 1</h6>"));
            Assert.AreEqual( expectedUser.Id, index.Instance.Com1Warehouse1ShiftData.UserId );
        }
    }
}
