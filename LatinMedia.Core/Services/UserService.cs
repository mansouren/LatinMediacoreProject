using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using LatinMedia.DataLayer.Entities.User;
using LatinMedia.Core.ViewModels;
using LatinMedia.Core.Security;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Generators;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using LatinMedia.DataLayer.Entities.Wallet;
namespace LatinMedia.Core.Services
{
    public class UserService : IUserService
    {
        private LatinMediaContext _Context;
        private IHostingEnvironment _environment;
        private IPermissionService _permissionService;
        public UserService(LatinMediaContext context, IHostingEnvironment environment,IPermissionService permissionService)
        {
            _Context = context;
            _environment = environment;
            _permissionService = permissionService;
        }

        public bool IsExistEmail(string email)
        {
            return _Context.Users.Any(u => u.Email == email);
        }

        public bool IsExistMobile(string mobile)
        {
            return _Context.Users.Any(u => u.Mobile == mobile);
        }
        public int AddUser(User user)
        {
            _Context.Users.Add(user);
            _Context.SaveChanges();
            return user.UserId;
        }
        public User LoginUser(LoginViewModel login)
        {
            string password = PasswordHelper.EncodePasswordMd5(login.Password);
            string email = FixedText.FixedEmail(login.Email);
            return _Context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }
        public User GetUserByEmail(string email)
        {
            return _Context.Users.SingleOrDefault(u => u.Email == email);
        }
        public User GetUserByActiveCode(string activecode)
        {
            return _Context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
        }
        public int GetUserIdByEmail(string email)
        {
            return _Context.Users.Single(u => u.Email == email).UserId;
        }
        public void UpdateUser(User user)
        {
            _Context.Update(user);
            _Context.SaveChanges();
        }
        public bool ActiveAccount(string activecode)
        {
            var user = _Context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = GeneratorName.GenrateUniqeCode();

            _Context.Users.Update(user);
            _Context.SaveChanges();
            return true;
        }
                                  
              

        public InformationUserViewModel GetUserinformation(string email)
        {
            var user = GetUserByEmail(email);
            InformationUserViewModel information = new InformationUserViewModel();
            information.Email = user.Email;
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.Wallet = BalanceWalletUser(email);
            return information;
        }

        public EditProfileViewModel GetDataForEditProfileUser(string email)
        {
            return _Context.Users.Where(u => u.Email == email).Select(u => new EditProfileViewModel
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Mobile = u.Mobile,
                AvatarName = u.UserAvatar,
                Email=u.Email

            }).Single();
        }

        public void EditProfile(string email, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath ="";
                if (profile.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                profile.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByEmail(email);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.Mobile = profile.Mobile;
            user.UserAvatar = profile.AvatarName;

            UpdateUser(user);
        }

        public bool CompareOldPassword(string email, string oldPassword)
        {
            string hashOldPassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _Context.Users.Any(u => u.Email == email && u.Password == hashOldPassword);
        }

        public void ChangeUserPassword(string email, string newPassword)
        {
            var user = GetUserByEmail(email);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(user);
        }
     
        public int BalanceWalletUser(string email)
        {
            int userid = GetUserIdByEmail(email);
            //--------variz------------------------------------------------------//
            var deposit = _Context.Wallets.Where(w => w.UserID == userid
                                                      && w.TypeID == 1
                                                      && w.IsPay == true).Select(w=>w.Amount);
            //---------Bardasht------------------------------------------------//
            var removal = _Context.Wallets.Where(w => w.UserID == userid
                                                    && w.TypeID == 2
                                                    && w.IsPay == true).Select(w => w.Amount);

            return (deposit.Sum() - removal.Sum());
        }

        public List<WalletInfoViewModel> GetWalletUser(string email)
        {
            int userid = GetUserIdByEmail(email);
            return _Context.Wallets.Where(w => w.UserID == userid && w.IsPay).Select(w => new WalletInfoViewModel()
            {
                DateTime = w.CreateDate,
                Description = w.Description,
                Type = w.TypeID,
                Amount = w.Amount
            }).ToList();
        }

        public int ChargeWallet(string email, int amount, string description, bool ispay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Today,
                Description = description,
                IsPay = ispay,
                UserID = GetUserIdByEmail(email),
                TypeID = 1
            };
            return Addwallet(wallet);
        }

        public int Addwallet(Wallet wallet)
        {
            _Context.Wallets.Add(wallet);
            _Context.SaveChanges();
            return wallet.WalletID;
        }

        public Wallet GetWalletbyWalletId(int walletid)
        {
            return _Context.Wallets.Find(walletid);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _Context.Wallets.Update(wallet);
            _Context.SaveChanges();
        }

        public SideBarAdminPanelViewModel GetSideBarAdminPanelData(string email)
        {
            return _Context.Users.Where(u => u.Email == email).Select(u => new SideBarAdminPanelViewModel
            {
                FirstName=u.FirstName,
                LastName=u.LastName,
                AvatarName=u.UserAvatar

            }).Single();
        }

        public UsersForAdminViewModel GetUsers(int pageId = 1, int take = 1, string filterByEmail = "", string filterByMobile = "")
        {
            IQueryable<User> result = _Context.Users;//LazyLoad
            if(!string.IsNullOrEmpty(filterByEmail))
            {
                result = result.Where(u => u.Email.Contains(filterByEmail));
            }
            if(!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile == filterByMobile);
            }
            int takedata = take;
            int skip = (pageId - 1) * takedata;
            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takedata);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takedata).ToList();
            list.UserCounts = _Context.Users.Count();
            return list;
        }

        public int AddUserFromAdmin(CreateUserViewModel model)
        {
            User user = new User();
            user.Email = model.Email;
            user.Password = PasswordHelper.EncodePasswordMd5(model.Password);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.CreateDate = DateTime.Now;
            user.IsActive = model.IsActive;
            user.Mobile = model.Mobile;
            user.ActiveCode = GeneratorName.GenrateUniqeCode();
          
            #region Save Avatar

            if (model.UserAvatar != null)
            {
                string imagePath = "";

                //-------Upload New User Image --------//
                user.UserAvatar = GeneratorName.GenrateUniqeCode() + Path.GetExtension(model.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.UserAvatar.CopyTo(stream);
                }
            }

            #endregion

            return AddUser(user);
        }

        public EditUserViewModel GetUserForshowInEditMode(int userId)
        {
            return _Context.Users.Where(u => u.UserId == userId).Select(u => new EditUserViewModel
            {
                FirstName = u.FirstName,
                AvatarName = u.UserAvatar,
                Email = u.Email,
                IsActive = u.IsActive,
                LastName = u.LastName,
                Mobile = u.Mobile,
                UserId = u.UserId,
                UserRoles = u.UserRoles.Select(r => r.RoleId).ToList()

            }).Single();
        }

        public User GetUserById(int userId)
        {
            return _Context.Users.Find(userId);
        }

        public void EditUserFromAdmin(EditUserViewModel edituser)
        {
            var user = GetUserById(edituser.UserId);
            user.FirstName = edituser.FirstName;
            user.LastName = edituser.LastName;
            user.Mobile = edituser.Mobile;
            user.IsActive = edituser.IsActive;

            if(!string.IsNullOrEmpty(edituser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(edituser.Password);
            }

            if (edituser.UserAvatar != null)
            {
                string imagePath = "";
                if (edituser.AvatarName != "default.png")
                {
                    //------Delete User Image --------//
                    imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", edituser.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                }
                //-------Upload New User Image --------//
                edituser.AvatarName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(edituser.UserAvatar.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "UserAvatar", edituser.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    edituser.UserAvatar.CopyTo(stream);
                }
            }

            user.UserAvatar = edituser.AvatarName;
            _Context.Users.Update(user);
            _Context.SaveChanges();
        }

        public UsersForAdminViewModel GetDeleteUsers(int pageId = 1, int take = 10, string filterByEmail = "", string filterByMobile = "")
        {
            IQueryable<User> result = _Context.Users.IgnoreQueryFilters().Where(u=>u.IsDelete);//LazyLoad
            if (!string.IsNullOrEmpty(filterByEmail))
            {
                result = result.Where(u => u.Email.Contains(filterByEmail));
            }
            if (!string.IsNullOrEmpty(filterByMobile))
            {
                result = result.Where(u => u.Mobile == filterByMobile);
            }
            int takedata = take;
            int skip = (pageId - 1) * takedata;
            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(result.Count() / (double)takedata);
            list.Users = result.OrderByDescending(u => u.CreateDate).Skip(skip).Take(takedata).ToList();
            list.UserCounts = _Context.Users.Count();
            return list;
        }

        public void DeleteUser(int UserId)
        {
            var user = GetUserById(UserId);
            user.IsDelete = true;
            UpdateUser(user);
            _permissionService.RemoveRoleUser(user.UserId);
        }

        public InformationUserViewModel GetUserinformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.Email = user.Email;
            information.FirstName = user.FirstName;
            information.LastName = user.LastName;
            information.Mobile = user.Mobile;
            information.RegisterDate = user.CreateDate;
            information.Wallet = BalanceWalletUser(user.Email);
            return information;
        }

        public bool ChangeUserEmail(int userId, string token, string newEmail)
        {
           var user= _Context.Users.SingleOrDefault(u => u.UserId == userId && u.ActiveCode == token);
            if(user !=null)
            {
                user.Email = EncryptData.Decrypt(newEmail);
                user.ActiveCode = GeneratorName.GenrateUniqeCode();
                UpdateUser(user);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public int UserFinalCount()
        {
            return _Context.Users.Count(u => u.IsActive);
        }

        public List<User> GetLatestRegisteredUsers()
        {
            return _Context.Users.OrderByDescending(u=>u.CreateDate).Take(5).ToList();
        }
    }
}