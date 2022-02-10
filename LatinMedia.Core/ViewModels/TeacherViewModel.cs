using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.ViewModels
{
    public class ShowAllTeacherViewModel
    {
        public int TeacherId { get; set; }
        public string TeacherNameFa { get; set; }
        public string TeacherNameEn { get; set; }
        public string Description { get; set; }
        public string TeacherImageName { get; set; }
        public int CourseCount { get; set; }
    }
}
