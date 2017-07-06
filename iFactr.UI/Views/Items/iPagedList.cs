using System;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents a paginated list of <see cref="iItem"/>s on an <see cref="iLayer"/>.
    /// </summary>
    public class iPagedList : iList
    {
        /// <summary>
        /// Gets or sets the maximum number of items to allow on a single page.
        /// </summary>
        /// <value>The page size as a <see cref="Int32"/> value.</value>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value >= 0 ? value : 0; }
        }
        private int _pageSize = 15;

        /// <summary>
        /// Gets or sets a value that is used by bindings to cache pages for pagination support.
        /// </summary>
        public string PagedLayerID { get; set; }

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="iPagedList"/> class.
        /// </summary>
        public iPagedList() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="iPagedList"/> class.
        /// </summary>
        /// <param name="name">The name of this instance.</param>
        public iPagedList(string name)
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iPagedList"/> class.
        /// </summary>
        /// <param name="name">The name of this instance.</param>
        /// <param name="pageSize">The maximum number of items to allow on a single page.</param>
        public iPagedList(string name, int pageSize)
            : this(name)
        {
            PageSize = pageSize;
        }
        #endregion
    }
}
