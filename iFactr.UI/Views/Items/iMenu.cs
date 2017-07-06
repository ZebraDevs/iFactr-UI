using System;
using System.Xml;
using System.Xml.Serialization;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a menu of <see cref="iItem"/>s on an <see cref="iLayer"/>.
    /// </summary>
    public class iMenu : iCollection, IXmlSerializable
    {
        /// <summary>
        /// Gets or sets the menu ID.
        /// </summary>
        /// <value>The ID as a <see cref="String"/> value.</value>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the menu style.
        /// </summary>
        /// <value>The style as a <see cref="String"/> value.</value>
        public string Style { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iMenu"/> class.
        /// </summary>
        public iMenu()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iMenu"/> class.
        /// </summary>
        /// <param name="header">The header text to display.</param>
        public iMenu(string header)
        {
            Header = header;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new iMenu Clone()
        {
            return (iMenu)base.CloneOverride();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            ReadXml(reader, Items);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            WriteXml(writer, Items);
        }
    }
}