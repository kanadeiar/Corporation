namespace Corporation.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.Cookie = Request.Cookies[".AspNetCore.Identity.Application"];
            var model = new IndexWebModel();
            model.Cookie = Request.Cookies[".AspNetCore.Identity.Application"];
            if (User.Identity.IsAuthenticated)
            {
                model.User = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            return View(model);
        }

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
            var result = await _signInManager.PasswordSignInAsync(model.UserName,model.Password,model.RememberMe,false);
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
            public string Cookie { get; set; }
            /// <summary> Сведения о пользовате </summary>
            public User User { get; set; }
        }

        /// <summary> Веб модель регистрации </summary>
        public class RegisterWebModel
        {
            /// <summary> Имя пользователя </summary>
            [Required (ErrorMessage = "Нужно обязательно ввести логин пользователя")]
            [Display(Name = "Логин пользователя")]
            public string UserName { get; set; }

            [Required (ErrorMessage = "Нужно обязательно ввести свой адрес электронной почты")]
            [EmailAddress (ErrorMessage = "Нужно ввести корректный адрес своей электронной почты")]
            [Display(Name = "Адрес электронной почты e-mail")]            
            public string Email { get; set; }

            /// <summary> Пароль </summary>
            [Required (ErrorMessage = "Нужно обязательно придумать и ввести какой-либо пароль")]
            [Display(Name = "Пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary> Подтверждение пароля </summary>
            [Required (ErrorMessage = "Нужно обязательно ввести подтверждение пароля")]
            [Display(Name = "Подтверждение пароля")]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
            public string PasswordConfirm { get; set; }
        }

        /// <summary> Веб модель входа в систему </summary>
        public class LoginWebModel
        {
            /// <summary> Имя пользователя </summary>
            [Required (ErrorMessage = "Нужно обязательно ввести логин пользователя")]
            [Display(Name = "Логин пользователя")]
            public string UserName { get; set; }
            /// <summary> Пароль пользователя </summary>
            [Required (ErrorMessage = "Нужно обязательно ввести свой пароль")]
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
