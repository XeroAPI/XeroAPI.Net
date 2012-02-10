using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Linq;
using System.Xml.Serialization;

namespace XeroApi.Model
{
    /// <summary>
    /// Navigates across a model tree and serialised to an <c ref="XmlWriter"/>.
    /// </summary>
    /// <remarks>
    /// This class doesn't serialise properties marked as [Obsolete] or [ReadOnly].
    /// </remarks>
    public class ModelTreeNavigator : IDisposable
    {
        private static readonly ICollection<Type> KnownPropertyTypes = new Collection<Type>
        {
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(Guid),
            typeof(Enum),
            typeof(String),
            typeof(DateTime),
            typeof(Decimal)
        };

        private XmlWriter _writer;


        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTreeNavigator"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public ModelTreeNavigator(XmlWriter writer)
        {
            _writer = writer;
        }


        /// <summary>
        /// Navigates across the specified model and outputs to the current XmlWriter.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model.</param>
        public void Navigate<TModel>(TModel model)
            where TModel : ModelBase
        {
            WriteClass(model);
        }


        /// <summary>
        /// Writes the class.
        /// </summary>
        /// <param name="model">The class to be written.</param>
        /// <param name="elementName">Name of the element.</param>
        public void WriteClass(object model, string elementName = "")
        {
            if (model == null)
            {
                return;
            }

            Type modelType = GetNonNullableType(model.GetType());

            if (string.IsNullOrEmpty(elementName))
            {
                elementName = modelType.Name;
            }
            
            Debug.WriteLine(string.Format("Class name:{0} type:{1}", "?", modelType));

            _writer.WriteStartElement(elementName);

            IEnumerable<PropertyInfo> propertiesToSerialise = modelType.GetProperties()
                .Where(prop => FindAttributeOnProperty<XmlIgnoreAttribute>(prop) == null)
                .Where(prop => FindAttributeOnProperty<ReadOnlyAttribute>(prop) == null)
                .ToArray();


            // Write attributes on the parent element BEFORE writing child elements (simple types only)
            foreach (PropertyInfo property in propertiesToSerialise.Where(prop => FindAttributeOnProperty<XmlAttributeAttribute>(prop) != null))
            {
                WritePropertyAsAttribute(model, property);
            }


            // Write child elements (simple and complex types)
            foreach (PropertyInfo property in propertiesToSerialise.Where(prop => FindAttributeOnProperty<XmlAttributeAttribute>(prop) == null))
            {
                WritePropertyAsElement(model, property);
            }
            
            _writer.WriteEndElement();
        }


        public void WriteList(IModelList modelList, PropertyInfo property)
        {
            if (modelList == null)
            {
                return;
            }
            
            _writer.WriteStartElement(property.Name);

            XmlArrayItemAttribute xmlArrayItem = FindAttributeOnProperty<XmlArrayItemAttribute>(property);
            string overrideElementName = (xmlArrayItem == null) ? string.Empty : xmlArrayItem.ElementName;

            foreach (var modelListItem in modelList)
            {
                WriteClass(modelListItem, overrideElementName);
            }
            
            _writer.WriteEndElement();
        }


        public void WritePropertyAsAttribute(object model, PropertyInfo property)
        {
            XmlAttributeAttribute xmlAttribute = FindAttributeOnProperty<XmlAttributeAttribute>(property);

            Type propType = GetNonNullableType(property.PropertyType);
            string propName = xmlAttribute.AttributeName ?? property.Name;
            object propValue = property.GetValue(model, new object[0]);
            
            if (propType.IsEnum)
            {
                _writer.WriteAttributeString(propName, string.Empty, ConvertValueToString(propValue));
                return;
            }

            if (KnownPropertyTypes.Any(propType.IsAssignableFrom))
            {
                _writer.WriteAttributeString(propName, string.Empty, ConvertValueToString(propValue));
                return;
            }
            
            throw new SerializationException(string.Format("The property type {0} cannot be serialised as an attribute", propType.Name));
        }


        /// <summary>
        /// Writes the property.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="property">The property.</param>
        public void WritePropertyAsElement<TInput>(TInput model, PropertyInfo property)
        {
            var propType = GetNonNullableType(property.PropertyType);
            object propValue = property.GetValue(model, null);

            Debug.WriteLine(string.Format("Prop name:{0} type:{1}", property.Name, propType));

            if (propValue == null)
            {
                return;
            }

            // Write enum value as simple element
            if (typeof(Enum).IsAssignableFrom(propType))
            {
                _writer.WriteElementString(property.Name, string.Empty, ConvertValueToString(propValue)); 
            }

            // Write value as simple xml element...
            else if (KnownPropertyTypes.Any(propType.IsAssignableFrom))
            {
                _writer.WriteElementString(property.Name, string.Empty, ConvertValueToString(propValue));    
            }

            // Write List as complex type
            else if (typeof(IModelList).IsAssignableFrom(propType))
            {
                WriteList((IModelList)propValue, property);
            }

            // Write nested classes
            else 
            {
                WriteClass(propValue);
            }
        }


        private static string ConvertValueToString(object input)
        {
            if (input == null)
            {
                return null;
            }

            if (input is DateTime)
            {
                return ((DateTime) input).ToString("s");
            }
            
            return input.ToString();
        }


        public void Dispose()
        {
            _writer = null;
        }


        #region Helper Methods (TODO: move to ModelTypeHelper)
        

        public static Type GetNonNullableType(Type type)
        {
            return typeof(Nullable<>).IsAssignableFrom(type) ? type.GetGenericArguments()[0] : type;
        }

        /// <summary>
        /// Finds the attribute on property.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static  TAttribute FindAttributeOnProperty<TAttribute>(PropertyInfo property)
            where TAttribute : Attribute
        {
            if (property == null)
            {
                return null;
            }

            return property.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }

        /// <summary>
        /// Finds the attribute on class.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TInput">The type of the model.</typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static TAttribute FindAttributeOnClass<TAttribute, TInput>(TInput input)
            where TAttribute : Attribute
            where TInput : ModelBase
        {
            if (input == null)
            {
                return null;
            }
            
            return input.GetType().GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }

        #endregion
    }
}
