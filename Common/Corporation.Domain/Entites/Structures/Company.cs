using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Entites.Structures
{
    /// <summary> Компания </summary>
    public class Company : Entity
    {
        /// <summary> Название компании </summary>
        [Required(ErrorMessage = "Название компании обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название компании должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Метка удаления </summary>
        public bool IsDelete { get; set; }

        /// <summary> Корпорация </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана корпорация")]
        public int CorpId { get; set; }
        /// <summary> Корпорация </summary>
        [ForeignKey(nameof(CorpId))]
        public Corp Corp { get; set; }

        /// <summary> Данные по отделам </summary>
        public List<Department> DepartamentsDatas { get; set; } = new List<Department>();
    }
}
