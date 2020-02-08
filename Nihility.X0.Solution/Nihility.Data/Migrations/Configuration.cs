namespace Nihility.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Nihility.Model.Models.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NihiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// - Author: Hứa Minh Tuấn
        /// - Tạo dữ liệu mẫu mỗi khi update migration Database
        /// - Phương thức Seed sẽ được gọi sau khi migrating đến version cuối cùng
        /// - Có thể sử dụng DbSet<T>.AddOrUpdate() để tránh tạo dữ liệu trùng lập
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(NihiDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new NihiDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new NihiDbContext()));

            var user = new ApplicationUser
            {
                UserName = "DevTest",
                Email = "minhtuanit1992@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Hứa Minh Tuấn"
            };

            userManager.Create(user, "123456");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = userManager.FindByEmail(user.Email);

            userManager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }
    }
}
