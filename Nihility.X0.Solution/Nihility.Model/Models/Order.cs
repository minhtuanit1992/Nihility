using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nihility.Model.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerMobile { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { get; set; }

        [Required]
        [MaxLength(256)]
        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
