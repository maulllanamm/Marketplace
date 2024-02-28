using Marketplace.Enitities.Base;
using System.ComponentModel.DataAnnotations;

namespace Marketplace.Enitities
{
    public class Permission : Entity
    {
        public string name { get; set; }
        public string http_method { get; set; }
        public string path { get; set; }
    }
}
