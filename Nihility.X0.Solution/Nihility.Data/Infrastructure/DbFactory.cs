using Nihility.Data.Interface;

namespace Nihility.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        NihiDbContext dbContext;
        public NihiDbContext Init()
        {
            // Nếu dbContext là null thì tạo mới một đối tương
            return dbContext ?? (dbContext = new NihiDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
