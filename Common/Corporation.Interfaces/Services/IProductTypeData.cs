using Corporation.Domain.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;
using Corporation.Domain.Company1;

namespace Corporation.Interfaces.Services
{
    /// <summary> Источник видов товаров </summary>
    public interface IProductTypeData
    {
        /// <summary> Получить все </summary>
        Task<IEnumerable<Com1ProductType>> GetAllAsync(bool includes = false, bool trashed = false);
        /// <summary> Данные одного </summary>
        Task<Com1ProductType> GetAsync(int id);
        /// <summary> Добавить </summary>
        Task<int> AddAsync(Com1ProductType productType);
        /// <summary> Обновить </summary>
        Task UpdateAsync(Com1ProductType productType);
        /// <summary> Удалить </summary>
        Task<bool> DeleteAsync(int id);
        /// <summary> Пометка удаления </summary>
        Task<bool> TrashAsync(int id, bool restore = false);
    }
}
