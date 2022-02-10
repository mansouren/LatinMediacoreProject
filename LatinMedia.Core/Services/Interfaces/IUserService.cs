using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.DataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.Services.Interfaces
{
   public interface IUserService
    {
        #region Account
        bool IsExistEmail(string email);
        bool IsExistMobile(string mobile);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string activecode);
        int GetUserIdByEmail(string email);
        User GetUserById(int userId);
        void UpdateUser(User user);
        bool ActiveAccount(string activecode);

        #endregion

        #region UserPanel
        InformationUserViewModel GetUserinformation(string email);
        InformationUserViewModel GetUserinformation(int userId);
        EditProfileViewModel GetDataForEditProfileUser(string email);
        void EditProfile(string email, EditProfileViewModel profile);
        bool CompareOldPassword(string email, string oldPassword);
        void ChangeUserPassword(string email, string newPassword);
        bool ChangeUserEmail(int userId, string token, string newEmail);
        #endregion

        #region Wallet
        int BalanceWalletUser(string email);

        List<WalletInfoViewModel> GetWalletUser(string email);

        int ChargeWallet(string email, int amount, string description, bool ispay = false);
        int Addwallet(Wallet wallet);
        Wallet GetWalletbyWalletId(int walletid);
        void UpdateWallet(Wallet wallet);
        #endregion

        #region Admin Panel
        SideBarAdminPanelViewModel GetSideBarAdminPanelData(string email);
        UsersForAdminViewModel GetUsers(int pageId=1,int take=10, string filterByEmail = "", string filterByMobile = "");
        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, int take = 10, string filterByEmail = "", string filterByMobile = "");
        int AddUserFromAdmin(CreateUserViewModel model);
        EditUserViewModel GetUserForshowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel edituser);
        void DeleteUser(int UserId);
        int UserFinalCount();
        List<User> GetLatestRegisteredUsers();
        #endregion

    }
}
