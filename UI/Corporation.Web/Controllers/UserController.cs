namespace Corporation.Web.Controllers;

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

    /// <summary> Пользователи </summary>
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

    /// <summary> Создание нового пользователя </summary>
    public IActionResult Create()
    {
        return View("Edit");
    }

    /// <summary> Редактирование пользователя </summary>
    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
            return View(new UserEditWebModel());
        if (await _userManager.FindByIdAsync(id) is { } user)
        {
            var model = new UserEditWebModel
            {
                Id = user.Id,
                SurName = user.SurName,
                FirstName = user.FirstName,
                Patronymic = user.Patronymic,
                UserName = user.UserName,
                Email = user.Email,
                Birthday = user.Birthday,
                Department = user.Department,
            };
            return View(model);
        }
        return NotFound();
    }

    /// <summary> Редактирование пользователя </summary>
    /// <param name="model">Модель пользователя</param>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditWebModel model)
    {
        if (model is null)
            return BadRequest();
        if (!ModelState.IsValid)
            return View(model);
        var user = await _userManager.FindByIdAsync(model.Id);
        IdentityResult result;
        if (user is null)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError(nameof(model.Password), "Нужно обязательно ввести новый пароль пользователя");
                return View(model);
            }
            var newUser = new User
            {
                SurName = model.SurName,
                FirstName = model.FirstName,
                Patronymic = model.Patronymic,
                UserName = model.UserName,
                Email = model.Email,
                Birthday = model.Birthday,
                Department = model.Department,
            };
            result = await _userManager.CreateAsync(newUser, model.Password);
        }
        else
        {
            user.SurName = model.SurName;
            user.FirstName = model.FirstName;
            user.Patronymic = model.Patronymic;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Birthday = model.Birthday;
            user.Department = model.Department;
            result = await _userManager.UpdateAsync(user);
            if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
            {
                await _userManager.RemovePasswordAsync(user);
                result = await _userManager.AddPasswordAsync(user, model.Password);
            }
        }
        if (result.Succeeded)
            return RedirectToAction("Index", "User");
        foreach (var err in result.Errors)
            ModelState.AddModelError("", err.Description);
        return View(model);
    }

    /// <summary> Удалить пользователя </summary>
    /// <param name="id">Идентификатор пользователя</param>
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest();
        var u = await _userManager.FindByIdAsync(id);
        if (u is null)
            return NotFound();
        var model = new UserWebModel
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
        };
        return View(model);
    }

    /// <summary> Подтверждение удаления пользователя </summary>
    /// <param name="id">Идентификатор пользователя</param>
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest();
        var user = await _userManager.FindByIdAsync(id);
        if (user.UserName == "admin")
            return BadRequest();
        await _userManager.DeleteAsync(user);
        return RedirectToAction("Index", "User");
    }

    #region WebAPI

    [AllowAnonymous]
    public async Task<IActionResult> IsNameFree(string UserName)
    {
        await Task.Delay(1000);
        var user = await _userManager.FindByNameAsync(UserName);
        return Json(user is null ? "true" : "Пользователь с таким имененем уже существует");
    }

    #endregion

    #region Вспомогательные вебмодели

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

        [Display(Name = "Дата рождения")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Отдел")]
        public string Department { get; set; }
    }


    public class UserEditWebModel
    {
        [HiddenInput(DisplayValue = false)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Фамилия должна быть длинной от 3 до 200 символов")]
        [Display(Name = "Фамилия пользователя")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Имя обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Имя должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Имя пользователя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Отчество обязательно для пользователя")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Отчество должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Отчество пользователя")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести свой адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Нужно ввести корректный адрес своей электронной почты")]
        [Display(Name = "Адрес электронной почты E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна для ввода")]
        [Display(Name = "День рождения пользователя")]
        public DateTime Birthday { get; set; } = DateTime.Today.AddYears(-18);

        [Required(ErrorMessage = "Название отдела пользователя обязательно")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Название отдела пользователя должно быть длинной от 3 до 200 символов")]
        [Display(Name = "Отдел пользователя")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести логин пользователя")]
        [Display(Name = "Логин пользователя")]
        [Remote("IsNameFree", "Account")]
        public string UserName { get; set; }

        [Display(Name = "Новый пароль пользователя")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    #endregion
}

