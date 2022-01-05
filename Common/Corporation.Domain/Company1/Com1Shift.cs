using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Company1
{
    /// <summary> Рабочая смена </summary>
    public class Com1Shift : Entity
    {
        /// <summary> Название </summary>
        [Required(ErrorMessage = "Название смены обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название смены должно быть от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Метка удаления </summary>
        public bool IsDelete { get; set; }
    }
}
