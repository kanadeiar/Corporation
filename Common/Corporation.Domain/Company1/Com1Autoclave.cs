using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Company1
{
    /// <summary> Автоклав </summary>
    public class Com1Autoclave : Entity
    {
        /// <summary> Название автоклава, одна еденица </summary>
        [Required(ErrorMessage = "Название автоклава обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название автоклава должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }
    }
}
