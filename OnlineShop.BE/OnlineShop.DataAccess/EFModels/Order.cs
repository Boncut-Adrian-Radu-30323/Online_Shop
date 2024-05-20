using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DataAccess.EFModels
{
    namespace OnlineShop.DataAccess.EFModels
    {
        public class Order
        {
            [Key]
            public int Id { get; set; }

            [ForeignKey("User")]
            public int UserId { get; set; }

            public string PhoneNumber { get; set; }

            public string Address { get; set; }

            public string FullName { get; set; }
            public decimal TotalOrderPrice { get; set; }

            public DateTime OrderDate { get; set; }
            public virtual User User { get; set; }

            public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        }
    }
}