using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineShop.DataAccess.EFModels
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore] 
        public virtual User User { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart()
        {
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
        }
    }
}
