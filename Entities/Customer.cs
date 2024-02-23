using Marketplace.Enitities.Base;

namespace Marketplace.Enitities
{
    public class Customer : GuidEntity
    {
        public string username { get; set; }
        public string password_salt { get; set; }
        public string password_hash { get; set; }
        public string email { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
    }
}
