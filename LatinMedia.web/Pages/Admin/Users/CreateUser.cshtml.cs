using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Security;

namespace LatinMedia.web.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;

        public CreateUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> selectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }
            
            if(_userService.IsExistEmail(FixedText.FixedEmail(CreateUserViewModel.Email)))
               {
                ModelState.AddModelError("CreateUserViewModel.Email", "ایمیل وارد شده تکراری است");
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
               }
            if(_userService.IsExistMobile(CreateUserViewModel.Mobile))
            {
                ModelState.AddModelError("CreateUserViewModel.Mobile", "شماره موبایل وارد شده تکراری است");
                ViewData["Roles"] = _permissionService.GetRoles();
                return Page();
            }
            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);
            _permissionService.AddRolesTouser(selectedRoles, userId);
            return RedirectToPage("Index");
        }
    }
}