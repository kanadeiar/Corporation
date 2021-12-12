using Corporation.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation.Services
{
    public class ProductsInfoService
    {
        private readonly IProductTypeData _productTypeData;
        public ProductsInfoService(IProductTypeData productTypeData)
        {
            _productTypeData = productTypeData;
        }

        public async Task<IEnumerable<ProsuctInfoWebModel>> GetInfoForProducts()
        {
            var infos = await _productTypeData.GetAllAsync(true);
            var results = infos.Select(pt => new ProsuctInfoWebModel
            {
                Id = pt.Id,
                Name = pt.Name,
                Number = pt.Number,
                Volume = pt.Volume,
                Units = pt.Units,
                Weight = pt.Weight,
            }).ToList();
            return results.OrderBy(r => r.Name);
        }
    }

    public class ProsuctInfoWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public double Volume { get; set; }
        public int Units { get; set; }
        public double Weight { get; set; }
    }
}
