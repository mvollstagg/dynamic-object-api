using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Core.Attributes;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;

namespace IODynamicObject.Application.Types.Customers
{
    public class CustomerModel : IOEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [IOMappingPropertyType(typeof(List<IOOrder>))]
        public List<OrderModel> Orders { get; set; }
        public List<Dictionary<string, List<Dictionary<string, string>>>> DynamicObjects { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public DateTime ModificationDateUtc { get; set; }
    }
}
