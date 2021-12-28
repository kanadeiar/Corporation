using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Corporation.Domain.Entites.Structures;

namespace Corporation.Domain.Identity
{
    /// <summary> Роль пользователя </summary>
    public class Role : IdentityRole
    {
        /// <summary> Описание роли </summary>
        [Required(ErrorMessage = "Название обязательна для роли пользователей")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название роли должно быть длинной от 3 до 200 символов")]
        public string RoleName { get; set; }

        /// <summary> Отдел </summary>
        public int DepartmentId { get; set; }
        /// <summary> Отдел </summary>
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }
}
