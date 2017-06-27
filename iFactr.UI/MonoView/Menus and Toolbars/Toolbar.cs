using System;
using System.Collections.Generic;
using System.Diagnostics;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI
{
    /// <summary>
    /// Represents a toolbar containing items that provide support functions for an <see cref="iFactr.UI.ICanvasView"/>.
    /// </summary>
    public class Toolbar : IToolbar
    {
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackgroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackgroundColorProperty = "BackgroundColor";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";
        #endregion

        /// <summary>
        /// Gets or sets the background color of the toolbar on platforms that support it.
        /// </summary>
        public Color BackgroundColor
        {
            get { return Pair.BackgroundColor; }
            set { Pair.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the foreground color of the toolbar on platforms that support it.
        /// Any toolbar items that do not specify their own foreground color will use this value.
        /// </summary>
        public Color ForegroundColor
        {
            get { return Pair.ForegroundColor; }
            set { Pair.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets a collection of toolbar items that are considered primary commands.
        /// These are commonly aligned to the right of the toolbar, but platform behavior may vary.
        /// </summary>
        public IEnumerable<IToolbarItem> PrimaryItems
        {
            get { return Pair.PrimaryItems; }
            set { Pair.PrimaryItems = value; }
        }

        /// <summary>
        /// Gets or sets a collection of toolbar items that are considered secondary commands.
        /// These are commonly aligned to the left of the toolbar, but platform behavior may vary.
        /// </summary>
        public IEnumerable<IToolbarItem> SecondaryItems
        {
            get { return Pair.SecondaryItems; }
            set { Pair.SecondaryItems = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with the toolbar.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected IToolbar Pair
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
        private IToolbar pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as IToolbar; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Toolbar"/> class.
        /// </summary>
        public Toolbar()
        {
            Pair = MXContainer.Resolve<IToolbar>();

            pair.BackgroundColor = new Color();
            pair.ForegroundColor = new Color();
        }

        /// <summary>
        /// Determines whether the specified <see cref="iFactr.UI.IToolbar"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="iFactr.UI.IToolbar"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="iFactr.UI.IToolbar"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(IToolbar other)
        {
            Toolbar toolbar = other as Toolbar;
            if (toolbar != null)
            {
                return Pair == toolbar.Pair;
            }

            return Pair == other;
        }
    }
}
