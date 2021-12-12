namespace Corporation.Web.Pages;

public class ProductsModel : PageModel
{
    private readonly ProductsInfoService _productsInfoService;
    public ProductsModel(ProductsInfoService productsInfoService)
    {
        _productsInfoService = productsInfoService;
    }
    public IEnumerable<ProsuctInfoWebModel> ProductInfos { get; set; }
    public async Task OnGetAsync()
    {
        ProductInfos = await _productsInfoService.GetInfoForProducts();
    }
}

