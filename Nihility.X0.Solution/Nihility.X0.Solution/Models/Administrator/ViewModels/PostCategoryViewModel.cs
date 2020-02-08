using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nihility.X0.Solution.Models.ViewModels
{
    public class PostCategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string Image { get; set; }
        public Nullable<bool> HomeFlag { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string MetaKeywork { get; set; }
        public string MetaDescription { get; set; }
        public bool Status { get; set; }
    }
}