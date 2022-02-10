﻿// <auto-generated />
using System;
using LatinMedia.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LatinMedia.DataLayer.Migrations
{
    [DbContext(typeof(LatinMediaContext))]
    partial class LatinMediaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Company.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyImageName")
                        .HasMaxLength(200);

                    b.Property<string>("CompanyTitle")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("IsDelete");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.CommentCourse", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("CourseId");

                    b.Property<DateTime>("CreateDate");

                    b.Property<bool>("IsDelete");

                    b.Property<bool>("IsReadAdmin");

                    b.Property<int>("UserId");

                    b.HasKey("CommentId");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("CommentCourses");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId");

                    b.Property<int>("CountFiles");

                    b.Property<string>("CourseDescription")
                        .IsRequired();

                    b.Property<string>("CourseFaTitle")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<string>("CourseFileName")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("CourseImageName")
                        .HasMaxLength(100);

                    b.Property<string>("CourseLatinTitle")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.Property<int>("CoursePrice");

                    b.Property<int>("CourseTime");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("DemoFileName")
                        .HasMaxLength(500);

                    b.Property<int>("GroupId");

                    b.Property<bool>("IsDelete");

                    b.Property<bool>("IsSpecial");

                    b.Property<bool>("IsSubTitle");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("LevelId");

                    b.Property<int?>("SecondSubGroupId");

                    b.Property<int?>("SubGroupId");

                    b.Property<string>("Tags")
                        .HasMaxLength(600);

                    b.Property<int>("TeacherId");

                    b.Property<int>("TypeId");

                    b.Property<int>("Volume");

                    b.HasKey("CourseId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("GroupId");

                    b.HasIndex("LevelId");

                    b.HasIndex("SecondSubGroupId");

                    b.HasIndex("SubGroupId");

                    b.HasIndex("TeacherId");

                    b.HasIndex("TypeId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.CourseGroup", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("GroupImageName")
                        .HasMaxLength(100);

                    b.Property<string>("GroupTitle")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("IsDelete");

                    b.Property<int?>("ParentId");

                    b.HasKey("GroupId");

                    b.HasIndex("ParentId");

                    b.ToTable("CourseGroups");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.CourseLevel", b =>
                {
                    b.Property<int>("LevelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LevelTitle")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("LevelId");

                    b.ToTable("CourseLevels");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.CourseType", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeTitle")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("TypeId");

                    b.ToTable("CourseTypes");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.UserCourse", b =>
                {
                    b.Property<int>("UC_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseId");

                    b.Property<int>("UserId");

                    b.HasKey("UC_Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCourses");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Orders.Discount", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DiscountCode")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("DiscountPercent");

                    b.Property<DateTime?>("EndDate");

                    b.Property<DateTime?>("StartDate");

                    b.Property<int?>("UsableCount");

                    b.HasKey("DiscountId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Orders.OrderDetail", b =>
                {
                    b.Property<int>("DetailId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseId");

                    b.Property<int>("CoursePrice");

                    b.Property<int>("OrderCount");

                    b.Property<int>("OrderId");

                    b.HasKey("DetailId");

                    b.HasIndex("CourseId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Orders.Orders", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsFinally");

                    b.Property<DateTime>("OrderDate");

                    b.Property<int>("OrderSum");

                    b.Property<int>("UserId");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Permissions.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ParentId");

                    b.Property<string>("PermissionTitle")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("PermissionId");

                    b.HasIndex("ParentId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Permissions.RolePermission", b =>
                {
                    b.Property<int>("RP_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PermissionId");

                    b.Property<int>("RoleId");

                    b.HasKey("RP_Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Teacher.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDelete");

                    b.Property<string>("TeacherImageName")
                        .HasMaxLength(200);

                    b.Property<string>("TeacherNameEn")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("TeacherNameFa")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("TeacherId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.User.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDelete");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("RoleTitle")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.User.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActiveCode")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("UserAvatar")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.User.UserDiscountCode", b =>
                {
                    b.Property<int>("UD_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DiscountId");

                    b.Property<int>("UserId");

                    b.HasKey("UD_Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("UserId");

                    b.ToTable("UserDiscountCodes");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.User.UserRole", b =>
                {
                    b.Property<int>("UR_Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("UR_Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Wallet.Wallet", b =>
                {
                    b.Property<int>("WalletID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<bool>("IsPay");

                    b.Property<int>("TypeID");

                    b.Property<int>("UserID");

                    b.HasKey("WalletID");

                    b.HasIndex("TypeID");

                    b.HasIndex("UserID");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Wallet.WalletType", b =>
                {
                    b.Property<int>("TypeID");

                    b.Property<string>("TypeTitle")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("TypeID");

                    b.ToTable("walletTypes");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.CommentCourse", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Course.Course", "Course")
                        .WithMany("CommentCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.User.User", "User")
                        .WithMany("CommentCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.Course", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Company.Company", "Company")
                        .WithMany("Courses")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.Course.CourseGroup", "CourseGroup")
                        .WithMany("Courses")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.Course.CourseLevel", "CourseLevel")
                        .WithMany("Courses")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.Course.CourseGroup", "SecondSubGroup")
                        .WithMany("SecondSubGroupCourses")
                        .HasForeignKey("SecondSubGroupId");

                    b.HasOne("LatinMedia.DataLayer.Entities.Course.CourseGroup", "SubGroup")
                        .WithMany("SubGroupCourses")
                        .HasForeignKey("SubGroupId");

                    b.HasOne("LatinMedia.DataLayer.Entities.Teacher.Teacher", "Teacher")
                        .WithMany("Courses")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.Course.CourseType", "CourseType")
                        .WithMany("Courses")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.CourseGroup", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Course.CourseGroup")
                        .WithMany("CourseGroups")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Course.UserCourse", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Course.Course", "Course")
                        .WithMany("UserCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.User.User", "User")
                        .WithMany("UserCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Orders.OrderDetail", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Course.Course", "Course")
                        .WithMany("OrderDetails")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.Orders.Orders", "Orders")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Orders.Orders", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.User.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Permissions.Permission", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Permissions.Permission")
                        .WithMany("Permissions")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Permissions.RolePermission", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Permissions.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.User.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.User.UserDiscountCode", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Orders.Discount", "Discount")
                        .WithMany("UserDiscountCodes")
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.User.User", "User")
                        .WithMany("UserDiscountCodes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.User.UserRole", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.User.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.User.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LatinMedia.DataLayer.Entities.Wallet.Wallet", b =>
                {
                    b.HasOne("LatinMedia.DataLayer.Entities.Wallet.WalletType", "walletType")
                        .WithMany("Wallets")
                        .HasForeignKey("TypeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LatinMedia.DataLayer.Entities.User.User", "User")
                        .WithMany("Wallets")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
