using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Company1
{
    /// <summary> Упаковка </summary>
    public class Com1Pack : Entity
    {
        /// <summary> Название упаковки, одна еденица </summary>
        [Required(ErrorMessage = "Название упаковки обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название упаковки должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }
    }
}
