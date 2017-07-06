using System;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents an item in an <see cref="iFactr.UI.IToolbar"/> object that serves as a separator between other items.
    /// </summary>
    public class ToolbarSeparator : IToolbarSeparator
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";
        #endregion

        /// <summary>
        /// Gets or sets the foreground color of the item.
        /// </summary>
        public Color ForegroundColor
        {
            get { return Pair.ForegroundColor; }
            set { Pair.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the toolbar separator.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected IToolbarSeparator Pair
        {
            get
            {
                if (pair == null)
                {
                    throw new InvalidOperationException("No native object was found for the current instance.");
                }
                return pair;
            }
            set
            {
                if (pair == null && value != null)
                {
                    pair = value;
                    pair.Pair = this;
                }
            }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IToolbarSeparator pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as IToolbarSeparator; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.ToolbarSeparator"/> class.
        /// </summary>
        public ToolbarSeparator()
        {
            Pair = MXContainer.Resolve<IToolbarSeparator>();

            pair.ForegroundColor = new Color();
        }

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.IToolbarSeparator"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.IToolbarSeparator"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.IToolbarSeparator"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(IToolbarSeparator other)
        {
            ToolbarSeparator item = other as ToolbarSeparator;
            if (item != null)
            {
                return Pair == item.Pair;
            }

            return Pair == other;
        }
    }
}
