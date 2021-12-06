using Corporation.Dal.Data;
using Corporation.Domain.Entites;
using Corporation.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporation.Services.Database;

public class DatabaseProductTypeData : IProductTypeData
{
    private readonly Plant1Context _context;
    private readonly ILogger<DatabaseProductTypeData> _logger;
    public DatabaseProductTypeData(Plant1Context context, ILogger<DatabaseProductTypeData> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductType>> GetAllAsync(bool includes = false, bool trashed = false)
    {
        IQueryable<ProductType> query = _context.ProductTypes.AsQueryable();
        query = !trashed
            ? query.Where(pt => !pt.IsDelete)
            : query.Where(pt => pt.IsDelete);
        return await query.ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<ProductType> GetAsync(int id)
    {
        var result = await _context.ProductTypes.SingleOrDefaultAsync(pt => pt.Id == id).ConfigureAwait(false);
        return result;
    }

    public async Task<int> AddAsync(ProductType productType)
    {
        if (productType is null)
            throw new ArgumentNullException(nameof(productType));
        _context.ProductTypes.Add(productType);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return productType.Id;
    }

    public async Task UpdateAsync(ProductType productType)
    {
        if (productType is null)
            throw new ArgumentNullException(nameof(productType));
        if (_context.ProductTypes.Local.Any(e => e == productType) == false)
        {
            var origin = await _context.ProductTypes.FindAsync(productType.Id).ConfigureAwait(false);
            origin.Name = productType.Name;
            origin.Number = productType.Number;
            origin.Units = productType.Units;
            origin.Volume = productType.Volume;
            origin.Weight = productType.Weight;
            origin.Price = productType.Price;
            origin.IsDelete = productType.IsDelete;
            _context.Update(origin);
        }
        else
            _context.Update(productType);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (await GetAsync(id) is not { } item)
            return false;
        _context.ProductTypes.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TrashAsync(int id, bool restore = false)
    {
        if (await GetAsync(id).ConfigureAwait(false) is { } productType)
            productType.IsDelete = !restore;
        else
            return false;
        await _context.SaveChangesAsync();
        return true;
    }
}

