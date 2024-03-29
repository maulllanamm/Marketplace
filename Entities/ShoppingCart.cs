﻿using Marketplace.Enitities.Base;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Enitities
{
    public class ShoppingCart : Entity
    {
        public int product_id { get; set; }
        public Guid user_id { get; set; }
        public int quantity { get; set; }
        public long total_price { get; set; }
    }
}
