using Nihility.Data.Infrastructure;
using Nihility.Data.Interface;
using Nihility.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Data.Repositories
{
    public interface IProductTagRepository : IRepository<ProductTag>
    {

    }

    public class ProductTagRepository : RepositoryBase<ProductTag>
    {
        public ProductTagRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
