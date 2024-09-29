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
        public static TModel ApplyMapping<T, TModel>(T entity, params string[] ignoreProperties)
        where T : IOEntityBase, new()
        where TModel : new()
        {
            var model = new TModel();

            MapStaticProperties(entity, model, ignoreProperties);
            MapCollections(entity, model, ignoreProperties);
            MapNestedObjects(entity, model, ignoreProperties);
            MapDynamicObjects(entity, model);

            return model;
        }

        private static void MapStaticProperties<T, TModel>(T entity, TModel model, string[] ignoreProperties)
        {
            var entityType = typeof(T);
            var modelType = typeof(TModel);

            var entityProperties = entityType.GetProperties();
            var modelProperties = modelType.GetProperties();

            foreach (var modelProp in modelProperties)
            {
                if (ignoreProperties.Contains(modelProp.Name))
                    continue;

                var entityProp = entityProperties.FirstOrDefault(
                    p => p.Name == modelProp.Name && p.PropertyType == modelProp.PropertyType);

                if (entityProp != null)
                {
                    var value = entityProp.GetValue(entity);
                    modelProp.SetValue(model, value);
                }
            }
        }

        private static void MapCollections<T, TModel>(T entity, TModel model, string[] ignoreProperties)
        {
            var entityType = typeof(T);
            var modelType = typeof(TModel);

            var entityProperties = entityType.GetProperties();
            var modelProperties = modelType.GetProperties();

            foreach (var modelProp in modelProperties)
            {
                if (ignoreProperties.Contains(modelProp.Name))
                    continue;

                if (modelProp.PropertyType.IsGenericType && modelProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var entityProp = entityProperties.FirstOrDefault(p => p.Name == modelProp.Name);

                    if (entityProp != null && entityProp.PropertyType.IsGenericType && entityProp.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var entityItemType = entityProp.PropertyType.GetGenericArguments()[0];
                        var modelItemType = modelProp.PropertyType.GetGenericArguments()[0];

                        var mappingAttribute = modelProp.GetCustomAttribute<IOMappingPropertyTypeAttribute>();
                        if (mappingAttribute != null && mappingAttribute.EntityType.IsGenericType &&
                            mappingAttribute.EntityType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            entityItemType = mappingAttribute.EntityType.GetGenericArguments()[0];
                        }

                        var entityList = entityProp.GetValue(entity) as IEnumerable;
                        if (entityList != null)
                        {
                            var modelList = Activator.CreateInstance(typeof(List<>).MakeGenericType(modelItemType)) as IList;

                            foreach (var entityItem in entityList)
                            {
                                var mappedItem = ApplyMapping(entityItem, entityItemType, modelItemType, "Customer");
                                modelList.Add(mappedItem);
                            }

                            modelProp.SetValue(model, modelList);
                        }
                    }
                }
            }
        }

        private static void MapNestedObjects<T, TModel>(T entity, TModel model, string[] ignoreProperties)
        {
            var entityType = typeof(T);
            var modelType = typeof(TModel);

            var entityProperties = entityType.GetProperties();
            var modelProperties = modelType.GetProperties();

            foreach (var modelProp in modelProperties)
            {
                if (ignoreProperties.Contains(modelProp.Name))
                    continue;

                if (!modelProp.PropertyType.IsPrimitive && !modelProp.PropertyType.IsValueType &&
                    modelProp.PropertyType != typeof(string) && !modelProp.PropertyType.IsGenericType)
                {
                    var entityProp = entityProperties.FirstOrDefault(p => p.Name == modelProp.Name);
                    var mappingAttribute = modelProp.GetCustomAttribute<IOMappingPropertyTypeAttribute>();

                    if (entityProp != null)
                    {
                        Type entityPropertyType = mappingAttribute != null ? mappingAttribute.EntityType : entityProp.PropertyType;

                        if (entityProp.PropertyType == entityPropertyType)
                        {
                            var entityNestedObject = entityProp.GetValue(entity);
                            if (entityNestedObject != null)
                            {
                                var mappedNestedObject = ApplyMapping(entityNestedObject, entityPropertyType, modelProp.PropertyType, "Customer");
                                modelProp.SetValue(model, mappedNestedObject);
                            }
                        }
                    }
                }
            }
        }

        private static object ApplyMapping(object entity, Type entityType, Type modelType, params string[] ignoreProperties)
        {
            var mappingMethod = typeof(IOMappingHelper).GetMethod("ApplyMapping");
            var genericMethod = mappingMethod.MakeGenericMethod(entityType, modelType);
            return genericMethod.Invoke(null, new object[] { entity, ignoreProperties });
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
