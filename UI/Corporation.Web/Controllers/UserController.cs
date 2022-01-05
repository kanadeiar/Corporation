using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Corporation.Web.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly CorporationContext _Context;
    public UserController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, CorporationContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _Context = context;
    }

    /// <summary> Пользователи </summary>
    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users;
        //var r = (_roleManager.Roles.First().RoleName);
        var models = await users.Include(u => u.Company).Select(u => new UserWebModel
        {
            Id = u.Id,
            SurName = u.SurName,
            FirstName = u.FirstName,
            Patronymic = u.Patronymic,
            UserName = u.UserName,
            Email = u.Email,
            BirthDay = u.Birthday,
            Age = DateTime.Today.Year - u.Birthday.Year,
            CompanyName = u.Company.Name,
            RolesNames = _userManager.GetRolesAsync(u).Result,
        }).ToArrayAsync();
        foreach (var m in models)
        {
            m.RolesNames = m.RolesNames.Select(r => _roleManager.Roles.First(rr => rr.Name == r).RoleName);
        }
        return View(models);
    }

    /// <summary> Создание нового пользователя </summary>
    public async Task<IActionResult> Create()
    {
        var model = new UserEditWebModel
        {
            NonMembersRoles = _roleManager.Roles.ToList()
                .Select(r => r.Name)
        };
        model.NonMemberNamesRoles = model.NonMembersRoles.Select(r => _roleManager.Roles.First(rr => rr.Name == r).RoleName);
        ViewBag.Companies = new SelectList(await _Context.Companies.ToListAsync(), "Id", "Name");
        return View("Edit", model);
    }

    /// <summary> Редактирование пользователя </summary>
    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            var model = new UserEditWebModel
            {
                NonMembersRoles = _roleManager.Roles.ToList()
                    .Select(r => r.Name)
            };
            model.NonMemberNamesRoles = model.NonMembersRoles.Select(r => _roleManager.Roles.First(rr => rr.Name == r).RoleName);
            ViewBag.Companies = new SelectList(await _Context.Companies.ToListAsync(), "Id", "Name");
            return View(model);
        }
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
                CompanyId = user.CompanyId,
                MembersRoles = _userManager.GetRolesAsync(user).Result,
                NonMembersRoles = _roleManager.Roles.ToList()
                    .Select(r => r.Name)
                    .Except(_userManager.GetRolesAsync(user).Result),
            };
            model.MemberNamesRoles = model.MembersRoles.Select(r => _roleManager.Roles.First(rr => rr.Name == r).RoleName);
            model.NonMemberNamesRoles = model.NonMembersRoles.Select(r => _roleManager.Roles.First(rr => rr.Name == r).RoleName);
            ViewBag.Companies = new SelectList(await _Context.Companies.ToListAsync(), "Id", "Name");
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
                CompanyId = model.CompanyId,
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
            user.CompanyId = model.CompanyId;
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

    /// <summary> Редактирование роли у пользователя </summary>
    /// <param name="userid">Ид пользователя</param>
    /// <param name="rolename">Роль</param>
    public async Task<IActionResult> SelectMemberRoles(string userid, string rolename)
    {
        var user = await _userManager.FindByIdAsync(userid);
        IdentityResult result;
        if (await _userManager.IsInRoleAsync(user, rolename))
        {
            if (rolename == "admins" && user.UserName == "admin")
            {
                return RedirectToAction("Edit", "User", new { id = userid });
            }
            result = await _userManager.RemoveFromRoleAsync(user, rolename);
        }
        else
        {
            result = await _userManager.AddToRoleAsync(user, rolename);
        }
        if (result.Succeeded)
        {
            return RedirectToAction("Edit", "User", new { id = userid });
        }
        foreach (var err in result.Errors)
        {
            ModelState.AddModelError("", err.Description);
        }
        return RedirectToAction("Edit", "User", new { id = userid });
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
            CompanyName = u.Company.Name,
            RolesNames = _userManager.GetRolesAsync(u).Result,
        };
        model.RolesNames = model.RolesNames.Select(r => _roleManager.Roles.First(rr => rr.Name == r).RoleName);
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
    public async Task<IActionResult> IsNameFree(string UserName, string Id)
    {
        var user = await _userManager.FindByNameAsync(UserName);
        var result = (user is null || (user is { } u && u.Id == Id)) ? "true" : "Пользователь с таким имененем уже существует";
        return Json(result);
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

        [Display(Name = "Компания")]
        public string CompanyName { get; set; }

        [Display(Name = "Роли у этого пользователя")]
        //public string RolesNames { get; set; }
        public IEnumerable<string> RolesNames { get; set; } = Enumerable.Empty<string>();
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

        /// <summary> Компания </summary>
        [Required(ErrorMessage = "Компания обязательня для роли пользователей")]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть выбрана компания")]
        [Display(Name = "Компания")]
        public int CompanyId { get; set; }

        [Display(Name = "Роли, назначенные пользователю")]
        public IEnumerable<string>? MembersRoles { get; set; }
        public IEnumerable<string>? MemberNamesRoles { get; set; }

        [Display(Name = "Можно назначить роли")]
        public IEnumerable<string>? NonMembersRoles { get; set; }
        public IEnumerable<string>? NonMemberNamesRoles { get; set; }

        [Required(ErrorMessage = "Нужно обязательно ввести логин пользователя")]
        [Display(Name = "Логин пользователя")]
        [Remote("IsNameFree", "User", AdditionalFields = "Id")]
        public string UserName { get; set; }

        [Display(Name = "Новый пароль пользователя")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    #endregion
}

