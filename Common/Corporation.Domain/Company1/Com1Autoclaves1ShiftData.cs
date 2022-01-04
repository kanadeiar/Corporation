using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Corporation.Domain.Entites.Base;
using Corporation.Domain.Identity;

namespace Corporation.Domain.Company1
{
    /// <summary> Данные за одну смену по автоклавам </summary>
    public class Com1Autoclaves1ShiftData : Entity
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

        /// <summary> Автоклавщик </summary>
        [Required(ErrorMessage = "Должен быть выбран автоклавщик")]
        public string UserId { get; set; }
        /// <summary> Автоклавщик </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary> Автоклав </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран автоклав")]
        public int Com1AutoclaveId { get; set; }
        /// <summary> Автоклав </summary>
        [ForeignKey(nameof(Com1AutoclaveId))]
        public Com1Autoclave Com1Autoclave { get; set; }

        /// <summary> Дата начала автоклаирования </summary>
        [Required(ErrorMessage = "Дата начала автоклавирования обязательна")]
        public DateTime TimeStart { get; set; } = DateTime.Now;

        /// <summary> Продолжительность автоклавирования </summary>
        public TimeSpan AutoclavedTime { get; set; }

        /// <summary> Количество пачек автоклавировано </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Количество пачек автоклавированных должно быть положительным числом")]
        public int AutoclavedCount { get; set; }
    }
}
