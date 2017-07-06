using System;
using iFactr.Core;
using MonoCross.Navigation;
#if !NETCF
using System.Diagnostics;
#endif

namespace iFactr.UI
{
    /// <summary>
    /// Represents a UI element that acts as a header for a <see cref="Section"/>.
    /// </summary>
    public class SectionHeader : ISectionHeader
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
        /// Gets or sets the background color of the header.
        /// </summary>
        public Color BackgroundColor
        {
            get { return Pair.BackgroundColor; }
            set { Pair.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the foreground content of the header.
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
        protected ISectionHeader Pair
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
        private ISectionHeader pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as ISectionHeader; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionHeader"/> class.
        /// </summary>
        public SectionHeader()
        {
            Pair = MXContainer.Resolve<ISectionHeader>();

            pair.BackgroundColor = iApp.Instance.Style.SectionHeaderColor;
            pair.ForegroundColor = iApp.Instance.Style.SectionHeaderTextColor;
            pair.Font = Font.PreferredSectionHeaderFont;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionHeader"/> class.
        /// </summary>
        /// <param name="text">The text to be displayed on the screen.</param>
        public SectionHeader(string text)
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
            SectionHeader header = obj as SectionHeader;
            if (header != null)
            {
                return Pair == header.Pair;
            }

            ISectionHeader iheader = obj as ISectionHeader;
            if (iheader != null)
            {
                return Pair == iheader;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ISectionHeader"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="ISectionHeader"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="ISectionHeader"/> is equal to this instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(ISectionHeader other)
        {
            SectionHeader header = other as SectionHeader;
            if (header != null)
            {
                return Pair == header.Pair;
            }

            return Pair == other;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="SectionHeader"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in
        /// hashing algorithms and data structures such as a hash table.</returns>
        public override int GetHashCode()
        {
            return Pair.GetHashCode();
        }
    }
}
