using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Model.Models
{
    [Table("SystemConfigs")]
    public class SystemConfig
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Code { get; set; }

        [MaxLength(50)]
        public string ValueString { get; set; }

        public Nullable<int> ValueInt { get; set; }
    }
}
