using Nihility.Model.Astract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Model.Models
{
    [Table("ProductCategories")]
    public class ProductCategory : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Alias { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public Nullable<int> ParentID { get; set; }

        public Nullable<int> DisplayOrder { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        public Nullable<bool> HomeFlag { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
