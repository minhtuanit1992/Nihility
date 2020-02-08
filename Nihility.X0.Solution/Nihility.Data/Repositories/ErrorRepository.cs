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
    public interface IErrorRepository: IRepository<Error>
    {

    }

    public class ErrorRepository : RepositoryBase<Error>, IErrorRepository
    {
        public ErrorRepository(IDbFactory dbFactory) : base (dbFactory)
        {

        }
    }
}
