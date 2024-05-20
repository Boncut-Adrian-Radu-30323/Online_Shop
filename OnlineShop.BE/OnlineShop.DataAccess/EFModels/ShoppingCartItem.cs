using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DataAccess.EFModels
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("ShoppingCart")]
        public int ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
