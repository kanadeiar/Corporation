using Microsoft.AspNetCore.Mvc;

namespace Corporation.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            var models = await users.Select(u => new UserWebModel
            {
                Id = u.Id,
                SurName = u.SurName,
                FirstName = u.FirstName,
                Patronymic = u.Patronymic,
                UserName = u.UserName,
                Email = u.Email,
                BirthDay = u.Birthday,
                Age = DateTime.Today.Year - u.Birthday.Year,
                Department = u.Department,
            }).ToArrayAsync();
            return View(models);
        }

        /// <summary> Вебмодель просмотра пользователя </summary>
        public class UserWebModel
        {
            public string Id { get; set; }

            [Display(Name = "Фамилия пользователя")]
            public string SurName { get; set; }

            [Display(Name = "Имя пользователя")]
            public string FirstName { get; set; }

            [Display(Name = "Отчество пользователя")]
            public string Patronymic { get; set; }

            [Display(Name = "Логин пользователя")]
            public string UserName { get; set; }

            [Display(Name = "Почта пользователя")]
            public string Email { get; set; }

            [Display(Name = "Возраст")]
            public int Age { get; set; }

            [Display(Name = "Дата рождения")]
            public DateTime BirthDay { get; set; }

            [Display(Name = "Отдел")]
            public string Department { get; set; }
        }
    }
}
