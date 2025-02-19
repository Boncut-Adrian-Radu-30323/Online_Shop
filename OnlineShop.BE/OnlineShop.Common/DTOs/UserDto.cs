﻿namespace OnlineShop.Common.DTOs
{
    public class UserDto : BaseUserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedDate { get; set; }

    }

}
