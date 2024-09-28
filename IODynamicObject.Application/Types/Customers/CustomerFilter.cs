using IODynamicObject.Core.Filtering;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Types.Customers
{
    public class CustomerFilter : BaseFilter
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
