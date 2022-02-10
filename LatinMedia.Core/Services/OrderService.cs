using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;
using LatinMedia.DataLayer.Entities.Orders;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LatinMedia.DataLayer.Entities.Course;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities.User;

namespace LatinMedia.Core.Services
{
    public class OrderService : IOrderService
    {
        private LatinMediaContext _Context;
        IUserService _userService;
        public OrderService(LatinMediaContext Context, IUserService userService)
        {
            _Context = Context;
            _userService = userService;

        }

        public void AddDiscount(Discount discount)
        {
            _Context.Discounts.Add(discount);
            _Context.SaveChanges();
        }

        public int AddOrder(string email, int courseId)
        {
            int userId = _Context.Users.Single(u => u.Email == email).UserId;
            Orders orders = _Context.Orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinally);
            var course = _Context.Courses.Find(courseId);
            if (orders == null)
            {
                orders = new Orders()
                {
                    IsFinally = false,
                    OrderDate = DateTime.Now,
                    OrderSum = course.CoursePrice,
                    UserId = userId,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            CourseId=course.CourseId,
                            CoursePrice=course.CoursePrice,
                            OrderCount=1

                        }
                    }

                };
                _Context.Orders.Add(orders);
                _Context.SaveChanges();
            }
            else //اگر فاکتور بازی را پیدا کرد
            {
                OrderDetail detail = _Context.OrderDetails.FirstOrDefault(d => d.OrderId == orders.OrderId && d.CourseId == course.CourseId);
                if (detail != null)
                {
                    detail.OrderCount += 1;
                    _Context.OrderDetails.Update(detail);

                }
                else
                {
                    detail = new OrderDetail()
                    {
                        CourseId = course.CourseId,
                        CoursePrice = course.CoursePrice,
                        OrderCount = 1,
                        OrderId = orders.OrderId
                    };
                    _Context.OrderDetails.Add(detail);
                }
                _Context.SaveChanges();
                UpdateOrderPrice(orders.OrderId);
            }
            return orders.OrderId;
        }

        public bool FinalyOrder(string email, int orderId)
        {
            int userId = _Context.Users.Single(u => u.Email == email).UserId;
            var order = _Context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);
            if (order == null || order.IsFinally)
            {
                return false;
            }
            if (_userService.BalanceWalletUser(email) >= order.OrderSum)
            {
                order.IsFinally = true;
                _userService.Addwallet(new DataLayer.Entities.Wallet.Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    Description = "شماره فاکتور #" + order.OrderId,
                    IsPay = true,
                    TypeID = 2,
                    UserID = userId

                });
                _Context.Orders.Update(order);
                foreach (var detail in order.OrderDetails)
                {
                    _Context.UserCourses.Add(new UserCourse()
                    {
                        CourseId = detail.CourseId,
                        UserId = userId
                    });
                }
                _Context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Discount> GetAllDiscounts()
        {
            return _Context.Discounts.ToList();
        }

        public List<Orders> GetAllOrdersForAdmin()
        {
            return _Context.Orders.
                Include(c => c.User).
                Include(c => c.OrderDetails)
                .ThenInclude(od => od.Course)
                .OrderByDescending(c=>c.OrderDate)
                .ToList();
        }

        public List<UserCourse> GetCourseForDownload(string email)
        {
            int userId = _userService.GetUserIdByEmail(email);
            return _Context.UserCourses.Where(uc => uc.UserId == userId).Include(uc => uc.Course).ToList();
        }

        public Discount GetDiscountById(int discountId)
        {
            return _Context.Discounts.Find(discountId);
        }

        public int GetNotFinalyOrderCount()
        {
            return _Context.Orders.Count(o => o.IsFinally == false);
        }

        public Orders GetOrderById(int orderId)
        {
            return _Context.Orders.Find(orderId);
        }

        public List<OrderDetail> GetOrderDetailsById(int orderId)
        {
            return _Context.OrderDetails.Include(od => od.Course).Where(od => od.OrderId == orderId).ToList();
        }

        public Orders GetOrdersForUserPanel(string email, int orderId)
        {
            int userId = _Context.Users.Single(u => u.Email == email).UserId;
            return _Context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course).FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);
        }

        public List<Orders> GetUserOrders(string email)
        {
            int userId = _Context.Users.Single(u => u.Email == email).UserId;
            return _Context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public bool IsExistDiscount(string code)
        {
            return _Context.Discounts.Any(d => d.DiscountCode == code.ToLower());
        }

        public bool IsExistDiscountForEdit(int DiscountId, string code)
        {
            return _Context.Discounts.Any(d => d.DiscountCode == code.ToLower() && d.DiscountId != DiscountId);
        }

        public bool IsUserInCourse(string email, int courseId)
        {
            int userId = _userService.GetUserIdByEmail(email);
            return _Context.UserCourses.Any(c => c.UserId == userId && c.CourseId == courseId);

        }

        public int GetSumOrderCourses()
        {
            return _Context.Orders.Where(o=>o.IsFinally).Sum(o => o.OrderSum);
        }

        public void UpdateDiscount(Discount discount)
        {
            _Context.Discounts.Update(discount);
            _Context.SaveChanges();
        }

        public void UpdateOrder(Orders order)
        {
            _Context.Update(order);
            _Context.SaveChanges();
        }

        public void UpdateOrderPrice(int orderId)
        {
            var order = _Context.Orders.Find(orderId);
            order.OrderSum = _Context.OrderDetails.Where(d => d.OrderId == order.OrderId).Sum(d => d.CoursePrice);
            _Context.Orders.Update(order);
            _Context.SaveChanges();
        }

        public DiscountUseType UseDiscount(int orderId, string Code)
        {
            var discount = _Context.Discounts.SingleOrDefault(d => d.DiscountCode == Code);
            if (discount == null)
                return DiscountUseType.NotFound;
            if (discount.StartDate != null && discount.StartDate > DateTime.Now)
                return DiscountUseType.ExpireDate;
            if (discount.EndDate != null && discount.EndDate < DateTime.Now)
                return DiscountUseType.ExpireDate;
            if (discount.UsableCount != null && discount.UsableCount < 1)
                return DiscountUseType.Finished;

            var order = GetOrderById(orderId);
            if (_Context.UserDiscountCodes.Any(d => d.UserId == order.UserId && d.DiscountId == discount.DiscountId))
            {
                return DiscountUseType.Used;
            }
            int percent = (order.OrderSum * discount.DiscountPercent / 100);
            order.OrderSum = order.OrderSum - percent;
            UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }

            _Context.Discounts.Update(discount);
            _Context.UserDiscountCodes.Add(new UserDiscountCode()
            {
                UserId = order.UserId,
                DiscountId = discount.DiscountId

            });
            _Context.SaveChanges();

            return DiscountUseType.Success;
        }

        public List<Course> GetFullOrderItems()
        {
            return _Context.Courses.Include(o => o.OrderDetails).ThenInclude(od => od.Orders).Take(10).ToList();
        }
    }
}
