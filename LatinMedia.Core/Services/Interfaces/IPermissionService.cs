using LatinMedia.DataLayer.Entities.Permissions;
using LatinMedia.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.Services.Interfaces
{
   public interface IPermissionService
    {
        #region Role
        List<Role> GetRoles();
        void AddRolesTouser(List<int> roleIds, int userId);
        void EditRolesUser(int userId, List<int> rolesId);
        void RemoveRoleUser(int userId);
        List<string> GetUserRoles(int userId);
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        #endregion
        #region Permission
        List<Permission> GetAllPermissions();
        void AddPermissionsToRoles(int roleId, List<int> permissions);
        List<int> PermissionRole(int roleId);
        void UpdatePermissionsRole(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, string email);
        bool CheckUserHasRole(string email);
        #endregion
    }
}
