using System.Collections.Generic;
using IODynamicObject.Application.Types.IODynamicObjects;

namespace IODynamicObject.Application.Validators.Implementations
{
    public class IODynamicObjectValidator : IIODynamicObjectValidator
    {
        public List<string> Validate(IODynamicObjectRequest request, bool isSubObject = false)
        {
            var errors = new List<string>();

            if (!isSubObject)
            {
                // Check if Operation is required for main request
                if (string.IsNullOrWhiteSpace(request.Operation))
                {
                    errors.Add("Operation is required.");
                }
            }

            // Validate ObjectType
            if (string.IsNullOrWhiteSpace(request.ObjectType))
            {
                errors.Add("ObjectType is required.");
            }

            // Define required fields for each object type
            var requiredFields = new Dictionary<string, List<string>>
            {
                { "Product", new List<string> { "Name", "Price" } },
                { "Order", new List<string> { "CustomerId" } },
                { "OrderItem", new List<string> { "ProductId", "Quantity" } },
                { "Customer", new List<string> { "FirstName", "LastName", "Email" } }
            };

            if (!string.IsNullOrWhiteSpace(request.ObjectType))
            {
                if (!requiredFields.ContainsKey(request.ObjectType))
                {
                    errors.Add($"Unsupported object type: {request.ObjectType}");
                }
                else
                {
                    // Validate required fields
                    var fields = requiredFields[request.ObjectType];
                    if (request.Data == null)
                    {
                        errors.Add("Data is required.");
                    }
                    else
                    {
                        foreach (var field in fields)
                        {
                            if (!request.Data.ContainsKey(field))
                            {
                                errors.Add($"Field '{field}' is required for object type '{request.ObjectType}'.");
                            }
                        }
                    }
                }
            }

            // Recursively validate sub-objects
            if (request.SubObjects != null)
            {
                foreach (var subRequest in request.SubObjects)
                {
                    var subErrors = Validate(subRequest, isSubObject: true);
                    errors.AddRange(subErrors);
                }
            }

            return errors;
        }
    }
}
