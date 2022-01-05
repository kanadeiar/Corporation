using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Corporation.Web.Tests.Blazor.Components
{
    [TestClass]
    public class TooltipTest
    {
        [TestMethod]
        public void Tooltip_Init_ShouldCorrect()
        {
            using var context = new Bunit.TestContext();

            var index = context
                .RenderComponent<Web.Blazor.Components.Tooltip>(builder => builder.Add(c => c.ChildContent, "Test").Add(c => c.Text, "Test1"));

            Assert.IsTrue(index.Markup.Contains("<div class=\"tooltip-wrapper\""));
            Assert.IsTrue(index.Markup.Contains(">Test1</span>"));
        }
    }
}
