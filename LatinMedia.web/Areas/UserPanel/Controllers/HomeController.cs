using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Senders;

namespace LatinMedia.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        public HomeController(IUserService userService,IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }
        public IActionResult Index()
        {
            return View(_userService.GetUserinformation(User.Identity.GetEmail()));
        }

        #region EditProfile
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_userService.GetDataForEditProfileUser(User.Identity.GetEmail()));
        }

        [HttpPost]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel Profile)
        {
            if (!ModelState.IsValid)
                return View(Profile);

            _userService.EditProfile(User.Identity.GetEmail(), Profile);

            //--------LogOut User-------------//
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }
        #endregion

        #region ChangePassword
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            string currentuser = User.Identity.GetEmail();
            if(!ModelState.IsValid)
            
                return View(change);
            
            if(!_userService.CompareOldPassword(currentuser,change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد");
                return View(change);
            }
            _userService.ChangeUserPassword(currentuser, change.Password);
            ViewBag.IsSuccess = true;

            return View();
        }
        #endregion



        #region Change Mail
        [Route("UserPanel/ChangeMail")]
        public IActionResult ChangeMail()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangeMail")]
        public IActionResult ChangeMail(ChaneMailViewModel mail)
        {
            if(!ModelState.IsValid)
            {
                return View(mail);
            }
            if(_userService.IsExistEmail(mail.Email))
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                return View(mail);
            }
            var user = _userService.GetUserByEmail(User.Identity.GetEmail());
            ChaneMailViewModel viewModel = new ChaneMailViewModel();
            string Email = mail.Email;
            viewModel.Email = EncryptData.Encrypt( mail.Email);
            viewModel.Token = user.ActiveCode;
            viewModel.UserId = user.UserId;

            #region Send Email for Change Mail
            string bodyemail = _viewRender.RenderToStringAsync("_ChangeMail", viewModel);
            SendEmail.Send(Email, "تغییر ایمیل کاربری", bodyemail);
            #endregion

            //--------LogOut User-------------//
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?ChangeMail=true");
        }

        #endregion


    }
}