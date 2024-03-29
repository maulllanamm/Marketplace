﻿namespace Marketplace.Responses.Base
{
    public class GuidViewModel
    {
        public Guid Id { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
