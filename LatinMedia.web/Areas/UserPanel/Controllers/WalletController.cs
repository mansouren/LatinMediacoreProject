using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LatinMedia.Core.ViewModels;
namespace LatinMedia.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private IUserService _userService;
        public WalletController(IUserService userService)
        {
            _userService = userService;
        }
           
        [Route("UserPanel/Wallet")]

        public IActionResult Index()
        {
            ViewBag.WalletList = _userService.GetWalletUser(User.Identity.GetEmail());
            return View();
        }

        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Index(chargeWalletViewModel charge)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.WalletList = _userService.GetWalletUser(User.Identity.GetEmail());
                return View(charge);
            }
           int walletid=_userService.ChargeWallet(User.Identity.GetEmail(), charge.Amount, "شارژ حساب", false);
            #region Online Payment by Zarinpal
            var payment = new ZarinpalSandbox.Payment(charge.Amount);
            var response = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44391/OnlinePayment/" + walletid, "info@latinmedia.com");
            if(response.Result.Status==100)//is success
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + response.Result.Authority);
            }
            #endregion
            return null;
        }

       
    }
}