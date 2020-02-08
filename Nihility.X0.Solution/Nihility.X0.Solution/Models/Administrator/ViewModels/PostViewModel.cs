using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nihility.X0.Solution.Models.ViewModels
{
    public class PostViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public int CategoryID { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public Nullable<bool> HomeFlag { get; set; }

        public Nullable<bool> HotFlag { get; set; }

        public Nullable<int> ViewCount { get; set; }

        public virtual PostCategoryViewModel PostCategory { get; set; }

        public virtual IEnumerable<PostTagViewModel> PostTags { get; set; }
    }
}