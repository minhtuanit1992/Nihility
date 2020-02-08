using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Data.Interface
{
    public interface IRepository<T> where T : class
    {
        //Đánh dấu một Entity mới
        T Add(T entity);

        //Đánh dấu chỉnh sửa một Entity
        void Update(T entity);

        //Đánh dấu xóa một Entity
        T Delete(T entity);

        //Đánh dấu xóa một Entity theo ID
        T Delete(int id);

        /// <summary>
        /// Xóa nhiều Entity cùng một lúc
        /// </summary>
        /// <param name="where">Câu truy vấn</param>
        void DeleteMulti(Expression<Func<T, bool>> where);

        //Lấy một Entity bằng ID (int)
        T GetSingleById(int id);

        /// <summary>
        /// Lấy một Entity theo truy vấn Linq
        /// </summary>
        /// <param name="expression">Câu truy vấn</param>
        /// <param name="includes">Các bảng con</param>
        /// <returns></returns>
        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IEnumerable<T> GetAll(string[] includes = null);

        IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);
    }
}
