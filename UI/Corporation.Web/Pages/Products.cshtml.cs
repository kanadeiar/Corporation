namespace Corporation.Web.Pages;

public class ProductsModel : PageModel
{
    private readonly ProductsInfoService _productsInfoService;
    public ProductsModel(ProductsInfoService productsInfoService)
    {
        _productsInfoService = productsInfoService;
    }
    public IEnumerable<ProductInfoWebModel> ProductInfos { get; set; }
    public async Task OnGetAsync()
    {
        ProductInfos = await _productsInfoService.GetInfoForProducts();
    }
}

