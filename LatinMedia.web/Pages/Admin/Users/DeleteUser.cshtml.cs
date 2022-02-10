using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.ViewModels;
using LatinMedia.Core.Security;

namespace LatinMedia.web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeleteUserModel : PageModel
    {
        private IUserService _userService;
        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }
        public InformationUserViewModel  InformationUserViewModel { get; set; }
        public void OnGet(int id)
        {
            InformationUserViewModel = _userService.GetUserinformation(id);
            ViewData["UserId"] = id;
        }
        public IActionResult OnPost(int UserId)
        {
            _userService.DeleteUser(UserId);
            return RedirectToPage("Index");
        }
    }
}