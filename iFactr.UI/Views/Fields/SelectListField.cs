using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field with multiple options that can be selected.
    /// </summary>
    public class SelectListField : Field
    {
        #region Properties

        /// <summary>
        /// Gets or sets a collection of <see cref="SelectListFieldItem"/>s to populate this instance with.
        /// </summary>
        public List<SelectListFieldItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        /// <value>The index of the selected item.</value>
        [XmlIgnore]
        public int SelectedIndex
        {
            get { return Items == null || _selectedIndex > Items.Count - 1 ? -1 : _selectedIndex; }
            set
            {
                if (Items == null || !Items.Any())
                {
                    _selectedIndex = value;
                    return;
                }

                if (value < 0 || value >= Items.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                _selectedIndex = value;
                Text = SelectedValue;
            }
        }

        /// <summary>
        /// Gets or sets the selected <see cref="SelectListFieldItem"/>.
        /// </summary>
        /// <remarks>If this is set to be a value that isn't in the list of items, the first item in the list is chosen instead.</remarks>
        public SelectListFieldItem SelectedItem
        {
            get
            {
                return SelectedIndex == -1 ? null : Items[_selectedIndex];
            }
            set
            {
                SelectedIndex = value != null && Items.Contains(value) ? Items.IndexOf(value) : 0;
            }
        }

        /// <summary>
        /// Gets or sets the key to the selected item.
        /// </summary>
        /// <remarks>If this is set to be a key that isn't in any item, the first item in the list is chosen instead.</remarks>
        [XmlIgnore]
        public string SelectedKey
        {
            get
            {
                return SelectedIndex == -1 ? null : Items[_selectedIndex].Key ?? _selectedIndex.ToString(CultureInfo.InvariantCulture);
            }
            set
            {
                foreach(var item in Items)
                {
                    if (item.Key == value)
                    {
                        SelectedItem = item;
                        return;
                    }
                }
                SelectedItem = Items.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets or sets the value of the selected item.
        /// </summary>
        /// <remarks>If this is set to be a value that isn't the Value of any item, the first item in the list is chosen instead.</remarks>
        [XmlIgnore]
        public string SelectedValue
        {
            get
            {
                return SelectedIndex == -1 ? null : Items[_selectedIndex].Value;
            }
            set
            {
                foreach (var item in Items)
                {
                    if (item.Value == value)
                    {
                        SelectedItem = item;
                        return;
                    }
                }
                SelectedItem = Items.FirstOrDefault();
            }
        }

        private int _selectedIndex;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListField"/> class with no submission ID.
        /// </summary>
        public SelectListField()
        {
            Items = new List<SelectListFieldItem>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListField"/> class.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        public SelectListField(string id) : this(id, new string[] { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListField"/> class.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        /// <param name='items'>A group of strings representing the display text of each <see cref="SelectListFieldItem"/>.</param>
        public SelectListField(string id, params string[] items) : this(id, items.ToList()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListField"/> class.
        /// </summary>
        /// <param name="items">A <see cref="IEnumerable&lt;T&gt;"/> representing the Items value.</param>
        [Obsolete("Use SelectListField(string id, IEnumerable<string> items) instead")]
        public SelectListField(IEnumerable<string> items) : this(null, items) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListField"/> class.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        /// <param name='items'>A group of strings representing the display text of each <see cref="SelectListFieldItem"/>.</param>
        public SelectListField(string id, IEnumerable<string> items)
        {
            ID = id;
            SetItemsArray(items);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListField"/> class.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        /// <param name='items'>A group of strings representing the key and display text of each <see cref="SelectListFieldItem"/>.</param>
        public SelectListField(string id, IEnumerable<KeyValuePair<string, string>> items)
        {
            ID = id;
            SetItemsArray(items);
        }

        #endregion

        /// <summary>
        /// Creates <see cref="SelectListFieldItem"/>s from the specified strings and populates this instance with them.
        /// </summary>
        /// <param name="items">An <see cref="IEnumerable&lt;T&gt;"/> containing the strings to create <see cref="SelectListFieldItem"/>s from.</param>
        public void SetItemsArray(IEnumerable<string> items)
        {
            Items = new List<SelectListFieldItem>(items.Select(item => new SelectListFieldItem(item)));
            if (Items.Count > 0)
                SelectedIndex = 0;
        }

        /// <summary>
        /// Creates <see cref="SelectListFieldItem"/>s from the specified strings and populates this instance with them.
        /// </summary>
        /// <param name="items">An <see cref="IEnumerable&lt;T&gt;"/> containing the keys and values to create <see cref="SelectListFieldItem"/>s from.</param>
        public void SetItemsArray(IEnumerable<KeyValuePair<string, string>> items)
        {
            Items = new List<SelectListFieldItem>(items.Select(item => new SelectListFieldItem(item.Key, item.Value)));
            if (Items.Count > 0)
                SelectedIndex = 0;
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public new SelectListField Clone()
        {
            return (SelectListField)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected override object CloneOverride()
        {
            var slf = (SelectListField)base.CloneOverride();
            slf.Items = new List<SelectListFieldItem>();
            foreach (var item in Items)
            {
                slf.Items.Add(item.Clone());
            }
            slf.SelectedIndex = SelectedIndex;
            return slf;
        }
    }
}