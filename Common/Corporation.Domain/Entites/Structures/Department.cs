using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Entites.Structures
{
    /// <summary> Отдел </summary>
    public class Department : Entity
    {
        /// <summary> Название отдела </summary>
        [Required(ErrorMessage = "Название компании обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название компании должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Метка удаления </summary>
        public bool IsDelete { get; set; }

        /// <summary> Компания </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана компания")]
        public int CompanyId { get; set; }
        /// <summary> Компания </summary>
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        /// <summary> Данные по рабочим местам </summary>
        public List<Workstation> WorkstationDatas { get; set; } = new List<Workstation>();
    }
}
