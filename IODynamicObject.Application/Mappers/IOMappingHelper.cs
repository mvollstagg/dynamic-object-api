using IODynamicObject.Core.Attributes;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            // Map collections (like List<OrderItem>)
            MapCollections(entity, model);

            // Map nested objects (like Product in OrderItem)
            MapNestedObjects(entity, model);

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

        private static void MapCollections<T, TModel>(T entity, TModel model)
        {
            var entityType = typeof(T);
            var modelType = typeof(TModel);

            // Get all properties from both entity and model
            var entityProperties = entityType.GetProperties();
            var modelProperties = modelType.GetProperties();

            foreach (var modelProp in modelProperties)
            {
                // Check if the model property is a generic list
                if (modelProp.PropertyType.IsGenericType && modelProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // Find matching entity property by name
                    var entityProp = entityProperties.FirstOrDefault(p => p.Name == modelProp.Name);

                    if (entityProp != null && entityProp.PropertyType.IsGenericType && entityProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        // Get the generic type arguments of the list (e.g., OrderItem -> OrderItemModel)
                        var entityItemType = entityProp.PropertyType.GetGenericArguments()[0];
                        var modelItemType = modelProp.PropertyType.GetGenericArguments()[0];

                        // Get the value of the list from the entity
                        var entityList = entityProp.GetValue(entity) as IEnumerable<object>;
                        if (entityList != null)
                        {
                            // Create a new list for the model
                            var modelList = Activator.CreateInstance(typeof(List<>).MakeGenericType(modelItemType)) as IList;

                            // Loop through each item in the entity list and apply mapping recursively
                            foreach (var entityItem in entityList)
                            {
                                // Use ApplyMapping to map each entity item to model item
                                var modelItem = Activator.CreateInstance(modelItemType);
                                var mappedItem = typeof(IOMappingHelper)
                                    .GetMethod("ApplyMapping")
                                    .MakeGenericMethod(entityItemType, modelItemType)
                                    .Invoke(null, new object[] { entityItem });

                                modelList.Add(mappedItem);
                            }

                            // Set the model list in the model
                            modelProp.SetValue(model, modelList);
                        }
                    }
                }
            }
        }

        private static void MapNestedObjects<T, TModel>(T entity, TModel model)
        {
            var entityType = typeof(T);
            var modelType = typeof(TModel);

            // Get all properties from both entity and model
            var entityProperties = entityType.GetProperties();
            var modelProperties = modelType.GetProperties();

            foreach (var modelProp in modelProperties)
            {
                // Check if the property is a complex nested object (e.g., Product in OrderItemModel)
                if (!modelProp.PropertyType.IsPrimitive && !modelProp.PropertyType.IsValueType && modelProp.PropertyType != typeof(string))
                {
                    // Find matching entity property by name
                    var entityProp = entityProperties.FirstOrDefault(p => p.Name == modelProp.Name);
                    var entityPropertyType = modelProp.PropertyType;
                    var mappingAttribute = modelProp.GetCustomAttribute<IOMappingPropertyTypeAttribute>();

                    if (entityProp != null && mappingAttribute != null)
                    {
                        entityPropertyType = mappingAttribute != null ? mappingAttribute.EntityType : entityProp.PropertyType;
                    }

                    if (entityProp != null && (entityProp.PropertyType == modelProp.PropertyType || entityProp.PropertyType == entityPropertyType))
                    {
                        // Get the value of the nested object from the entity
                        var entityNestedObject = entityProp.GetValue(entity);
                        if (entityNestedObject != null)
                        {
                            // Recursively apply mapping to the nested object
                            var nestedModel = Activator.CreateInstance(modelProp.PropertyType);
                            var mappedNestedObject = typeof(IOMappingHelper)
                                .GetMethod("ApplyMapping")
                                .MakeGenericMethod(entityProp.PropertyType, modelProp.PropertyType)
                                .Invoke(null, new object[] { entityNestedObject });

                            // Set the mapped nested object in the model
                            modelProp.SetValue(model, mappedNestedObject);
                        }
                    }
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
