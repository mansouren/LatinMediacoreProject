using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrdersController : Controller
    {
       private IOrderService _orderService;
        private ICourseService _courseService;
        private IHostingEnvironment _hostingEnvironment;
        public MyOrdersController(IOrderService orderService, ICourseService courseService, IHostingEnvironment hostingEnvironment)
        {
            _orderService = orderService;
            _courseService = courseService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View(_orderService.GetUserOrders(User.Identity.GetEmail()));
        }
        public IActionResult ShowOrder(int id,bool finaly=false,string type="")
        {
            var order = _orderService.GetOrdersForUserPanel(User.Identity.GetEmail(), id);
            if(order==null)
            {
                return NotFound();
            }
            ViewBag.Finaly = finaly;
            ViewBag.Discount = type;
            return View(order);
        }
        public IActionResult FinalyOrder(int id)
        {
            if(_orderService.FinalyOrder(User.Identity.GetEmail(),id))
            {
                return Redirect("/UserPanel/MyOrders/ShowOrder/" + id + "?finaly=true");
            }
            return BadRequest();
        }

        public IActionResult UseDiscount(int orderId,string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId + "?type="+ type);
        }
        public IActionResult UserCourses()
        {
            return View(_orderService.GetCourseForDownload(User.Identity.GetEmail()));
        }
        [Route("DownloadFile/{courseId}")]
        public IActionResult DownloadFile(int courseId)
        {
            var course = _courseService.GetCourseById(courseId);
            if(course !=null)
            {
                if(User.Identity.IsAuthenticated)
                {
                    if(_orderService.IsUserInCourse(User.Identity.GetEmail(),course.CourseId))
                    {
                        string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "course/download", course.CourseFileName);
                        string filename = course.CourseFileName;
                        byte[] file = System.IO.File.ReadAllBytes(filepath);
                        return File(file,"application/force-download",filename);
                    }
                }
            }
            return Forbid();
        }
    }
}