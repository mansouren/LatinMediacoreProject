using LatinMedia.Core.Convertors;
using LatinMedia.Core.Generators;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities.Company;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Teacher;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LatinMedia.Core.Services
{
    public class CourseService : ICourseService
    {
        private LatinMediaContext _context;
        private IHostingEnvironment _environment;
        public CourseService(LatinMediaContext context,IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void AddComment(CommentCourse comment)
        {
            _context.CommentCourses.Add(comment);
            _context.SaveChanges();
        }

        public int AddCourse(Course course, IFormFile imgCourseUp, IFormFile courseFile)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "no-photo.png";
            
            if(imgCourseUp !=null && imgCourseUp.IsImage())
            {
                string imagePath = "";
                //-------Upload New User Image --------//
                course.CourseImageName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(imgCourseUp.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "course/images", course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourseUp.CopyTo(stream);
                }
                #region Save Thumbnail Course Image

                ImageConvertors imageResizer = new ImageConvertors();
                string thumbPath = Path.Combine(_environment.WebRootPath, "course/thumbnail", course.CourseImageName);
                imageResizer.Image_resize(imagePath, thumbPath, 150);

                #endregion


            }

            if(courseFile !=null)
            {
                string FilePath = "";
                
                course.CourseFileName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(courseFile.FileName);
                FilePath = Path.Combine(_environment.WebRootPath, "course/download", course.CourseFileName);
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    courseFile.CopyTo(stream);
                }
            }

            _context.Courses.Add(course);
            _context.SaveChanges();
            return course.CourseId;

        }

        public void AddCourseGroup(CourseGroup group)
        {
            _context.CourseGroups.Add(group);
            _context.SaveChanges();
        }

        public int CourseCommentCount()
        {
            return _context.CommentCourses.Count(c => c.IsReadAdmin == false);
        }

        public int CourseCount()
        {
            return _context.Courses.Count();
        }

        public List<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }

        public List<CourseType> GetAllCourseTypes()
        {
            return _context.CourseTypes.ToList();
        }

        public List<CourseGroup> GetAllGroups()
        {
            return _context.CourseGroups.Include(g=>g.CourseGroups).ToList();
        }

        public List<Teacher> GetAllTeachers()
        {
            return _context.Teachers.ToList();
        }

        public int GetCompaniesCount()
        {
            return _context.Companies.Count();
        }

        public List<SelectListItem> GetCompaniesForManageCourse()
        {
            return _context.Companies.Select(c => new SelectListItem()
            {
                Text = c.CompanyTitle,
                Value = c.CompanyId.ToString()
            }).ToList();
        }

        public Course GetCourseById(int CourseId)
        {
            return _context.Courses.Find(CourseId);
        }

        public Tuple<List<CommentCourse>, int> GetCourseComments(int courseId, int PageId=1)
        {
            int take = 6;
            int skip =(PageId - 1) * take;
            int pageCount =(int)Math.Ceiling( _context.CommentCourses.Include(c => c.User)
                .Count(c => c.CourseId == courseId && !c.IsDelete)/(double)take);

            var result = _context.CommentCourses.Include(c => c.User).Where(c => c.CourseId == courseId && !c.IsDelete)
                .OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
            return Tuple.Create(result, pageCount);
        }

        public Course GetCourseForShow(int id)
        {
            return _context.Courses.Include(c => c.Company)
                .Include(c => c.CourseLevel)
                .Include(c => c.Teacher)
                .Include(c=>c.CourseGroup)
                .Include(c=>c.SubGroup)
                .Include(c=>c.UserCourses)
                .Include(c=>c.SecondSubGroup)
                .FirstOrDefault(c => c.CourseId == id);
        }

        public CourseGroup GetCourseGroupById(int groupId)
        {
            return _context.CourseGroups.Find(groupId);
        }

        public Tuple<List<ShowCourseListitemViewModel>, int> GetCourses(int pageId = 1, string filter = "", int type = 0,
            int minPrice = 0, int maxPrice = 0, List<int> selectedGroup = null, int take = 0, int companyId = 0, int teacherId = 0)
        {
            IQueryable<Course> resault = _context.Courses;
            if (take == 0)
                take = 8;
            if(!(string.IsNullOrEmpty(filter)))
            {
                resault = resault.Where(c => c.CourseFaTitle.Contains(filter) || c.CourseLatinTitle.Contains(filter) || c.Tags.Contains(filter));
            }
            if(type>0)
            {
                resault = resault.Where(c => c.TypeId == type);
            }
            if(minPrice>0)
            {
                resault = resault.Where(c => c.CoursePrice > minPrice);
            }
            if(maxPrice>0)
            {
                resault = resault.Where(c => c.CoursePrice < maxPrice);
            }
            if(selectedGroup !=null && selectedGroup.Any())
            {
                foreach (var groupId in selectedGroup)
                {
                    resault = resault.Where(c => c.GroupId == groupId || c.SubGroupId == groupId || c.SecondSubGroupId == groupId);
                }
            }
            if(companyId > 0)
            {
                resault = resault.Where(c => c.CompanyId == companyId);
            }
            if(teacherId > 0)
            {
                resault = resault.Where(c => c.TeacherId == teacherId);
            }
            int skip = (pageId - 1) * take;
            int pageCount =(int) Math.Ceiling( resault.Include(c => c.Teacher).Include(c => c.Company)
                .Select(c => new ShowCourseListitemViewModel()
                {
                    CourseId = c.CourseId,
                    CourseTitle = c.CourseFaTitle,
                    CourseImage = c.CourseImageName,
                    CoursePrice = c.CoursePrice,
                    CourseTime = c.CourseTime,
                    Company = c.Company.CompanyTitle,
                    Teacher = c.Teacher.TeacherNameFa,
                    TeacherImage = c.Teacher.TeacherImageName

                }).Count() /(double) take);
            var query= resault.Include(c => c.Teacher).Include(c => c.Company)
                .Select(c=> new ShowCourseListitemViewModel()
                {
                    CourseId=c.CourseId,
                    CourseTitle=c.CourseFaTitle,
                    CourseImage=c.CourseImageName,
                    CoursePrice=c.CoursePrice,
                    CourseTime=c.CourseTime,
                    Company=c.Company.CompanyTitle,
                    Teacher=c.Teacher.TeacherNameFa,
                    TeacherImage=c.Teacher.TeacherImageName

                }).Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount); 

        }

        public Tuple<List<ShowCourseForAdminViewModel>,int> GetCoursesForAdmin(int pageId = 1, int take = 0, string filterByName = "", string filterByCompany = "")
        {
            IQueryable<Course> result = _context.Courses;
            if (take == 0)
                take = 2;
            if (!(string.IsNullOrEmpty(filterByName)))
            {
               result=result.Where(c => c.CourseFaTitle.Contains(filterByName) || c.CourseLatinTitle.Contains(filterByName));
            }
            if(!(string.IsNullOrEmpty(filterByCompany)))
            {
                result = result.Where(c => c.Company.CompanyTitle.Contains(filterByCompany));
            }
            int takedata = take;
            int skip = (pageId - 1) * takedata;

            int pageCount = (int)Math.Ceiling(_context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                Company = c.Company.CompanyTitle,
                CourseId = c.CourseId,
                CourseType = c.CourseType.TypeTitle,
                CourseFaTitle = c.CourseFaTitle,
                CourseLatinTitle = c.CourseLatinTitle,
                CourseTime = c.CourseTime,
                CoursePrice = c.CoursePrice,
                CourseImageName = c.CourseImageName,
                CreateDate = c.CreateDate,
                IsSpecial = c.IsSpecial,
                GroupId = c.CourseGroup.GroupTitle,
                SubGroupId = c.SubGroup.GroupTitle,
                SecondSubGroupId = c.SecondSubGroup.GroupTitle,
                //CurrentPage = pageId,
                //PageCount = (int)Math.Ceiling(result.Count() / (double)takedata),
                //Courses = result.OrderByDescending(co => co.CreateDate).Skip(skip).Take(takedata).ToList(),
                //CourseCounts = _context.Courses.Count()
            }).Count()/(double) take);

            var query= _context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                Company = c.Company.CompanyTitle,
                CourseId = c.CourseId,
                CourseType = c.CourseType.TypeTitle,
                CourseFaTitle = c.CourseFaTitle,
                CourseLatinTitle = c.CourseLatinTitle,
                CourseTime = c.CourseTime,
                CoursePrice = c.CoursePrice,
                CourseImageName = c.CourseImageName,
                CreateDate = c.CreateDate,
                IsSpecial = c.IsSpecial,
                GroupId = c.CourseGroup.GroupTitle,
                SubGroupId = c.SubGroup.GroupTitle,
                SecondSubGroupId = c.SecondSubGroup.GroupTitle,
                //CurrentPage = pageId,
                //PageCount = (int)Math.Ceiling(result.Count() / (double)takedata),
                //Courses = result.OrderByDescending(co => co.CreateDate).Skip(skip).Take(takedata).ToList(),
                //CourseCounts = _context.Courses.Count()
            }).OrderByDescending(co => co.CreateDate).Skip(skip).Take(takedata).ToList();

            return Tuple.Create(query, pageCount);
        }

        public List<SelectListItem> GetCourseTypesForManageCourse()
        {
            return _context.CourseTypes.Select(t => new SelectListItem()
            {
                Text = t.TypeTitle,
                Value = t.TypeId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetGroupForManageCourses()
        {
            return _context.CourseGroups.Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
            {
                Value=g.GroupId.ToString(),
                Text=g.GroupTitle
            }).ToList();
        }

        public List<SelectListItem> GetLevelsForManageCourse()
        {
            return _context.CourseLevels.Select(l => new SelectListItem()
            {
                Text =l.LevelTitle ,
                Value = l.LevelId.ToString()
            }).ToList();
        }

        public List<ShowCourseListitemViewModel> GetPopularCourses()
        {
            return _context.Courses.Include(c => c.Teacher).Include(c => c.Company).Include(c => c.OrderDetails)
                .Where(od => od.OrderDetails.Any()).OrderByDescending(od => od.OrderDetails.Count()).
                Select(c => new ShowCourseListitemViewModel()
                {
                    CourseId = c.CourseId,
                    CourseTitle = c.CourseFaTitle,
                    CourseImage = c.CourseImageName,
                    CoursePrice = c.CoursePrice,
                    CourseTime = c.CourseTime,
                    Company = c.Company.CompanyTitle,
                    Teacher = c.Teacher.TeacherNameFa,
                    TeacherImage = c.Teacher.TeacherImageName

                }).Take(8).ToList();
        }

        public List<SelectListItem> GetSecondSubGroupForManageCourses(int SubGroupId)
        {
            return _context.CourseGroups.Where(g => g.ParentId == SubGroupId)
               .Select(g => new SelectListItem()
               {
                   Value = g.GroupId.ToString(),
                   Text = g.GroupTitle
               }).ToList();
        }

        public List<ShowCourseListitemViewModel> GetSpecialCourses()
        {
            return _context.Courses.Include(c => c.Teacher).Include(c => c.Company)
             .Where(c=>c.IsSpecial).Select(c => new ShowCourseListitemViewModel()
             {
                 CourseId = c.CourseId,
                 CourseTitle = c.CourseFaTitle,
                 CourseImage = c.CourseImageName,
                 CoursePrice = c.CoursePrice,
                 CourseTime = c.CourseTime,
                 Company = c.Company.CompanyTitle,
                 Teacher = c.Teacher.TeacherNameFa,
                 TeacherImage = c.Teacher.TeacherImageName

             }).ToList();
        }

        public List<SelectListItem> GetSubGroupForManageCourses(int GroupId)
        {
            return _context.CourseGroups.Where(g => g.ParentId == GroupId)
               .Select(g => new SelectListItem()
               {
                   Value = g.GroupId.ToString(),
                   Text = g.GroupTitle
               }).ToList();
        }

        public int GetSumTimeCourses()
        {
            return _context.Courses.Sum(c => c.CourseTime);
        }

        public int GetTeachersCount()
        {
            return _context.Teachers.Count();
        }

        public List<SelectListItem> GetTeachersForManageCourse()
        {
            return _context.Teachers.Select(t => new SelectListItem()
            {
                Text = t.TeacherNameEn + "-" + t.TeacherNameFa,
                Value = t.TeacherId.ToString()
            }).ToList();
        }

        public List<ShowAllCompaniesViewModel> showAllCompanies()
        {
            return _context.Companies.Select(c => new ShowAllCompaniesViewModel()
            {
                CompanyId=c.CompanyId,
                CompanyTitle=c.CompanyTitle,
                CompanyImageName=c.CompanyImageName,
                CourseCount=c.Courses.Count(g=>g.CompanyId==c.CompanyId)
            }).ToList();
        }

        public List<ShowAllTeacherViewModel> ShowAllTeachers()
        {
            return _context.Teachers.Select(t => new ShowAllTeacherViewModel()
            {
              TeacherId=t.TeacherId,
              TeacherNameEn=t.TeacherNameEn,
              TeacherNameFa=t.TeacherNameFa,
              TeacherImageName=t.TeacherImageName,
              CourseCount=_context.Courses.Count(c=>c.TeacherId==t.TeacherId)
            }).ToList();
        }

        public void UpdateCourse(Course course, IFormFile imageCourse, IFormFile courseFile)
        {
            var currentDate = course.CreateDate;

            if (imageCourse != null && imageCourse.IsImage())
            {
                string imagePath = "";
                #region Remove Old Course Image
                if (course.CourseImageName != "no-photo.png")
                {
                    //------Delete Course Image --------//
                    string  DeleteImagePath = Path.Combine(_environment.WebRootPath, "course/images", course.CourseImageName);
                    if (File.Exists(DeleteImagePath))
                    {
                        File.Delete(DeleteImagePath);
                    }
                    //--------Delete Thumb Course Image------------//
                    string DeleteThumbPath= Path.Combine(_environment.WebRootPath, "course/thumbnail", course.CourseImageName);
                    if (File.Exists(DeleteThumbPath))
                    {
                        File.Delete(DeleteThumbPath);
                    }
                }
                #endregion

                #region Add New Course Image

                course.CourseImageName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(imageCourse.FileName);
                imagePath = Path.Combine(_environment.WebRootPath, "course/images", course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageCourse.CopyTo(stream);
                }


                ImageConvertors imageResizer = new ImageConvertors();
                string thumbPath = Path.Combine(_environment.WebRootPath, "course/thumbnail", course.CourseImageName);
                imageResizer.Image_resize(imagePath, thumbPath, 150);

                #endregion
               

            }

            if (courseFile != null)
            {
                string FilePath = "";
                string DeleteFilePath= Path.Combine(_environment.WebRootPath, "course/download", course.CourseFileName);
                if (File.Exists(DeleteFilePath))
                {
                    File.Delete(DeleteFilePath);
                }
                course.CourseFileName = GeneratorName.GenrateUniqeCode() + Path.GetExtension(courseFile.FileName);
                FilePath = Path.Combine(_environment.WebRootPath, "course/download", course.CourseFileName);
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    courseFile.CopyTo(stream);
                }
            }
             course.CreateDate = currentDate;
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void UpdateCourseGroup(CourseGroup group)
        {
            _context.CourseGroups.Update(group);
            _context.SaveChanges();
        }
    }
}
