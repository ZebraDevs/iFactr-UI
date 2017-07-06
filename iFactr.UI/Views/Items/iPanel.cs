using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using iFactr.Core.Controls;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a container for rich text or HTML information on an <see cref="iLayer"/>.
    /// </summary>
    public class iPanel : iLayerItem, IHtmlText, IXmlSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iPanel"/> class.
        /// </summary>
        public iPanel()
        {
            Items = new List<PanelItem>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iPanel"/> class.
        /// </summary>
        /// <param name="header">The header text to display.</param>
        public iPanel(string header)
        {
            Header = header;
            Items = new List<PanelItem>();
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>The header text as a <see cref="String"/> value.</value>
        [Obsolete("Use Header instead.")]
        public string Title
        {
            get { return Header; }
            set { Header = value; }
        }
        /// <summary>
        /// Gets or sets the rich text or HTML to be rendered.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        public string Text
        {
            get
            {
                StringBuilder html = new StringBuilder(_text);
                foreach (var item in Items)
                    html.Append(item.GetHtml());
                return html.ToString();
            }
            set
            {
                _text = value;
                Items.Clear();
            }
        }
        private string _text;
        /// <summary>
        /// Gets or sets the style of this instance.
        /// </summary>
        /// <value>The style as a <see cref="String"/> value.</value>
        public string Style { get; set; }
        /// <summary>
        /// Gets or sets an image to place within this instance.
        /// </summary>
        /// <value>The image as a <see cref="Icon"/>.</value>
        [Obsolete("This property has been deprecated. Please use InsertImage to display an image.")]
        public Icon Image { get; set; }

        /// <summary>
        /// Gets or sets whether this instance should expand to the far edge of the pane its residing in,
        /// regardless of the size of its content.
        /// </summary>
        /// <value><c>true</c> if this instance is to be displayed full-size; otherwise, <c>false</c>.</value>
        public bool FullSize { get; set; }
        /// <summary>
        /// Gets or sets a collection of <see cref="IBlockPanelItem"/>s to populate this instance with.
        /// </summary>
        public List<PanelItem> Items { get; set; }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public new iPanel Clone()
        {
            return (iPanel)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected override object CloneOverride()
        {
            var block = (iPanel)base.CloneOverride();
            block.Items = new List<PanelItem>(Items.Select(i => i.Clone()));
            return block;
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