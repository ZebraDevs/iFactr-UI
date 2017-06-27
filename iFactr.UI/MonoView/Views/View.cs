using System;
using System.Diagnostics;
using iFactr.Core;

namespace iFactr.UI
{
	/// <summary>
    /// Represents the base class for the <see cref="ListView&lt;T&gt;"/>, <see cref="GridView&lt;T&gt;"/>,
    /// <see cref="TabView&lt;T&gt;"/>, <see cref="BrowserView&lt;T&gt;"/>, and <see cref="CanvasView&lt;T&gt;"/> classes.
    /// This class is abstract.
	/// </summary>
	public abstract class View : IView
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:HeaderColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string HeaderColorProperty = "HeaderColor";

        /// <summary>
        /// The name of the <see cref="P:Height"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string HeightProperty = "Height";

        /// <summary>
        /// The name of the <see cref="P:Width"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string WidthProperty = "Width";

        /// <summary>
        /// The name of the <see cref="P:PreferredOrientations"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PreferredOrientationsProperty = "PreferredOrientations";

        /// <summary>
        /// The name of the <see cref="P:Title"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TitleProperty = "Title";

        /// <summary>
        /// The name of the <see cref="P:TitleColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TitleColorProperty = "TitleColor";
        #endregion

        /// <summary>
        /// Occurs when the view is being rendered.
        /// </summary>
        public event EventHandler Rendering
        {
            add { Pair.Rendering += value; }
            remove { Pair.Rendering -= value; }
        }

        /// <summary>
        /// Gets or sets the color of the header bar, if there is one.
        /// </summary>
        public Color HeaderColor
        {
            get { return Pair.HeaderColor; }
            set { Pair.HeaderColor = value; }
        }

		/// <summary>
		/// Gets the current height value of the view in native coordinates.
		/// </summary>
		public double Height
		{
			get { return Pair.Height; }
		}

		/// <summary>
		/// Gets the current width value of the view in native coordinates.
		/// </summary>
		public double Width
		{
			get { return Pair.Width; }
		}

        /// <summary>
        /// Gets a collection for storing arbitrary data on the object.
        /// </summary>
        public MetadataCollection Metadata
        {
            get { return Pair.Metadata; }
        }

        /// <summary>
        /// Gets or sets the orientation preference for the view.
        /// </summary>
        public PreferredOrientation PreferredOrientations
        {
            get { return Pair.PreferredOrientations; }
            set { Pair.PreferredOrientations = value; }
        }

		/// <summary>
		/// Gets or sets the title for the view.
		/// </summary>
		public string Title
		{
			get { return Pair.Title; }
			set { Pair.Title = value; }
		}

        /// <summary>
        /// Gets or sets the color with which to display the title.
        /// </summary>
        public Color TitleColor
        {
            get { return Pair.TitleColor; }
            set { Pair.TitleColor = value; }
        }

        /// <summary>
        /// Gets the type of the model displayed by the view.
        /// </summary>
        public abstract Type ModelType { get; }

        /// <summary>
        /// Gets or sets the native object that is paired with the view.  This can be set only once.
        /// </summary>
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        protected IView Pair
        {
            get
            {
                if (_pair == null)
                {
                    throw new InvalidOperationException("No native view was found for the current instance.");
                }
                return _pair;
            }
            set
            {
                if (_pair == null && value != null)
                {
                    _pair = value;
                    _pair.Pair = this;

                    _pair.HeaderColor = iApp.Instance.Style.HeaderColor;
                    _pair.PreferredOrientations = PreferredOrientation.PortraitOrLandscape;
                    _pair.TitleColor = iApp.Instance.Style.HeaderTextColor;
                    _pair.SetBackground(iApp.Instance.Style.LayerBackgroundColor);
                }
            }
        }
#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private IView _pair;

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        IPairable IPairable.Pair
        {
            get { return Pair; }
            set { Pair = value as IView; }
        }

		/// <summary>
		/// Sets the background of the view to the specified <see cref="iFactr.UI.Color"/>.
		/// </summary>
		/// <param name="color">The color to set the background to.</param>
		public void SetBackground(Color color)
		{
			Pair.SetBackground(color);
		}

		/// <summary>
		/// Sets the background of the view to the image at the specified file path.
		/// </summary>
		/// <param name="imagePath">The file path of the image to set the background to.</param>
		/// <param name="stretch">The way the image is stretched to fill the available space.</param>
		public void SetBackground(string imagePath, ContentStretch stretch)
		{
			Pair.SetBackground(imagePath, stretch);
		}

        /// <summary>
        /// Gets the model for the view.
        /// </summary>
        public abstract object GetModel();

        /// <summary>
        /// Sets the model for the view.
        /// </summary>
        /// <param name="model">The object to set the model to.</param>
        /// <exception cref="InvalidCastException">Thrown when the <paramref name="model"/> is of an incorrect type.</exception>
        public abstract void SetModel(object model);

        /// <summary>
        /// Renders the view.  Override the <see cref="OnRender"/> method to perform view-specific application logic.
        /// </summary>
        public void Render()
        {
            OnRender();
            Pair.Render();
        }

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			View view = obj as View;
			if (view != null)
			{
				return Pair == view.Pair;
			}
			
			IView iview = obj as IView;
			if (iview != null)
			{
				return Pair == iview;
			}
			
			return false;
		}

		/// <summary>
		/// Determines whether the specified <see cref="iFactr.UI.IView"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="iFactr.UI.IView"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="iFactr.UI.IView"/> is equal to this instance;
		/// otherwise, <c>false</c>.</returns>
		public bool Equals(IView other)
		{
			View view = other as View;
			if (view != null)
			{
				return Pair == view.Pair;
			}
			
			return Pair == other;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="iFactr.UI.View"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in
		/// hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			return Pair.GetHashCode();
		}

        /// <summary>
        /// Called when the view is ready to be rendered.
        /// </summary>
        protected virtual void OnRender()
        {
        }
	}
}