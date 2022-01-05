using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Corporation.Domain.Entites.Base;

namespace Corporation.Domain.Entites.Structures
{
    /// <summary> Корпорация </summary>
    public class Corp : Entity
    {
        /// <summary> Название копорации </summary>
        [Required(ErrorMessage = "Название корпорации обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название корпорации должно быть длинной от 3 до 200 символов")]
        public string Name { get; set; }

        /// <summary> Метка удаления </summary>
        public bool IsDelete { get; set; }
        
        /// <summary> Данные по компаниям </summary>
        public List<Company> CompaniesDatas { get; set; } = new List<Company>();
    }
}
