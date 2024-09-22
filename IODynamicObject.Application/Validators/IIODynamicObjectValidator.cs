using System.Collections.Generic;
using IODynamicObject.Application.Types.IODynamicObjects;

namespace IODynamicObject.Application.Validators
{
    public interface IIODynamicObjectValidator
    {
        List<string> Validate(IODynamicObjectRequest request, bool isSubObject = false);
    }
}
