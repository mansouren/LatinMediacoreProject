using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.DataLayer.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.Core.Services.Interfaces
{
    public interface IOrderService
    {
        #region Orders

        int AddOrder(string email, int courseId);
        void UpdateOrderPrice(int orderId);
        Orders GetOrdersForUserPanel(string email, int orderId);
        Orders GetOrderById(int orderId);
        bool FinalyOrder(String email, int orderId);
        List<Orders> GetUserOrders(string email);
        void UpdateOrder(Orders order);
        bool IsUserInCourse(string email, int courseId);
        List<UserCourse> GetCourseForDownload(string email);
        List<Orders> GetAllOrdersForAdmin();
        List<OrderDetail> GetOrderDetailsById(int orderId);
        int GetNotFinalyOrderCount();
        int GetSumOrderCourses();
        List<Course> GetFullOrderItems();
        #endregion

        #region Discount
        DiscountUseType UseDiscount(int orderId, string Code);
        void AddDiscount(Discount discount);
        void UpdateDiscount(Discount discount);
        Discount GetDiscountById(int discountId);
        List<Discount> GetAllDiscounts();
        bool IsExistDiscount(string code);
        bool IsExistDiscountForEdit(int DiscountId,string code);
        #endregion
    }
}
