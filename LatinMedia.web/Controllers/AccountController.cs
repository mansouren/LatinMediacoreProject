using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;
using LatinMedia.Core.Generators;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using LatinMedia.Core.Senders;


namespace LatinMedia.web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _renderView;
        public AccountController(IUserService userService, IViewRenderService renderview)
        {
            _userService = userService;
            _renderView = renderview;
        }
        #region Register 

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            if (_userService.IsExistEmail(FixedText.FixedEmail(register.Email)))
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                return View(register);
            }
            if (_userService.IsExistMobile(register.Mobile))
            {
                ModelState.AddModelError("Mobile", "موبایل وارد شده تکراری است");
                return View(register);
            }

            DataLayer.Entities.User.User user = new DataLayer.Entities.User.User()
            {
                Mobile = register.Mobile,
                Email = FixedText.FixedEmail(register.Email),
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                FirstName = register.FirstName,
                LastName = register.LastName,
                ActiveCode = GeneratorName.GenrateUniqeCode(),
                IsActive = false,
                UserAvatar = "default.png",
                CreateDate = DateTime.Now


            };
            _userService.AddUser(user);

            #region SendActivationEmail
            string body = _renderView.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "ایمیل فعال سازی ", body);
            #endregion

            return View("SuccessRegister", model: user);
        }
        #endregion

        #region Login
        [Route("Login")]
        public IActionResult Login(bool EditProfile = false,bool permission=true,bool ChangeMail = true)
        {
            ViewBag.EditProfile = EditProfile;
            ViewBag.Permission = permission;
            ViewBag.ChangeMail = ChangeMail;
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = _userService.LoginUser(login);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claim = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.FirstName+" "+user.LastName),
                        new Claim(ClaimTypes.Email,user.Email)

                    };
                    var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };
                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نیست");
                }
            }
            ModelState.AddModelError("Email", "کاربری با این مشخصات یافت نشد");
            return View();
        }
        #endregion

        #region Logout
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
        #endregion

        #region Active Account
        public IActionResult ActivateAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }
        #endregion

        #region ForgotPassword
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }
            string fixedemail = FixedText.FixedEmail(forgot.Email);
            DataLayer.Entities.User.User user = _userService.GetUserByEmail(fixedemail);
            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
                return View(forgot);

            }
            string bodyemail = _renderView.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی کلمه عبور", bodyemail);
            ViewBag.IsSuccess = true;
            return View();
        }
        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }

      
        [HttpPost]

        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            DataLayer.Entities.User.User user = _userService.GetUserByActiveCode(reset.ActiveCode);
            if (user == null)
                return NotFound();

            string HashnewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = HashnewPassword;
            _userService.UpdateUser(user);
            ViewBag.PasswordResetSuccessfully = true;
            return View();

        }
        #endregion

        #region Change Mail Account
        public IActionResult ChangeMailAccount(string userId,string token,string mail)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (mail == null) throw new ArgumentNullException(nameof(mail));

            if (HttpContext.Request.Query["userId"] !=""
                && HttpContext.Request.Query["token"] !=""
                && HttpContext.Request.Query["mail"] !="")
            {
                int UserId = Convert.ToInt32(userId);
                token = HttpContext.Request.Query["token"];
                mail = HttpContext.Request.Query["mail"];
                if (_userService.ChangeUserEmail(UserId, token, mail))
                {
                    ViewBag.IsSuccess = true;
                }
                else
                {
                    ViewBag.IsSuccess = false;
                }
            }
            return View();
        }
        #endregion
      

    }
}