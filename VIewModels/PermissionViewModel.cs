using Marketplace.Responses.Base;

namespace Marketplace.Responses
{
    public class PermissionViewModel : ViewModel
    {
        public string Name { get; set; }
        public string HttpMethod { get; set; }
        public string Path { get; set; }
    }

}
