namespace Corporation.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var model = new IndexWebModel();
            model.Cookie = Request.Cookies[".AspNetCore.Identity.Application"];
            if (User.Identity.IsAuthenticated)
            {
                model.User = await _userManager.FindByNameAsync(User.Identity.Name);
                var roles = await _userManager.GetRolesAsync(model.User);
                model.UserRoleNames = _roleManager.Roles.Where(r => roles.Contains(r.Name)).Select(r => r.RoleName);
            }
            return View(model);
        }

        #region Роли пользователей

        public async Task<IActionResult> RoleList()
        {
            var roles = _roleManager.Roles;
            var models = await roles.Select(r => new RoleWebModel
            {
                Id = r.Id,
                Name = r.Name,
                RoleName = r.RoleName,
            }).ToListAsync();
            foreach (var item in models)
            {
                var users = await _userManager.GetUsersInRoleAsync(item.Name);
                string result = users.Count() == 0
                    ? "Нет пользователей"
                    : string.Join(", ", users.Take(3).Select(u => $"{u.SurName} {u.FirstName[0]}. {u.Patronymic[0]}.").ToArray());
                item.UsersNames = users.Count() > 3 ? $"{result}, и др." : result;
            };
            return View(models);
        }

        /// <summary> Создание новой роли </summary>
        public async Task<IActionResult> RoleCreate()
        {
            return View("Edit", new RoleEditWebModel());
        }

        /// <summary> Редактирование новой роли </summary>
        public async Task<IActionResult> RoleEdit(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new RoleEditWebModel());
            if (await _roleManager.FindByIdAsync(id) is { } role)
            {
                var model = new RoleEditWebModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    RoleName = role.RoleName,
                };
                return View(model);
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleEdit(RoleEditWebModel model)
        {
            if (model is null)
                return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var role = await _roleManager.FindByIdAsync(model.Id);
            IdentityResult result;
            if (role is null)
            {
                var newRole = new Role
                {
                    Name = model.Name,
                    RoleName = model.RoleName,
                };
                result = await _roleManager.CreateAsync(newRole);
            }
            else
            {
                role.Name = model.Name;
                role.RoleName = model.RoleName;
                result = await _roleManager.UpdateAsync(role);
            }
            if (result.Succeeded)
            {
                return RedirectToAction("RoleList", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var r = await _roleManager.FindByIdAsync(id);
            if (r is null)
                return NotFound();
            var model = new RoleWebModel
            {
                Id = r.Id,
                Name = r.Name,
                RoleName = r.RoleName,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoleConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("RoleList", "Account");
        }

        #endregion

        #region Регистрация пользователя

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterWebModel());

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterWebModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "users");
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            var errors = result.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
            return View(model);
        }

        #endregion

        #region Вход

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => View(new LoginWebModel { ReturnUrl = returnUrl });

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login(LoginWebModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле при входе в систему Identity");
            return View();
        }

        #endregion

        #region Выход из системы

        public async Task<IActionResult> Logout(string returnUrl)
        {
            var username = User.Identity!.Name;
            await _signInManager.SignOutAsync();
            return LocalRedirect(returnUrl ?? "/");
        }

        #endregion

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary> Вебмодель сведения о пользователе </summary>
        public class IndexWebModel
        {
            /// <summary> Куки пользователя </summary>
            public string Cookie { get; set; }
            /// <summary> Сведения о пользовате </summary>
            public User User { get; set; }
            /// <summary> Роли пользователя </summary>
            public IEnumerable<string> UserRoleNames { get; set; } = Enumerable.Empty<string>();
        }

        /// <summary> Веб модель просмотра роли пользователей </summary>
        public class RoleWebModel
        {
            public string Id { get; set; }

            [Required(ErrorMessage = "Системное имя роли обязательна для роли пользователей")]
            [StringLength(200, MinimumLength = 3, ErrorMessage = "Системное имя роли должно быть длинной от 3 до 200 символов")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Название обязательна для роли пользователей")]
            [StringLength(200, MinimumLength = 3, ErrorMessage = "Название роли должно быть длинной от 3 до 200 символов")]
            public string RoleName { get; set; }

            public string UsersNames { get; set; }
        }

        /// <summary> Веб модель редактирования роли пользователей </summary>
        public class RoleEditWebModel
        {
            public string Id { get; set; }

            [Required(ErrorMessage = "Системное имя роли обязательна для роли пользователей")]
            [StringLength(200, MinimumLength = 3, ErrorMessage = "Системное имя роли должно быть длинной от 3 до 200 символов")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Название обязательна для роли пользователей")]
            [StringLength(200, MinimumLength = 3, ErrorMessage = "Название роли должно быть длинной от 3 до 200 символов")]
            public string RoleName { get; set; }
        }

        /// <summary> Веб модель регистрации </summary>
        public class RegisterWebModel
        {
            /// <summary> Имя пользователя </summary>
            [Required(ErrorMessage = "Нужно обязательно ввести логин пользователя")]
            [Display(Name = "Логин пользователя")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Нужно обязательно ввести свой адрес электронной почты")]
            [EmailAddress(ErrorMessage = "Нужно ввести корректный адрес своей электронной почты")]
            [Display(Name = "Адрес электронной почты e-mail")]
            public string Email { get; set; }

            /// <summary> Пароль </summary>
            [Required(ErrorMessage = "Нужно обязательно придумать и ввести какой-либо пароль")]
            [Display(Name = "Пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary> Подтверждение пароля </summary>
            [Required(ErrorMessage = "Нужно обязательно ввести подтверждение пароля")]
            [Display(Name = "Подтверждение пароля")]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
            public string PasswordConfirm { get; set; }
        }

        /// <summary> Веб модель входа в систему </summary>
        public class LoginWebModel
        {
            /// <summary> Имя пользователя </summary>
            [Required(ErrorMessage = "Нужно обязательно ввести логин пользователя")]
            [Display(Name = "Логин пользователя")]
            public string UserName { get; set; }
            /// <summary> Пароль пользователя </summary>
            [Required(ErrorMessage = "Нужно обязательно ввести свой пароль")]
            [Display(Name = "Пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            /// <summary> Запомнить этого пользователя </summary>
            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
            /// <summary> Возвращение на страницу </summary>
            [HiddenInput(DisplayValue = false)]
            public string ReturnUrl { get; set; }
        }
    }


}
