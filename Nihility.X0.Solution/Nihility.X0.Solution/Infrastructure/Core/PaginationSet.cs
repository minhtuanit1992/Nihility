using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nihility.X0.Solution.Infrastructure.Core
{
    /// <summary>
    /// Author: Hứa Minh Tuấn
    /// - Lớp đảm nhận vai trò phân trang cho các đối tượng được đọc từ DbContext
    /// </summary>
    public class PaginationSet<T>
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
        public int Count { get => (Items != null) ? Items.Count() : 0; }
    }
}