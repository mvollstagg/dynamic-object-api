using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOCustomer : IOEntityDeletable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // Navigation property for Orders
        public ICollection<IOOrder> Orders { get; set; }

        // Navigation property for dynamic objects (custom fields for a customer)
        public ICollection<IOObject> DynamicObjects { get; set; }
    }
}
