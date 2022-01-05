using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Corporation.Domain.Entites.Base;
using Corporation.Domain.Identity;

namespace Corporation.Domain.Company1
{
    /// <summary> Данные за одну смену по упаковке </summary>
    public class Com1Packing1ShiftData : Entity
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

        /// <summary> Упаковщик </summary>
        [Required(ErrorMessage = "Должен быть выбран упаковщик")]
        public string UserId { get; set; }
        /// <summary> Упаоквщик </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary> Вид упаковки товара </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана упаковка товара")]
        public int Com1PackId { get; set; }
        /// <summary> Упаковка товара </summary>
        [ForeignKey(nameof(Com1PackId))]
        public Com1Pack Com1Pack { get; set; }
        /// <summary> Количество упаковок товара упаковано </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество упакованных упаковок товаров должно быть положительным числом")]
        public int Com1PackCount { get; set; }
    }
}
