using LatinMedia.DataLayer.Entities.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.User
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نیست")]
        public string Email { get; set; }
        [Display(Name = "پسورد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        //[DataType(Password)]
        public string Password { get; set; }
        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }
        [Display(Name = "کد فعال سازی")]
        [MaxLength(50, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ActiveCode { get; set; }
        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
        [Display(Name = "آواتار")]
        [MaxLength(50, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UserAvatar { get; set; }
        [Display(Name = "تاریخ ثبت نام")]
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }

        #region Relations
        public List<UserRole> UserRoles { get; set; }
        public List<Wallet.Wallet> Wallets { get; set; }
        public List<Orders.Orders> Orders  { get; set; }
        public List<Course.UserCourse> UserCourses { get; set; }
        public List<UserDiscountCode> UserDiscountCodes { get; set; }
        public List<CommentCourse> CommentCourses { get; set; }
        #endregion
    }
}
