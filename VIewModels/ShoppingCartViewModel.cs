using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class ShoppingCartViewModel : ViewModel
    {
        public int ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public int TotalAmount { get; set; }
    }

}
