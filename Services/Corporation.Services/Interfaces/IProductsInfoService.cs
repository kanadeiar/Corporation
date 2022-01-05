namespace Corporation.Services;

public interface IProductsInfoService
{
    Task<IEnumerable<ProductInfoWebModel>> GetInfoForProducts();
}