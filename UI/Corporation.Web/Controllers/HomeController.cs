namespace Corporation.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
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
