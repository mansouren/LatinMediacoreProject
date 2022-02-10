using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LatinMedia.DataLayer.Entities.Course
{
    public class UserCourse
    {
        [Key]
        public int UC_Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
       
        #region Relations
        public Course Course { get; set; }
        public User.User User { get; set; }
        #endregion
    }
}
