using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nihility.X0.Solution.Models.ViewModels
{
    public class PostTagViewModel
    {
        public int PostID { get; set; }
        public string TagID { get; set; }
        public virtual PostViewModel Post { get; set; }
        public virtual TagViewModel Tag { get; set; }
    }
}