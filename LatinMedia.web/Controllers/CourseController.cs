using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userService;
        public CourseController(ICourseService courseService,IOrderService orderService,IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
        }
        public IActionResult Index(int pageId = 1, string filter = "", int type = 0,
            int minPrice = 0, int maxPrice = 0, List<int> selectedGroup = null,int companyId=0,int teacherId=0)
        {
            ViewBag.Groups = _courseService.GetAllGroups();
            ViewBag.CourseTypes = _courseService.GetAllCourseTypes();
            ViewBag.Companies = _courseService.GetAllCompanies();
            ViewBag.Teachers = _courseService.GetAllTeachers();
            ViewData["SelectedGroup"] = selectedGroup;
            //..................................
            ViewBag.SelectedType = type;
            ViewBag.SelectedGroup = selectedGroup;
            ViewBag.SelectedTeacher = teacherId;
            ViewBag.SelectedCompany = companyId;
            ViewBag.PageId = pageId;
            var resault = _courseService.GetCourses(pageId, filter, type, minPrice, maxPrice, selectedGroup, 1,companyId,teacherId);
            return View("Index",model:resault);
        }

        public IActionResult Companies()
        {
            return View(_courseService.showAllCompanies());
        }

        public IActionResult Teachers()
        {
            return View(_courseService.ShowAllTeachers());
        }
        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id)
        {
            var course = _courseService.GetCourseForShow(id);
            if(course==null)
            {
                return NotFound();
            }
            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
            int orderId = _orderService.AddOrder(User.Identity.GetEmail(), id);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId);
        }
       
        [HttpPost]
        public IActionResult CreatComment(CommentCourse commentcourse)
        {
            commentcourse.UserId = _userService.GetUserIdByEmail(User.Identity.GetEmail());
            commentcourse.CreateDate = DateTime.Now;
            commentcourse.IsDelete = false;
            commentcourse.IsReadAdmin = false;
            _courseService.AddComment(commentcourse);
            return View("ShowComments",_courseService.GetCourseComments(commentcourse.CourseId));
        }
        public IActionResult ShowComments(int id,int pageId=1)
        {
            return View(_courseService.GetCourseComments(id, pageId));
        }
    }
}