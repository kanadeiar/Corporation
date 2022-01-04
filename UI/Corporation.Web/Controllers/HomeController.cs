namespace Corporation.Web.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index([FromServices] Factory1InfoService factory1InfoService)
    {
        ViewBag.Factory1Warehouse2 = await factory1InfoService.GetInfoFactory1Warehouse2();
        return View();
    }

    public async Task<IActionResult> Products([FromServices] ProductsInfoService productsInfoService)
    {
        ViewBag.ProductInfos = await productsInfoService.GetInfoForProducts();
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Error(string id)
    {
        switch (id)
        {
            default: return Content($"Status --- {id}");
            case "404": return View("Error404");
        }
    }
}
