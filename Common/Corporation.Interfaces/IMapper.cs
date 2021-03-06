namespace Corporation.Interfaces
{
    /// <summary> Маппер </summary>
    /// <typeparam name="TIn">Исходный</typeparam>
    /// <typeparam name="TOut">Требуемый</typeparam>
    public interface IMapper<TIn, TOut> where TIn: class where TOut: class
    {
        /// <summary> Преобразование типа </summary>
        /// <param name="source">Исходное</param>
        /// <returns>Требуемое</returns>
        TOut Map(TIn source);
    }
}
