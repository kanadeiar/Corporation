using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Corporation.Domain.Entites.Base;
using Corporation.Domain.Identity;

namespace Corporation.Domain.Company1
{
    /// <summary> Данные за одну смену по прессу </summary>
    public class Com1Press1ShiftData : Entity
    {
        /// <summary> Дата время смены </summary>
        [Required(ErrorMessage = "Дата время смены обязательна")]
        public DateTime Time { get; set; } = DateTime.Today;

        /// <summary> Смена </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана смена")]
        public int Com1ShiftId { get; set; }
        /// <summary> Cмена </summary>
        [ForeignKey(nameof(Com1ShiftId))]
        public Com1Shift Com1Shift { get; set; }

        /// <summary> Пресовщик </summary>
        [Required(ErrorMessage = "Должен быть выбран пресовщик")]
        public string UserId { get; set; }
        /// <summary> Пресовщик </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary> Вид товара </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран вид товара")]
        public int Com1ProductTypeId { get; set; }
        /// <summary> Вид товара </summary>
        [ForeignKey(nameof(Com1ProductTypeId))]
        public Com1ProductType Com1ProductType { get; set; }
        /// <summary> Количество вида товара </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество вида товаров должно быть положительным числом")]
        public int Com1ProductTypeCount { get; set; }

        /// <summary> Использовано сырья - песка </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Количество использованного сырья песка должно быть положительным числом")]
        public double Com1Loose1RawValue { get; set; }

        /// <summary> Использовано сырья - извести </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Количество использованного сырья извести должно быть положительным числом")]
        public double Com1Loose2RawValue { get; set; }
    }
}
