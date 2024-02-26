using Marketplace.Enitities.Base;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Enitities
{
    public class Product : Entity
    {
        public string name { get; set; }
        public string category { get; set; }
        public long price { get; set; }
        public string description { get; set; }
        public int stock_quantity { get; set; }
    }
}
