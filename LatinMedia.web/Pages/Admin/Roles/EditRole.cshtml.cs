using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditRoleModel : PageModel
    {
        private IPermissionService _permissionService;
        public EditRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        [BindProperty]
        public Role Role { get; set; }
        public void OnGet(int id)
        {
            Role = _permissionService.GetRoleById(id);
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
            ViewData["SelectedPermissions"] = _permissionService.PermissionRole(id);
        }
        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();
            _permissionService.UpdateRole(Role);
            _permissionService.UpdatePermissionsRole(Role.RoleId, SelectedPermission);
            return RedirectToPage("Index");
        }
    }
}