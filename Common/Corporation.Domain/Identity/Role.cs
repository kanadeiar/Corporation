using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Corporation.Domain.Identity
{
    /// <summary> Роль пользователя </summary>
    public class Role : IdentityRole
    {
        [Required(ErrorMessage = "Название обязательна для роли пользователей")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название роли должно быть длинной от 3 до 200 символов")]
        public string RoleName { get; set; }
    }
}
