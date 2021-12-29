using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Company1
{
    /// <summary> Сыпучее сырье </summary>
    public class Com1LooseRaw : Entity
    {
        /// <summary> Название сыпучего сырья, объем </summary>
        [Required(ErrorMessage = "Название сыпучего сырья обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название сырья должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Стоимость еденицы объема сырья </summary>
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Значение стоимости одной еденицы объема сырья должно быть положительным числом")]
        public decimal Price { get; set; }

        /// <summary> Метка удаления сырья </summary>
        public bool IsDelete { get; set; }
    }
}
