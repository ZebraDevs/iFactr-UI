using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using iFactr.Core.Styles;
using MonoCross.Utilities;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents the base implementation of an iLayer item.  This class is abstract.
    /// </summary>
    public abstract class iLayerItem
    {
        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the footer text.
        /// </summary>
        public string Footer { get; set; }

        internal int SectionIndex { get; set; }

        /// <summary>
        /// Returns a string representation of the specified style's background color.
        /// </summary>
        /// <param name="layerStyle">The <see cref="Style"/> containing the background color to return a string representation of.</param>
        public virtual string GetLayerStyle(Style layerStyle)
        {
            if (layerStyle == null)
            {
                return string.Empty;
            }

            var styleString = string.Format("background-color:{0};", layerStyle.LayerItemBackgroundColor.HexCode);
            if (layerStyle.LayerItemBackgroundColor.A == byte.MaxValue)
            {
                return styleString;
            }

            // we're not fully opaque - calculate the level
            // Color.A provides 256 levels of transparency
            // The CSS3 syntax for transparency is opacity:x
            // x can be from 0.0 - 1.0. A lower value makes the element more transparent.
            var opacityLevel = (float)layerStyle.LayerItemBackgroundColor.A / 255;
            styleString += string.Format("opacity:{0};", string.Format("{0:0.0}", opacityLevel));
            return styleString;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public iLayerItem Clone()
        {
            return (iLayerItem)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected virtual object CloneOverride() { return MemberwiseClone(); }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the <c>IXmlSerializable</c> interface,
        /// you should return <c>null</c> (<c>Nothing</c> in Visual Basic) from this method, and instead, if specifying 
        /// a custom schema is required, apply the <see cref="XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="IXmlSerializable.WriteXml"/> method and consumed by the <see cref="IXmlSerializable.ReadXml"/> method.</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="items"/> collection.</typeparam>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
        /// <param name="items">A collection of <typeparamref name="T"/> to populate.</param>
        protected void ReadXml<T>(XmlReader reader, IList<T> items)
        {
            items.Clear();
            Header = reader.GetAttribute("Header");
            Footer = reader.GetAttribute("Footer");
            if (reader.IsEmptyElement) { return; }
            reader.Read();
            while (reader.Depth > 2 || reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.Depth > 2)
                {
                    reader.Read();
                    continue;
                }
                var typeName = reader.Name;
                var type = Type.GetType(typeName.Replace("___", ", "));
                var x = new XmlSerializer(type, new XmlRootAttribute(typeName));
                var s = new StringReader(reader.ReadOuterXml());
                items.Add((T)x.Deserialize(s));
            }
            if (reader.NodeType == XmlNodeType.EndElement)
            {
                reader.ReadEndElement();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <paramref name="items"/> collection.</typeparam>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
        /// <param name="items">A collection of <typeparamref name="T"/> to write.</param>
        protected void WriteXml<T>(XmlWriter writer, IEnumerable<T> items)
        {
            if (Header != null)
            {
                writer.WriteAttributeString("Header", Header);
            }
            if (Footer != null)
            {
                writer.WriteAttributeString("Footer", Footer);
            }

            foreach (var item in items)
            {
                var s = new MemoryStream();
                try
                {
                    var type = item.GetType();
                    var xmlRoot = type.FullName;
                    var assembly = GetType().AssemblyQualifiedName;
                    assembly = assembly.Substring(assembly.IndexOf(',') + 1);
                    var typeAssembly = type.AssemblyQualifiedName;
                    typeAssembly = typeAssembly.Substring(typeAssembly.IndexOf(',') + 1);
                    if (typeAssembly != assembly)
                    {
                        xmlRoot = type.AssemblyQualifiedName;
                        var versionIndex = xmlRoot.IndexOf(", Version", StringComparison.Ordinal);
                        xmlRoot = xmlRoot.Remove(versionIndex);
                        xmlRoot = xmlRoot.Replace(", ", "___");
                    }
                    var x = new XmlSerializer(type, new XmlRootAttribute(xmlRoot));
                    x.Serialize(s, item);
                }
                catch (Exception e)
                {
                    Device.Log.Error("Could not serialize item:", e);
                    continue;
                }
                s.Position = 0;
                var xml = new StreamReader(s).ReadToEnd();
                xml = xml.Replace("<?xml version=\"1.0\"?>", string.Empty);
                xml = xml.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
                writer.WriteRaw(Environment.NewLine + xml.Trim());
            }
        }
    }
}