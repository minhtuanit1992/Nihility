using Microsoft.AspNet.Identity.EntityFramework;
using Nihility.Model.Models;
using Nihility.Model.Models.Identity;
using System.Data.Entity;

namespace Nihility.Data
{
    /// <summary>
    /// - Lớp Context kế thừa từ IdentityDbContext thay vì DbContext để có thể sử dụng được Identity
    /// </summary>
    public class NihiDbContext : IdentityDbContext<ApplicationUser>
    {
        public NihiDbContext() : base("NihiConnection")
        {
            this.Configuration.LazyLoadingEnabled = false; //Khi load một Entity sẽ không load kèm các Entity của các khóa ngoại.
        }

        public DbSet<Footer> Footers { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuGroup> MenuGroup { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<SupportOnline> SupportOnlines { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<VisitorStatistic> VisitorStatistics { get; set; }
        public DbSet<Error> Errors { get; set; }

        /// <summary>
        /// Contructor tạo đối tượng DBContext sử dụng để cấu hình Authorization
        /// </summary>
        /// <returns></returns>
        public static NihiDbContext Create()
        {
            return new NihiDbContext();
        }

        /// <summary>
        /// - Đổi tên các bảng mặc định của Identity Authentication thành tên bảng của riêng mình.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
        }
    }
}
