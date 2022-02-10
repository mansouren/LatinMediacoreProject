using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LatinMedia.web.Pages.Admin.Courses
{
    [PermissionChecker(11)]
    public class CreateCoursesModel : PageModel
    {
        private ICourseService _courseService;
        public CreateCoursesModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public Course Course { get; set; }
        public void OnGet()
        {
            var groups = _courseService.GetGroupForManageCourses();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            var subGroup = _courseService.GetSubGroupForManageCourses(int.Parse(groups.First().Value));
            ViewData["SubGroup"] = new SelectList(subGroup, "Value", "Text");

            var secondSubGroup = _courseService.GetSecondSubGroupForManageCourses(int.Parse(subGroup.First().Value));
            ViewData["SecondSubGroup"] = new SelectList(secondSubGroup, "Value", "Text");

            var levels = _courseService.GetLevelsForManageCourse();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text");

            var types = _courseService.GetCourseTypesForManageCourse();
            ViewData["Types"] = new SelectList(types, "Value", "Text");

            var teachers = _courseService.GetTeachersForManageCourse();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

            var company = _courseService.GetCompaniesForManageCourse();
            ViewData["Companies"] = new SelectList(company, "Value", "Text");

        }

        public IActionResult OnPost(IFormFile imgCourseUp,IFormFile courseFile)
        {
            if(!ModelState.IsValid)
            {
                var groups = _courseService.GetGroupForManageCourses();
                ViewData["Groups"] = new SelectList(groups, "Value", "Text");

                var subGroup = _courseService.GetSubGroupForManageCourses(int.Parse(groups.First().Value));
                ViewData["SubGroup"] = new SelectList(subGroup, "Value", "Text");

                var secondSubGroup = _courseService.GetSecondSubGroupForManageCourses(int.Parse(subGroup.First().Value));
                ViewData["SecondSubGroup"] = new SelectList(secondSubGroup, "Value", "Text");

                var levels = _courseService.GetLevelsForManageCourse();
                ViewData["Levels"] = new SelectList(levels, "Value", "Text");

                var types = _courseService.GetCourseTypesForManageCourse();
                ViewData["Types"] = new SelectList(types, "Value", "Text");

                var teachers = _courseService.GetTeachersForManageCourse();
                ViewData["Teachers"] = new SelectList(teachers, "Value", "Text");

                var company = _courseService.GetCompaniesForManageCourse();
                ViewData["Companies"] = new SelectList(company, "Value", "Text");

                return Page();
            }
            _courseService.AddCourse(Course, imgCourseUp, courseFile);
            TempData["InsertCourse"] = true;
            return RedirectToPage("Index");
        }
    }
}