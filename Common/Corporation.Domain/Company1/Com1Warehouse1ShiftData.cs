using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Corporation.Domain.Entites.Base;
using Corporation.Domain.Identity;

namespace Corporation.Domain.Company1
{
    /// <summary> Данные за одну смену </summary>
    public class Com1Warehouse1ShiftData : Entity
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
        [Required (ErrorMessage = "Должен быть выбран кладовщик")]
        public string UserId { get; set; }
        /// <summary> Кладовщик </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary> Сырье в баке 1 </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должно быть выбрано сырье в баке 1")]
        public int Com1Tank1LooseRawId { get; set; }
        /// <summary> Сырье в баке 1 </summary>
        [ForeignKey(nameof(Com1Tank1LooseRawId))]
        public Com1LooseRaw Com1Tank1LooseRaw { get; set; }
        /// <summary> Объем сырья в баке 1 </summary>
        public double Com1Tank1LooseRawValue { get; set; }

        /// <summary> Сырье в баке 2 </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должно быть выбрано сырье в баке 2")]
        public int Com1Tank2LooseRawId { get; set; }
        /// <summary> Сырье в баке 2 </summary>
        [ForeignKey(nameof(Com1Tank2LooseRawId))]
        public Com1LooseRaw Com1Tank2LooseRaw { get; set; }
        /// <summary> Объем сырья в баке 2 </summary>
        public double Com1Tank2LooseRawValue { get; set; }
    }
}
