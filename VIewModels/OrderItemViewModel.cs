﻿using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class OrderItemViewModel : ViewModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
    }

}
