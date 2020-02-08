using Nihility.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Model.Astract
{
    public class Auditable : IAuditable
    {
        public Nullable<DateTime> CreatedDate { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        public Nullable<DateTime> ModifiedDate { get; set; }

        [MaxLength(256)]
        public string ModifiedBy { get; set; }

        [MaxLength(256)]
        public string MetaKeywork { get; set; }

        [MaxLength(256)]
        public string MetaDescription { get; set; }

        public bool Status { get; set; }
    }
}
