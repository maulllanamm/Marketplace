using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class ProductViewModel : ViewModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public long Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
    }

    public class ProductResponse
    {
        public List<ProductViewModel> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }

}
