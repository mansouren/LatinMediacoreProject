using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.web.Pages.Admin
{
    [UserRoleChecker]
    public class profileModel : PageModel
    {
        private IUserService _userService;
        public profileModel(IUserService userService)
        {
            _userService = userService;
        }
        public InformationUserViewModel informationUserViewModel{ get; set; }
        public void OnGet()
        {
            informationUserViewModel = _userService.GetUserinformation(User.Identity.GetEmail());
        }
    }
}