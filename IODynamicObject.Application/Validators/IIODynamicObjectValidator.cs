using System.Collections.Generic;
using IODynamicObject.Application.DTOs.Requests;

namespace IODynamicObject.Application.Validators
{
    public interface IIODynamicObjectValidator
    {
        List<string> Validate(IODynamicObjectRequest request);
    }
}
