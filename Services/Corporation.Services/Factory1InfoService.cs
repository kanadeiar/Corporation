using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corporation.Dal.Data;
using Microsoft.EntityFrameworkCore;

namespace Corporation.Services
{
    public class Factory1InfoService : IFactory1InfoService
    {
        private readonly CorporationContext _Context;
        public Factory1InfoService(CorporationContext context)
        {
            _Context = context;
        }

        public async Task<Factory1Warehouse2WebModel> GetInfoFactory1Warehouse2()
        {
            var lastdata = await _Context.Com1Warehouse2ShiftDatas
                .Include(i => i.Com1Pack)
                .OrderByDescending(i => i.Time).FirstOrDefaultAsync();
            var model = (lastdata is not null) ? new Factory1Warehouse2WebModel
            {
                Id = lastdata.Id,
                PackName = lastdata.Com1Pack?.Name,
                Value = lastdata.Com1PackValue,
            } : null;
            return model;
        }
    }

    public class Factory1Warehouse2WebModel
    {
        public int Id { get; set; }
        public string PackName { get; set; }
        public int Value { get; set; }
    }
}
