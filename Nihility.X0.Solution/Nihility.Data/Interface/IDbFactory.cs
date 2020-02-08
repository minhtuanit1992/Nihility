using System;

namespace Nihility.Data.Interface
{
    public interface IDbFactory : IDisposable
    {
        NihiDbContext Init();
    }
}
