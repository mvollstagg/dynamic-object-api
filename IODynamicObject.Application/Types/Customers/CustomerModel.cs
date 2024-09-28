using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Types.Customers
{
    public class CustomerModel : IOEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
