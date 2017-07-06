using System.Xml;
using System.Xml.Serialization;

//using iFactr.Core.Targets;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a list of <see cref="iItem"/>s on an <see cref="iLayer"/>.
    /// </summary>
    public class iList : iCollection, IXmlSerializable
    {
        /// <summary>
        /// The available list style types.
        /// </summary>
        /// <remarks>
        /// This enumeration is scheduled for deprecation in iFactr version 2.0.
        /// </remarks>
        public enum StyleTypes
        {
            /// <summary>
            /// Simple formatting.
            /// </summary>
            Simple,
            /// <summary>
            /// Simple formatting w/word wrap of text.
            /// </summary>
            SimpleWrap,
            /// <summary>
            /// Forces subtext to be displayed below text.
            /// </summary>
            SubtextBelow,
            /// <summary>
            /// Forces subtext to be displayed beside text.
            /// </summary>
            SubtextBeside,
            /// <summary>
            /// Content formatting.
            /// </summary>
            Content,
            /// <summary>
            /// Header content formatting.
            /// </summary>
            HeaderContent,
            /// <summary>
            /// Header content formatting w/word wrap of text.
            /// </summary>
            HeaderWrapContent,
            /// <summary>
            /// Store item formatting.
            /// </summary>
            Store
        }

        /// <summary>
        /// Gets or sets the display style.
        /// </summary>
        /// <value>The display style as a <see cref="StyleTypes"/> instance.</value>
        public StyleTypes DisplayStyle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="iList"/> class.
        /// </summary>
        public iList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iList"/> class.
        /// </summary>
        /// <param name="header">The header text to display.</param>
        public iList(string header)
        {
            Header = header;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new iList Clone()
        {
            return (iList)base.CloneOverride();
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