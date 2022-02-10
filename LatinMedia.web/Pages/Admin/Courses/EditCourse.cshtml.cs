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
    [PermissionChecker(12)]
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;
        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public Course Course { get; set; }
        public void OnGet(int id)
        {
            Course = _courseService.GetCourseById(id);

            var groups = _courseService.GetGroupForManageCourses();

            ViewData["Groups"] = new SelectList(groups, "Value", "Text",Course.GroupId);
            List<SelectListItem> subgrouplist = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value=""}
            };
            subgrouplist.AddRange(_courseService.GetSubGroupForManageCourses(Course.GroupId));
            string selectedsubgroup = null;
            if(Course.SubGroup !=null)
            {
                selectedsubgroup = Course.SubGroup.ToString();
            }
            
            ViewData["SubGroup"] = new SelectList(subgrouplist, "Value", "Text", selectedsubgroup);
            List<SelectListItem> secondsubgrouplist = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="انتخاب کنید",
                    Value=""
                }
            };
            secondsubgrouplist.AddRange(_courseService.GetSecondSubGroupForManageCourses(Course.SubGroupId ?? 0));
            string selectedsecondsubgroup = null;
            if(Course.SecondSubGroup !=null)
            {
                selectedsecondsubgroup = Course.SecondSubGroup.ToString();
            }
            
           
            ViewData["SecondSubGroup"] = new SelectList(secondsubgrouplist, "Value", "Text", Course.SecondSubGroupId ?? 0);

            var levels = _courseService.GetLevelsForManageCourse();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text",Course.LevelId);

            var types = _courseService.GetCourseTypesForManageCourse();
            ViewData["Types"] = new SelectList(types, "Value", "Text",Course.TypeId);

            var teachers = _courseService.GetTeachersForManageCourse();
            ViewData["Teachers"] = new SelectList(teachers, "Value", "Text",Course.TeacherId);

            var company = _courseService.GetCompaniesForManageCourse();
            ViewData["Companies"] = new SelectList(company, "Value", "Text",Course.CompanyId);
        }

        public IActionResult OnPost(IFormFile imgCourseUp, IFormFile courseFile)
        {
            if(!ModelState.IsValid)
            {
                var groups = _courseService.GetGroupForManageCourses();

                ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

                var subGroup = _courseService.GetSubGroupForManageCourses(int.Parse(groups.First().Value));
                ViewData["SubGroup"] = new SelectList(subGroup, "Value", "Text", Course.SubGroupId ?? 0);

                var secondSubGroup = _courseService.GetSecondSubGroupForManageCourses(int.Parse(subGroup.First().Value));
                ViewData["SecondSubGroup"] = new SelectList(secondSubGroup, "Value", "Text", Course.SecondSubGroupId ?? 0);

                var levels = _courseService.GetLevelsForManageCourse();
                ViewData["Levels"] = new SelectList(levels, "Value", "Text", Course.LevelId);

                var types = _courseService.GetCourseTypesForManageCourse();
                ViewData["Types"] = new SelectList(types, "Value", "Text", Course.TypeId);

                var teachers = _courseService.GetTeachersForManageCourse();
                ViewData["Teachers"] = new SelectList(teachers, "Value", "Text", Course.TeacherId);

                var company = _courseService.GetCompaniesForManageCourse();
                ViewData["Companies"] = new SelectList(company, "Value", "Text", Course.CompanyId);
                return Page();
            }
            _courseService.UpdateCourse(Course, imgCourseUp, courseFile);
            TempData["UpdateCourse"] = true;
            return RedirectToPage("Index");
        }
    }
}