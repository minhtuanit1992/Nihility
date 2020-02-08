using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Nihility.Data;
using Nihility.Data.Infrastructure;
using Nihility.Data.Interface;
using Nihility.Data.Repositories;
using Nihility.Model.Models.Identity;
using Nihility.Service;
using Nihility.X0.Solution.Models;
using Owin;

[assembly: OwinStartup(typeof(Nihility.X0.Solution.Startup))]

namespace Nihility.X0.Solution
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //CreateRoleAndUser();
            ConfigAutofac(app);
        }


        /// <summary>
        /// Author: Hứa Minh Tuấn
        /// - Cài đặt AutoFac để tiêm Dependency Injection cho các đối tượng một cách tự động
        /// - Nuget Packet cần cài đặt: AutoFac, Autofac.Mvc5, AutoFac.WebApi
        /// </summary>
        /// <param name="app"></param>
        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // Đăng ký tự động tiêm Dependency vào các Constructor của Controller
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Đăng ký tự động tiêm Dependency vào các Constructor của ApiController
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Đăng ký tự động tiêm Dependency vào Constructor của UnitOfWork Pattern
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            // Đăng ký tự động tiêm Dependency vào Constructor của DbFactory Pattern
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            // Đăng ký tự động tiêm Dependency vào Constructor của DbContext
            builder.RegisterType<NihiDbContext>().AsSelf().InstancePerRequest();

            // Đăng ký tự động tiêm Dependency vào Constructor của các Asp.net Identity
            //builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();
            // Repositories
            builder.RegisterAssemblyTypes(typeof(PostCategoryRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(PostCategoryService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            //DI
            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        /// - Trong hàm này sẽ khởi tạo mặc định Roles và Tài khoản Admin cho hệ thống   
        /// </summary>
        private void CreateRoleAndUser()
        {
            NihiDbContext context = new NihiDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Mặc định khởi tạo một Role và User Admin nếu chưa tồn tại     
            if (!roleManager.RoleExists("Admin"))
            {
                // Tạo Role
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Đây là một Siêu User sẽ quản lý trang web
                var user = new ApplicationUser();
                user.UserName = ConfigurationManager.AppSettings["SystemUser"].ToString();
                user.Email = ConfigurationManager.AppSettings["SystemEmail"].ToString();

                string userPWD = ConfigurationManager.AppSettings["SystemPassword"].ToString();
                var resultUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (resultUser.Succeeded)
                {
                    var resultRole = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            // Tạo một Role để quản lý khách hàng
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);
            }

            // Tạo một Role cho khách hàng     
            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }
        }
    }
}
