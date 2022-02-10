using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatinMedia.web.ViewComponents
{
    public class CourseGroupComponent:ViewComponent
    {
        private ICourseService _courseService;
        public CourseGroupComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<IViewComponentResult> InvokeAsync() //Invoke returns View in viewcomponenets
        {
            return await Task.FromResult((IViewComponentResult)View("CourseGroup",_courseService.GetAllGroups()));
        }
    }
}
