using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODynamicObject.Application.Mappers
{
    public static class IOMappingHelper
    {
        public static TModel ApplyMapping<T, TModel>(T entity)
            where T : IOEntityBase, new()
            where TModel : new()
        {
            var model = new TModel();

            // Map static properties where names and types match
            MapStaticProperties(entity, model);

            // Map dynamic objects (IOObject)
            MapDynamicObjects(entity, model);

            return model;
        }

        private static void MapStaticProperties<T, TModel>(T entity, TModel model)
        {
            var entityType = typeof(T);
            var modelType = typeof(TModel);

            // Get all properties from both entity and model
            var entityProperties = entityType.GetProperties();
            var modelProperties = modelType.GetProperties();

            foreach (var modelProp in modelProperties)
            {
                // Find matching entity property by name and type
                var entityProp = entityProperties.FirstOrDefault(
                    p => p.Name == modelProp.Name && p.PropertyType == modelProp.PropertyType);

                if (entityProp != null)
                {
                    // Copy value from entity to model
                    var value = entityProp.GetValue(entity);
                    modelProp.SetValue(model, value);
                }
            }
        }

        private static void MapDynamicObjects<T, TModel>(T entity, TModel model)
        {
            var entityType = typeof(T);
            var modelType = typeof(TModel);

            // Get the dynamic objects property from the entity
            var dynamicObjectsProperty = entityType.GetProperty("DynamicObjects");
            var dynamicObjectsValue = dynamicObjectsProperty?.GetValue(entity) as List<IOObject>;

            if (dynamicObjectsValue == null) return;

            // Prepare a list of dynamic objects for the model
            var dynamicObjectsList = new List<Dictionary<string, List<Dictionary<string, string>>>>();

            foreach (var dynamicObject in dynamicObjectsValue)
            {
                var dynamicObjectEntry = new Dictionary<string, List<Dictionary<string, string>>>();

                // Group fields and values for this dynamic object
                var fieldList = new List<Dictionary<string, string>>();

                // Loop through each field in the dynamic object
                for (int i = 0; i < dynamicObject.Fields.First().Values.Count; i++)
                {
                    var fieldEntry = new Dictionary<string, string>();

                    foreach (var field in dynamicObject.Fields)
                    {
                        fieldEntry[field.Name] = field.Values[i].Value;
                    }

                    fieldList.Add(fieldEntry);
                }

                // Add the dynamic object name as the key and the list of field entries as the value
                dynamicObjectEntry[dynamicObject.Name] = fieldList;
                dynamicObjectsList.Add(dynamicObjectEntry);
            }

            // Set the dynamic objects in the model
            var dynamicObjectsModelProperty = modelType.GetProperty("DynamicObjects");
            dynamicObjectsModelProperty?.SetValue(model, dynamicObjectsList);
        }
    }
}
