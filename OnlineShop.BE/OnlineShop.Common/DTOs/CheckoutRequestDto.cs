﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Common.DTOs
{
    public class CheckoutRequestDto
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
    }
}
