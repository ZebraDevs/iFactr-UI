using System.Diagnostics;

using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that holds a boolean value and can be toggled by the user.
    /// </summary>
	public class Switch : Control, ISwitch
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:FalseColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string FalseColorProperty = "FalseColor";

        /// <summary>
        /// The name of the <see cref="P:TrueColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string TrueColorProperty = "TrueColor";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:Value"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ValueProperty = "Value";
        #endregion

        /// <summary>
        /// Occurs when the switch's boolean value has changed.
        /// </summary>
		[NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
		public event ValueChangedEventHandler<bool> ValueChanged
		{
			add { NativeControl.ValueChanged += value; }
			remove { NativeControl.ValueChanged -= value; }
		}

        /// <summary>
        /// Gets or sets the color of the switch when its value is <c>false</c>.
        /// </summary>
		public Color FalseColor
		{
			get { return NativeControl.FalseColor; }
			set { NativeControl.FalseColor = value; }
		}
		
        /// <summary>
        /// Gets or sets the color of the switch when its value is <c>true</c>.
        /// </summary>
		public Color TrueColor
		{
			get { return NativeControl.TrueColor; }
			set { NativeControl.TrueColor = value; }
		}
		
        /// <summary>
        /// Gets or sets the color of any additional visual information that is part of the switch.
        /// For example, Metro and Windows Phone include text that represents the value of the switch.
        /// </summary>
		public Color ForegroundColor
		{
			get { return NativeControl.ForegroundColor; }
			set { NativeControl.ForegroundColor = value; }
		}
		
        /// <summary>
        /// Gets or sets the boolean value of the switch.
        /// </summary>
		public bool Value
		{
			get { return NativeControl.Value; }
			set { NativeControl.Value = value; }
		}

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private ISwitch NativeControl
        {
            get { return (ISwitch)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Switch"/> class.
        /// </summary>
		public Switch()
		{
			Pair = MXContainer.Resolve<ISwitch>();

            NativeControl.FalseColor = new Color();
            NativeControl.ForegroundColor = iApp.Instance.Style.TextColor;
            NativeControl.TrueColor = new Color();
            NativeControl.HorizontalAlignment = HorizontalAlignment.Left;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.Switch"/> class.
        /// </summary>
        /// <param name="value">The initial boolean value.</param>
		public Switch(bool value)
			: this()
		{
			Value = value;
		}
	}
}

