using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.Core.Security;

namespace LatinMedia.web.Pages.Admin.Roles
{
    [PermissionChecker(6)]
    public class IndexModel : PageModel
    {
        private IPermissionService _permissionService;
        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        public List<Role> RolesList { get; set; }
        public void OnGet()
        {
            RolesList = _permissionService.GetRoles();
        }
    }
}