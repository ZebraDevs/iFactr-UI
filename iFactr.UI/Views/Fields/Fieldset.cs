using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using iFactr.Core.Layers;
using iFactr.UI;

/*
This namespace contains all of the classes that represent the abstract
implementation of a form layers as well the objects that can be directly placed on a
form.

The abstract idea of a form is generally defined as what would be necessary as
a screen on a base platform, although richer targets and platforms may support
situations where multiple forms are displayed to the user simultaneously in
order to provide the optimal user experience.
*/
namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a collection of <see cref="Field"/> objects within a layer.
    /// </summary>
    public class Fieldset : iCollection<Field>, IXmlSerializable
    {
        /// <summary>
        /// The available fieldset layouts.
        /// </summary>
        public enum FieldsetLayout
        {
            /// <summary>
            /// The fieldset is to be rendered as a list, or equivalent.
            /// </summary>
            List,
            /// <summary>
            /// Simple in-line rendering based on relative position in the layer code.
            /// </summary>
            Simple,
        }

        /// <summary>
        /// Gets or sets how this instance is laid out.
        /// </summary>
        /// <value>The fieldset layout as a <see cref="FieldsetLayout"/> value.</value>
        public FieldsetLayout Layout { get; set; }

        /// <summary>
        /// Gets or sets the fieldset legend.
        /// </summary>
        /// <value>The fieldset legend as a <see cref="String"/> value.</value>
        [Obsolete("Use Header instead.")]
        public string Legend
        {
            get { return Header; }
            set { Header = value; }
        }

        /// <summary>
        /// Gets or sets the items collection for this instance.
        /// </summary>
        /// <value>The fieldset items as an <see cref="ItemsCollection&lt;T&gt;"/>.</value>
        [Obsolete("Fieldset implements IList.  Use the IList methods instead.")]
        public ItemsCollection<Field> Fields
        {
            get { return Items; }
            set { Items = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fieldset"/> class using a list layout.
        /// </summary>
        public Fieldset()
        {
            Layout = FieldsetLayout.List;
            Items = new List<Field>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fieldset"/> class using a list layout.
        /// </summary>
        /// <param name="header">The header text to display.</param>
        public Fieldset(string header)
            : this()
        {
            Header = header;
        }

        /// <summary>
        /// Builds a table of fields laid out in multiple rows and columns.
        /// Each row has a different <see cref="Field"/> control, and each column is a duplication of the first under a different header.
        /// </summary>
        /// <param name="header">The aggregated fieldset's header.</param>
        /// <param name="columns">The headers of each column.  The number of columns is determined by the number of column headers.</param>
        /// <param name="rows">The fields that make up each row.  The number of rows is determined by the number of fields.</param>
        /// <returns>A list of fieldsets that create an aggreagate fieldset.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if there is not at least one column.</exception>
        public static List<iLayerItem> BuildColumnFieldset(string header, IEnumerable<string> columns, IEnumerable<Field> rows)
        {
            return BuildColumnFieldset(header, FieldsetLayout.Simple, columns, rows);
        }

        /// <summary>
        /// Builds a table of fields laid out in multiple rows and columns.
        /// Each row has a different <see cref="Field"/> control, and each column is a duplication of the first under a different header.
        /// </summary>
        /// <param name="header">The aggregated fieldset's header.</param>
        /// <param name="layout">How the fieldset is laid out.</param>
        /// <param name="columns">The headers of each column.  The number of columns is determined by the number of column headers.</param>
        /// <param name="rows">The fields that make up each row.  The number of rows is determined by the number of fields.</param>
        /// <returns>A list of fieldsets that create an aggreagate fieldset.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if there is not at least one column.</exception>
        public static List<iLayerItem> BuildColumnFieldset(string header, FieldsetLayout layout, IEnumerable<string> columns, IEnumerable<Field> rows)
        {
            // only build the AggregateFieldset if more than one column exists
            if (columns.Count() == 1)
            {
                string column = columns.ElementAt(0);

                var fields = new List<Field>();
                foreach (var row in rows)
                {
                    var item = row.Clone();
                    item.ID = string.Concat(column, ".", item.ID);
                    fields.Add(item);
                }

                return new List<iLayerItem>(new[] { new Fieldset
                {
                    Header = column,
                    Items = fields,
                    Layout = layout,
                } });
            }
            if (!columns.Any()) { throw new ArgumentOutOfRangeException("columns"); }

            var fieldsets = new List<iLayerItem>();
            foreach (var column in columns)
            {
                var fields = new List<Field>();
                foreach (var row in rows)
                {
                    var item = row.Clone();
                    item.ID = string.Concat(column, ".", item.ID);
                    fields.Add(item);
                }

                fieldsets.Add(new AggregateFieldset
                {
                    Header = column,
                    Items = fields,
                    Layout = layout,
                });
            }
            ((AggregateFieldset)fieldsets.First()).AggregateHeader = header;
            return fieldsets;
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