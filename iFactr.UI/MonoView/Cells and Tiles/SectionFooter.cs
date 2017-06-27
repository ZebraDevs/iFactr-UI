using System;
using iFactr.Core;
using MonoCross.Navigation;
#if !NETCF
using System.Diagnostics;
#endif

namespace iFactr.UI
{
    /// <summary>
    /// Represents a UI element that acts as a footer for a <see cref="Section"/>.
    /// </summary>
    public class SectionFooter : ISectionFooter
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

        /// <summary>
        /// The name of the <see cref="P:Font"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string FontProperty = "Font";

        /// <summary>
        /// The name of the <see cref="P:Text"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TextProperty = "Text";
        #endregion

        /// <summary>
        /// Gets or sets the background color of the footer.
        /// </summary>
        public Color BackgroundColor
        {
            get { return Pair.BackgroundColor; }
            set { Pair.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the foreground content of the footer.
        /// </summary>
        public Color ForegroundColor
        {
            get { return Pair.ForegroundColor; }
            set { Pair.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the font to be used when rendering the text.
        /// </summary>
        public Font Font
        {
            get { return Pair.Font; }
            set { Pair.Font = value; }
        }

        /// <summary>
        /// Gets or sets the text that will be displayed on screen.
        /// </summary>
        public string Text
        {
            get { return Pair.Text; }
            set { Pair.Text = value; }
        }

        /// <summary>
        /// Gets or sets the native object that is paired with this instance.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected ISectionFooter Pair
        {
            get
            {
                if (pair == null)
                {
                    throw new InvalidOperationException("No native cell was found for the current instance.");
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
        private ISectionFooter pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as ISectionFooter; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionFooter"/> class.
        /// </summary>
        public SectionFooter()
        {
            Pair = MXContainer.Resolve<ISectionFooter>();

            pair.BackgroundColor = iApp.Instance.Style.SectionHeaderColor;
            pair.ForegroundColor = iApp.Instance.Style.SectionHeaderTextColor;
            pair.Font = Font.PreferredSectionFooterFont;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionFooter"/> class.
        /// </summary>
        /// <param name="text">The text to be displayed on the screen.</param>
        public SectionFooter(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            SectionFooter header = obj as SectionFooter;
            if (header != null)
            {
                return Pair == header.Pair;
            }

            ISectionFooter iheader = obj as ISectionFooter;
            if (iheader != null)
            {
                return Pair == iheader;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ISectionFooter"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="ISectionFooter"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="ISectionFooter"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(ISectionFooter other)
        {
            SectionFooter header = other as SectionFooter;
            if (header != null)
            {
                return Pair == header.Pair;
            }

            return Pair == other;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="SectionFooter"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in
        /// hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return Pair.GetHashCode();
        }
    }
}
