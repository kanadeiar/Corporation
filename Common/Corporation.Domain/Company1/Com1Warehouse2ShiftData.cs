using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Corporation.Domain.Entites.Base;
using Corporation.Domain.Identity;

namespace Corporation.Domain.Company1
{
    /// <summary> Данные за одну смену по складу товаров </summary>
    public class Com1Warehouse2ShiftData : Entity
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

        /// <summary> Кладовщик </summary>
        [Required(ErrorMessage = "Должен быть выбран кладовщик")]
        public string UserId { get; set; }
        /// <summary> Кладовщик </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary> Товар на месте хранения 1 </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должно быть выбран вид товара на месте хранения 1")]
        public int Com1PackId { get; set; }
        /// <summary> Товар на месте хранения 1 </summary>
        [ForeignKey(nameof(Com1PackId))]
        public Com1Pack Com1Pack { get; set; }
        /// <summary> Количество товара на месте хранения 1 </summary>
        public int Com1PackValue { get; set; }
    }
}
