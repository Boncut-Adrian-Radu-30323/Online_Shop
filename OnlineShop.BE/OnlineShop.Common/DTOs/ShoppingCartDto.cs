namespace OnlineShop.Common.DTOs
{
    public class ShoppingCartDto
    {
        public int UserId { get; set; }
        public List<ShoppingCartItemDto> ShoppingCartItems { get; set; }
    }
}
