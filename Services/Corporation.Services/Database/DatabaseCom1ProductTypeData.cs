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
using Corporation.Domain.Company1;

namespace Corporation.Services.Database;

public class DatabaseCom1ProductTypeData : IProductTypeData
{
    private readonly CorporationContext _context;
    private readonly ILogger<DatabaseCom1ProductTypeData> _logger;
    public DatabaseCom1ProductTypeData(CorporationContext context, ILogger<DatabaseCom1ProductTypeData> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Com1ProductType>> GetAllAsync(bool includes = false, bool trashed = false)
    {
        IQueryable<Com1ProductType> query = _context.Com1ProductTypes.AsQueryable();
        query = !trashed
            ? query.Where(pt => !pt.IsDelete)
            : query.Where(pt => pt.IsDelete);
        return await query.ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<Com1ProductType> GetAsync(int id)
    { 
        var result = await _context.Com1ProductTypes.SingleOrDefaultAsync(pt => pt.Id == id).ConfigureAwait(false);
        return result;
    }

    public async Task<int> AddAsync(Com1ProductType productType)
    {
        if (productType is null)
            throw new ArgumentNullException(nameof(productType));
        _context.Com1ProductTypes.Add(productType);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return productType.Id;
    }

    public async Task UpdateAsync(Com1ProductType productType)
    {
        if (productType is null)
            throw new ArgumentNullException(nameof(productType));
        if (_context.Com1ProductTypes.Local.Any(e => e == productType) == false)
        {
            var origin = await _context.Com1ProductTypes.FindAsync(productType.Id).ConfigureAwait(false);
            origin.Name = productType.Name;
            origin.Number = productType.Number;
            origin.Units = productType.Units;
            origin.Com1Loose1RawId = productType.Com1Loose1RawId;
            origin.Com1Loose1RawValue = productType.Com1Loose1RawValue;
            origin.Com1Loose2RawId = productType.Com1Loose2RawId;
            origin.Com1Loose2RawValue = productType.Com1Loose2RawValue;
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
        _context.Com1ProductTypes.Remove(item);
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

