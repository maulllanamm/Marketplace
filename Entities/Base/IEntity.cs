using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Base
{
    public interface IEntity
    {
        public int id { get; set; }
        public bool? is_deleted { get; set; }
        public DateTimeOffset? created_date { get; set; }
        public string created_by { get; set; }
        public DateTimeOffset? modified_date { get; set; }
        public string modified_by { get; set; }
    }
}
