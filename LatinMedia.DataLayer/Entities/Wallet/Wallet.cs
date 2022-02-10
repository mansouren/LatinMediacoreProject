﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Wallet
{
  public  class Wallet
    {
        [Key]
        public int WalletID { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int TypeID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int UserID { get; set; }

        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amount { get; set; }

        [Display(Name = "شرح")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0}نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Description { get; set; }

        [Display(Name = "تائید شده")]
        public bool IsPay { get; set; }

        [Display(Name = "تاریخ")]
        public DateTime CreateDate { get; set; }

        #region Relations
        public User.User User { get; set; }
        public WalletType walletType { get; set; }
        #endregion
    }
}
