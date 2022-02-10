using LatinMedia.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Company;
using LatinMedia.DataLayer.Entities.Teacher;

namespace LatinMedia.Core.Services.Interfaces
{
   public interface ICourseService
    {
        #region Groups
        List<CourseGroup> GetAllGroups();
        List<SelectListItem> GetGroupForManageCourses();
        List<SelectListItem> GetSubGroupForManageCourses(int GroupId);
        List<SelectListItem> GetSecondSubGroupForManageCourses(int SubGroupId);
        void AddCourseGroup(CourseGroup group);
        void UpdateCourseGroup(CourseGroup group);
        CourseGroup GetCourseGroupById(int groupId);
        #endregion

        #region Teachers
        List<Teacher> GetAllTeachers();
        List<SelectListItem> GetTeachersForManageCourse();
        List<ShowAllTeacherViewModel> ShowAllTeachers();
        int GetTeachersCount();
        #endregion

        #region Levels
        List<SelectListItem> GetLevelsForManageCourse();

        #endregion

        #region company
        List<Company> GetAllCompanies();
        List<SelectListItem> GetCompaniesForManageCourse();
        List<ShowAllCompaniesViewModel> showAllCompanies();
        int GetCompaniesCount();
        #endregion

        #region Course Types
        List<SelectListItem> GetCourseTypesForManageCourse();
        List<CourseType> GetAllCourseTypes();
        #endregion

        #region Course
        Tuple<List<ShowCourseForAdminViewModel>,int> GetCoursesForAdmin(int pageId=1,int take=1,string filterByName="",string filterByCompany="");
        int AddCourse(Course course, IFormFile imgCourseUp, IFormFile courseFile);
        Course GetCourseById(int CourseId);
        void UpdateCourse(Course course, IFormFile imageCourse, IFormFile courseFile);
        Tuple<List<ShowCourseListitemViewModel>,int> GetCourses(int pageId=1,string filter="",int type=0,
            int minPrice=0,int maxPrice=0,List<int> selectedGroup=null, int take = 0,int companyId=0,int teacherId=0);
        List<ShowCourseListitemViewModel> GetSpecialCourses();
        List<ShowCourseListitemViewModel> GetPopularCourses();
        Course GetCourseForShow(int id);
        int CourseCount();
        int GetSumTimeCourses();
        #endregion
        #region Comments
        void AddComment(CommentCourse comment);
        Tuple<List<CommentCourse>,int> GetCourseComments(int courseId, int PageId=1);
        int CourseCommentCount();
        #endregion

    }
}
