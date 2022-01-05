using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Entites.Structures
{
    /// <summary> Рабочее место </summary>
    public class Workstation : Entity
    {
        /// <summary> Название рабочего места </summary>
        [Required(ErrorMessage = "Название рабочего места обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название рабочего места должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Метка удаления </summary>
        public bool IsDelete { get; set; }

        /// <summary> Отдел </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Должен быть выбран отдел")]
        public int DepartmentId { get; set; }
        /// <summary> Отдел </summary>
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }
}
