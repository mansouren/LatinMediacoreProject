using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.web.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public List<LatinMedia.DataLayer.Entities.Orders.Orders> Orders { get; set; }
        public void OnGet()
        {
            Orders = _orderService.GetAllOrdersForAdmin();
        }
    }
}
