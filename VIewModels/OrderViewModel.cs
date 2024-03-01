using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class OrderViewModel : ViewModel
    {
        public Guid UserId { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public int TotalPrice { get; set; }
    }

}
