using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LatinMedia.web.Pages.Admin.CourseGroups
{
    [PermissionChecker(18)]
    public class IndexModel : PageModel
    {
        private ICourseService _CourseService;
        public IndexModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }
        public List<CourseGroup> CourseGroups { get; set; }
        public void OnGet()
        {
            CourseGroups = _CourseService.GetAllGroups();
        }
    }
}
