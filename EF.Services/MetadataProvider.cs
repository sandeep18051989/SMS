using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EF.Services
{
    public class MetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            var additionalValues = attributes.OfType<IAttribute>().ToList();
            foreach (var additionalValue in additionalValues)
            {
                if (metadata.AdditionalValues.ContainsKey(additionalValue.Name))
                    throw new Exception("There is already an attribute with the name of \"" + additionalValue.Name + "\" on this model.");
                metadata.AdditionalValues.Add(additionalValue.Name, additionalValue);
            }
            return metadata;
        }
    }
}