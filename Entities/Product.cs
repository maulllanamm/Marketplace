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

    public class ProductResponseEntitiy
    {
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
