using Corporation.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation.Services
{
    public class ProductsInfoService : IProductsInfoService
    {
        private readonly IProductTypeData _productTypeData;
        public ProductsInfoService(IProductTypeData productTypeData)
        {
            _productTypeData = productTypeData;
        }

        public async Task<IEnumerable<ProductInfoWebModel>> GetInfoForProducts()
        {
            var infos = await _productTypeData.GetAllAsync(true);
            var results = infos.Select(pt => new ProductInfoWebModel
            {
                Id = pt.Id,
                Name = pt.Name,
                Number = pt.Number,
                Units = pt.Units,
            }).ToList();
            return results.OrderBy(r => r.Name);
        }
    }

    public class ProductInfoWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int Units { get; set; }
    }
}
