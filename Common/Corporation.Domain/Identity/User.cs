using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Corporation.Domain.Identity
{
    /// <summary> Пользователь </summary>
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Фамилия обязательна для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Фамилия должна быть длинной от 3 до 200 символов")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Имя обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Имя должно быть длинной от 3 до 200 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Отчество обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Отчество должно быть длинной от 3 до 200 символов")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна для ввода")]
        public DateTime Birthday { get; set; } = DateTime.Today.AddYears(- 18);

        [Required(ErrorMessage = "Название отдела пользователя обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название отдела пользователя должно быть длинной от 3 до 200 символов")]
        public string Department { get; set; }
    }
}
