using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LatinMedia.Core.ViewModels;
using LatinMedia.Core.Security;

namespace LatinMedia.web.Pages.Admin.Courses
{
    [PermissionChecker(10)]
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;
        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public Tuple<List<ShowCourseForAdminViewModel>,int> CourseList { get; set; }
        public void OnGet(int pageId = 1, int take = 1, string filterByName = "", string filterByCompany = "")

        {
            if (pageId > 1)
            {
                ViewData["Take"] = (pageId - 1) * take + 1;
            }
            else
            {
                ViewData["Take"] = take;
            }
            ViewData["FilterName"] = filterByName;
            ViewData["FilterCompany"] = filterByCompany;
            ViewData["PageID"] = (pageId - 1) * take + 1;
            CourseList = _courseService.GetCoursesForAdmin(pageId, take, filterByName, filterByCompany);
            //CourseList = _courseService.GetCoursesForAdmin();
        }
    }
}