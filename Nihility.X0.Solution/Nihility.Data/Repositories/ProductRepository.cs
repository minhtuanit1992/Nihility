using Nihility.Data.Infrastructure;
using Nihility.Data.Interface;
using Nihility.Model.Models;

namespace Nihility.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {

    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
