using System;
using System.Diagnostics;

using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Represents a UI element that can accept a single line of text input from the user.
    /// The input is masked so that it cannot be read, allowing for secured entry of sensitive information.
    /// </summary>
	public class PasswordBox : Control, IPasswordBox
	{
        #region Property Names
        /// <summary>
        /// The name of the <see cref="P:BackgroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string BackgroundColorProperty = "BackgroundColor";

        /// <summary>
        /// The name of the <see cref="P:Expression"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ExpressionProperty = "Expression";

        /// <summary>
        /// The name of the <see cref="P:Font"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string FontProperty = "Font";

        /// <summary>
        /// The name of the <see cref="P:ForegroundColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string ForegroundColorProperty = "ForegroundColor";

        /// <summary>
        /// The name of the <see cref="P:IsFocused"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string IsFocusedProperty = "IsFocused";

        /// <summary>
        /// The name of the <see cref="P:KeyboardType"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string KeyboardTypeProperty = "KeyboardType";

        /// <summary>
        /// The name of the <see cref="P:KeyboardReturnType"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string KeyboardReturnTypeProperty = "KeyboardReturnType";

        /// <summary>
        /// The name of the <see cref="P:Password"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PasswordProperty = "Password";

        /// <summary>
        /// The name of the <see cref="P:Placeholder"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PlaceholderProperty = "Placeholder";

        /// <summary>
        /// The name of the <see cref="P:PlaceholderColor"/> property.  Use this when binding to the property.  This field is read-only.
        /// </summary>
        public static readonly string PlaceholderColorProperty = "PlaceholderColor";
        #endregion

        /// <summary>
        /// Occurs when the password box receives focus.
        /// </summary>
        public event EventHandler GotFocus
        {
            add { NativeControl.GotFocus += value; }
            remove { NativeControl.GotFocus -= value; }
        }

        /// <summary>
        /// Occurs when the password box loses focus.
        /// </summary>
        public event EventHandler LostFocus
        {
            add { NativeControl.LostFocus += value; }
            remove { NativeControl.LostFocus -= value; }
        }

        /// <summary>
        /// Occurs when the password box's password value has changed.
        /// </summary>
		[NativeOnly("http://support.ifactr.com/kb/webkit-compatibility")]
		public event ValueChangedEventHandler<string> PasswordChanged
		{
			add { NativeControl.PasswordChanged += value; }
			remove { NativeControl.PasswordChanged -= value; }
		}

        /// <summary>
        /// Occurs when the 'Return' key on the keyboard is pressed.
        /// </summary>
        public event EventHandler<EventHandledEventArgs> ReturnKeyPressed
        {
            add { NativeControl.ReturnKeyPressed += value; }
            remove { NativeControl.ReturnKeyPressed -= value; }
        }

        /// <summary>
        /// Gets or sets the background color of the password box.
        /// </summary>
		public Color BackgroundColor
		{
			get { return NativeControl.BackgroundColor; }
			set { NativeControl.BackgroundColor = value; }
		}

        /// <summary>
        /// Gets or sets a regular expression string for restricting the text that the user can input.
        /// A value of <c>null</c> means that there is no restriction.
        /// </summary>
		public string Expression
		{
			get { return NativeControl.Expression; }
			set { NativeControl.Expression = value; }
		}

        /// <summary>
        /// Gets or sets the font to be used when rendering masked input characters.
        /// </summary>
		public Font Font
		{
			get { return NativeControl.Font; }
			set { NativeControl.Font = value; }
		}
		
        /// <summary>
        /// Gets or sets the color of masked input characters.
        /// </summary>
		public Color ForegroundColor
		{
			get { return NativeControl.ForegroundColor; }
			set { NativeControl.ForegroundColor = value; }
		}

        /// <summary>
        /// Gets a value indicating whether or not this instance has input focus.
        /// </summary>
        public bool IsFocused
        {
            get { return NativeControl.IsFocused; }
        }

        /// <summary>
        /// Gets or sets the type of soft keyboard to use when the password box has focus.
        /// </summary>
		public KeyboardType KeyboardType
		{
			get { return NativeControl.KeyboardType; }
			set { NativeControl.KeyboardType = value; }
		}

        /// <summary>
        /// Gets or sets how the 'Return' key of a soft keyboard should be presented when the password box has focus.
        /// </summary>
        public KeyboardReturnType KeyboardReturnType
        {
            get { return NativeControl.KeyboardReturnType; }
            set { NativeControl.KeyboardReturnType = value; }
        }

        /// <summary>
        /// Gets or sets the password value inside of the password box.
        /// </summary>
		public string Password
		{
			get { return NativeControl.Password; }
			set { NativeControl.Password = value; }
		}

        /// <summary>
        /// Gets or sets the text to display when the password box does not have a value.
        /// </summary>
        public string Placeholder
        {
            get { return NativeControl.Placeholder; }
            set { NativeControl.Placeholder = value; }
        }

        /// <summary>
        /// Gets or sets the color of the placeholder text.
        /// </summary>
        public Color PlaceholderColor
        {
            get { return NativeControl.PlaceholderColor; }
            set { NativeControl.PlaceholderColor = value; }
        }

#if !NETCF
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
		private IPasswordBox NativeControl
        {
            get { return (IPasswordBox)Pair; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.PasswordBox"/> class.
        /// </summary>
		public PasswordBox()
		{
			Pair = MXContainer.Resolve<IPasswordBox>();

            NativeControl.BackgroundColor = new Color();
            NativeControl.ForegroundColor = new Color();
            NativeControl.Font = Font.PreferredTextBoxFont;
            NativeControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            NativeControl.VerticalAlignment = VerticalAlignment.Top;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="iFactr.UI.Controls.PasswordBox"/> class.
        /// </summary>
        /// <param name="password">The initial password value.</param>
		public PasswordBox(string password)
			: this()
		{
			Password = password;
		}

        /// <summary>
        /// Programmatically sets input focus to the password box.
        /// </summary>
		public void Focus()
		{
			NativeControl.Focus();
		}
	}
}

