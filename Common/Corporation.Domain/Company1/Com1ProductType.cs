using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Company1
{
    /// <summary> Вид товара </summary>
    public class Com1ProductType : Entity
    {
        /// <summary> Название пачки товара, еденица </summary>
        [Required(ErrorMessage = "Название вида товаров обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название вида товаров должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Номер пачки товаров </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Номер пачки продукта должен быть положительным числом")]
        public int Number { get; set; }

        /// <summary> Количество неделимых едениц в одной пачке </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество неделимых едениц в пачке продукта должно быть положительным числом")]
        public int Units { get; set; }

        /// <summary> Количество сырья первого </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должно быть выбрано сырье первое")]
        public int Com1Loose1RawId { get; set; }
        /// <summary> Сырье первое </summary>
        [ForeignKey(nameof(Com1Loose1RawId))]
        public Com1LooseRaw Com1Loose1Raw { get; set; }
        /// <summary> Объем сырья первого </summary>
        public double Com1Loose1RawValue { get; set; }

        /// <summary> Количество сырья второго </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должно быть выбрано сырье в баке 2")]
        public int Com1Loose2RawId { get; set; }
        /// <summary> Сырье второе </summary>
        [ForeignKey(nameof(Com1Loose2RawId))]
        public Com1LooseRaw Com1Loose2Raw { get; set; }
        /// <summary> Объем сырья второго </summary>
        public double Com1Loose2RawValue { get; set; }

        /// <summary> Метка удаления вида упаковки продукта </summary>
        public bool IsDelete { get; set; }
    }
}
