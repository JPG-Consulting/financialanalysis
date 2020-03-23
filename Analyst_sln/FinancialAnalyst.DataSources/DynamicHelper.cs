using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FinancialAnalyst.DataSources
{

    /// Extension methods for the dynamic object.///
    /// https://martinnormark.com/serialize-c-dynamic-and-anonymous-types-to-xml/
    public static class DynamicHelper
    {
        /// Defines the simple types that is directly writeable to XML.///
        private static readonly Type[] _writeTypes = new[] { typeof(string), typeof(DateTime), typeof(Enum), typeof(decimal), typeof(Guid) };

        /// Determines whether [is simple type] [the specified type].///
        /// The type to check.
        /// true if [is simple type] [the specified type]; otherwise, false.
        public static bool IsSimpleType(this Type type)
        {
            if (type == null)
                return false;

            return type.IsPrimitive || _writeTypes.Contains(type);
        }
        ///
        /// Converts an anonymous type to an XElement.///
        /// The input.
        /// Returns the object as it's XML representation in an XElement.
        public static XElement ToXml(this object input)
        {
            return input.ToXml(null);
        }
        ///
        /// Converts an anonymous type to an XElement.///
        /// The input.
        /// The element name.
        /// Returns the object as it's XML representation in an XElement.
        public static XElement ToXml(this object input, string element)
        {
            if (input == null)
            {
                return null;
            }

            if (String.IsNullOrEmpty(element))
            {
                element = "object";
            }

            element = XmlConvert.EncodeName(element); var ret = new XElement(element);

            if (input != null)
            {
                var type = input.GetType(); var props = type.GetProperties();
                var elements = from prop
                               in props
                               let name = XmlConvert.EncodeName(prop.Name)
                               let val = prop.PropertyType.IsArray ? "array" : prop.GetValue(input, null)
                               let value = prop.PropertyType.IsArray ? GetArrayElement(prop, (Array)prop.GetValue(input, null)) : (prop.PropertyType.IsSimpleType() ? new XElement(name, val) : val.ToXml(name))
                               where value != null
                               select value;
                ret.Add(elements);
            }
            return ret;
        }


        /// Gets the array element.
        /// The property info.
        /// The input object.
        /// Returns an XElement with the array collection as child elements.
        private static XElement GetArrayElement(PropertyInfo info, Array input)
        {
            var name = XmlConvert.EncodeName(info.Name); 
            XElement rootElement = new XElement(name); 
            var arrayCount = input.GetLength(0);
            for (int i = 0; i < arrayCount; i++)
            {
                var val = input.GetValue(i);
                XElement childElement = val.GetType().IsSimpleType() ? new XElement(name + "Child", val) : val.ToXml();
                rootElement.Add(childElement);
            }
            return rootElement;
        }
    }
}
