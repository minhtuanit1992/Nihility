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
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {

    }
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
