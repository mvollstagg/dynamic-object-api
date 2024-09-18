using System.Collections.Generic;
using IODynamicObject.Application.DTOs.Requests;

namespace IODynamicObject.Application.Validators.Implementations
{
    public class IODynamicObjectValidator : IIODynamicObjectValidator
    {
        public List<string> Validate(IODynamicObjectRequest request)
        {
            var errors = new List<string>();

            // Define required fields for each object type
            var requiredFields = new Dictionary<string, List<string>>
            {
                { "Product", new List<string> { "Name", "Price" } },
                { "Order", new List<string> { "CustomerId", "OrderItems" } },
                { "Customer", new List<string> { "FirstName", "LastName", "Email" } }
            };

            if (!requiredFields.ContainsKey(request.ObjectType))
            {
                errors.Add($"Unsupported object type: {request.ObjectType}");
                return errors;
            }

            var fields = requiredFields[request.ObjectType];
            foreach (var field in fields)
            {
                if (!request.Data.ContainsKey(field))
                {
                    errors.Add($"Field '{field}' is required for object type '{request.ObjectType}'.");
                }
            }

            return errors;
        }
    }
}
