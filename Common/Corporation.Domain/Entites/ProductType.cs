using Corporation.Domain.Entites.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Corporation.Domain.Entites
{
    /// <summary> Вид продукта, одна упаковка </summary>
    public class ProductType : Entity
    {
        /// <summary> Название продукта, одна упаковка </summary>
        [Required(ErrorMessage = "Название упаковки вида продукта обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название упаковки вида продукта должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Номер упаковки вида товаров </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер упаковки вида продукта должен быть положительным числом")]
        public int FormatNumber { get; set; }

        /// <summary> Количество неделимых едениц в одной упаковке </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество неделимых едениц в упаковке продукта должно быть положительным числом")]
        public int Units { get; set; }

        /// <summary> Объем, занимаемый одной упаковкой </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Объем одной упаковки продукта должен быть положительным числом")]
        public double Volume { get; set; }

        /// <summary> Вес одной упаковки </summary>
        [Range(0.0, double.MaxValue, ErrorMessage = "Вес одной упаковки продукта должен быть положительным числом")]
        public double Weight { get; set; }

        /// <summary> Стоимость одной упаковки продукта </summary>
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Значение стоимости одной упаковки продукта должно быть положительным числом")]
        public decimal Price { get; set; }

        /// <summary> Метка удаления вида упаковки продукта </summary>
        public bool IsDelete { get; set; }

        public override string ToString()
        {
            return $"[{FormatNumber}] {Name}";
        }
    }
}
