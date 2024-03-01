using Marketplace.Enitities.Base;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Enitities
{
    public class Order : Entity
    {
        public Guid user_id { get; set; }
        public DateTimeOffset? order_date { get; set; }
        public string shipping_address { get; set; }
        public int total_price { get; set; }
    }
}
