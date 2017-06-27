using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using iFactr.UI;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a layer containing <see cref="Tab"/> objects for display within a Tabs pane.
    /// </summary>
    public class NavigationTabs : iLayer
    {
        /// <summary>
        /// Gets a collection of <see cref="Tab"/> objects.
        /// </summary>
        public NavigationTabItems TabItems
        {
            get { return _tabItems; }
            set { _tabItems = value ?? new NavigationTabItems(); }
        }
        private NavigationTabItems _tabItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationTabs"/> class.
        /// </summary>
        public NavigationTabs()
        {
            TabItems = new NavigationTabItems();
        }

        /// <summary>
        /// Gets an <see cref="iList"/> representation of this layer's <see cref="Tab"/>s.
        /// </summary>
        [XmlIgnore]
        public iList List
        {
            get
            {
                var list = new iList();
                list.OnItemSelection += item =>
                {
                    iApp.SetNavigationContext(NavContext);
                    iApp.CurrentNavContext.ActiveTab = list.IndexOf(item);
                    item.Link.Parameters.AddRange(ActionParameters);
                    iApp.Navigate(item.Link, View);
                };
                foreach (var tab in TabItems)
                {
                    list.Items.Add(tab);
                }
                return list;
            }
        }

        /// <summary>
        /// Removes all UI elements and validation errors from the layer.
        /// </summary>
        public override void Clear()
        {
            TabItems.Clear();
            base.Clear();
        }

        /// <summary>
        /// Represents a collection of <see cref="Tab"/> objects.
        /// </summary>
        public class NavigationTabItems : ItemsCollection<Tab>
        {
            /// <summary>
            ///  Adds a navigation tab item using the title and navigation URL provided.
            /// </summary>
            /// <param name="title">A <see cref="String"/> representing the title of the tab.</param>
            /// <param name="navigationUrl">A <see cref="String"/> representing the URL to navigate to when the tab is first selected.</param>
            public void Add(string title, string navigationUrl)
            {
                lock (SyncRoot)
                {
                    if (this.FirstOrDefault(tab => tab.Link.Address == navigationUrl) == null)
                        Add(new Tab(title, navigationUrl));
                }
            }

            /// <summary>
            /// Adds a navigation tab item using the title and navigation URL provided.
            /// </summary>
            /// <param name="title">A <see cref="String"/> representing the title of the tab.</param>
            /// <param name="navigationUrl">A <see cref="String"/> representing the URL to navigate to when the tab is first selected.</param>
            /// <param name='refreshOnFocus'>Whether the tab should navigate to the navigationUrl every time it is selected.</param>
            public void Add(string title, string navigationUrl, bool refreshOnFocus)
            {
                lock (SyncRoot)
                {
                    if (this.FirstOrDefault(tab => tab.Link.Address == navigationUrl) == null)
                        Add(new Tab(title, navigationUrl, refreshOnFocus));
                }
            }

            /// <summary>
            ///  Adds a navigation tab item using the title, navigation URL, and icon provided.
            /// </summary>
            /// <param name="title">A <see cref="String"/> representing the title of the tab.</param>
            /// <param name="navigationUrl">A <see cref="String"/> representing the URL to navigate to when the tab is first selected.</param>
            /// <param name="icon">A <see cref="String"/> representing the path of the icon to use for the tab.</param>
            public void Add(string title, string navigationUrl, string icon)
            {
                lock (SyncRoot)
                {
                    if (this.FirstOrDefault(tab => tab.Link.Address == navigationUrl) == null)
                        Add(new Tab(title, navigationUrl, icon));
                }
            }

            /// <summary>
            ///  Adds a navigation tab item using the title, navigation URL, and icon provided.
            /// </summary>
            /// <param name="title">A <see cref="String"/> representing the title of the tab.</param>
            /// <param name="navigationUrl">A <see cref="String"/> representing the URL to navigate to when the tab is first selected.</param>
            /// <param name="icon">A <see cref="String"/> representing the path of the icon to use for the tab.</param>
            /// <param name='refreshOnFocus'>Whether the tab should navigate to the navigationUrl every time it is selected.</param>
            public void Add(string title, string navigationUrl, string icon, bool refreshOnFocus)
            {
                lock (SyncRoot)
                {
                    if (this.FirstOrDefault(tab => tab.Link.Address == navigationUrl) == null)
                        Add(new Tab(title, navigationUrl, icon, refreshOnFocus));
                }
            }

            /// <summary>
            ///  Adds a navigation tab item using the title, navigation URL, icon, and badge text provided.
            /// </summary>
            /// <param name="title">A <see cref="String"/> representing the title of the tab.</param>
            /// <param name="navigationUrl">A <see cref="String"/> representing the URL to navigate to when the tab is first selected.</param>
            /// <param name="icon">A <see cref="String"/> representing the path of the icon to use for the tab.</param>
            /// <param name="badge">A <see cref="String"/> representing the text to display in the tab's badge.</param>
            public void Add(string title, string navigationUrl, string icon, string badge)
            {
                lock (SyncRoot)
                {
                    if (this.FirstOrDefault(tab => tab.Link.Address == navigationUrl) == null)
                        Add(new Tab(title, navigationUrl, icon, badge));
                }
            }

            /// <summary>
            ///  Adds a navigation tab item using the title, navigation URL, icon, and badge text provided.
            /// </summary>
            /// <param name="title">A <see cref="String"/> representing the title of the tab.</param>
            /// <param name="navigationUrl">A <see cref="String"/> representing the URL to navigate to when the tab is first selected.</param>
            /// <param name="icon">A <see cref="String"/> representing the path of the icon to use for the tab.</param>
            /// <param name="badge">A <see cref="String"/> representing the text to display in the tab's badge.</param>
            /// <param name='refreshOnFocus'>Whether the tab should navigate to the navigationUrl every time it is selected.</param>
            public void Add(string title, string navigationUrl, string icon, string badge, bool refreshOnFocus)
            {
                lock (SyncRoot)
                {
                    if (this.FirstOrDefault(tab => tab.Link.Address == navigationUrl) == null)
                        Add(new Tab(title, navigationUrl, icon, badge, refreshOnFocus));
                }
            }
        }
    }
}