using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.ViewModels;
using LatinMedia.Core.Security;
using Microsoft.AspNetCore.Authorization;

namespace LatinMedia.web.Pages.Admin
{
    [UserRoleChecker]
    public class ChangePasswordModel : PageModel
    {
        private IUserService _userService;
        public ChangePasswordModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public void OnGet()
        {

            
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            if (!_userService.CompareOldPassword(User.Identity.GetEmail(), ChangePasswordViewModel.OldPassword))
            {
                ViewData["CssClass"] = "alert alert-danger";
                ViewData["Message"] = "کلمه عبور فعلی صحیح نمی باشد";
                return Page();
            }
               
            _userService.ChangeUserPassword(User.Identity.GetEmail(), ChangePasswordViewModel.Password);
            ViewData["CssClass"] = "alert alert-success";
            ViewData["Message"] = "کلمه عبور با موفقیت به روزرسانی شد";
            return Page();
        }
    }
}