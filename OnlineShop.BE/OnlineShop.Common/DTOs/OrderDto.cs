using System;
using System.Collections.Generic;

namespace OnlineShop.Common.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
