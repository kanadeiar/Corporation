using System.ComponentModel.DataAnnotations;

namespace Corporation.Domain.Entites.Base
{
    /// <summary> Базовая сущность данных </summary>
    public abstract class Entity
    {
        /// <summary> Идентификатор </summary>
        [Key]
        public int Id { get; set; }
        /// <summary> Датовременной штамп </summary>
        [Timestamp]
        public byte[]? Timestamp { get; set; }
    }
}
