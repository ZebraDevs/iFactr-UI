using System;
using System.Linq;
using MonoCross.Utilities;

namespace iFactr.UI.Reflection
{
    /// <summary>
    /// Represents a user control that will be rendered on the screen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ControlAttribute : OptInAttribute
    {
        internal ControlType Type { get; private set; }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color for the control.
        /// </summary>
        public string ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the name of the font family to be used when the control renders text.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets the size of the font that is used when the control renders text.
        /// </summary>
        public string FontSize { get; set; }

        /// <summary>
        /// Gets or sets the formatting to apply to the font that is used when the control renders text.
        /// </summary>
        public FontFormatting FontFormatting { get; set; }

        /// <summary>
        /// Gets or sets the address to navigate to when a button control has been pressed.
        /// </summary>
        public string ButtonAddress { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of lines that are allowed to be displayed when presenting the text of a label control.
        /// A value of 0 means that there is no limit.
        /// </summary>
        public string LabelLines { get; set; }

        /// <summary>
        /// Gets or sets a string describing the format to use when presenting the value of a picker control.
        /// </summary>
        public string PickerValueFormat { get; set; }

        /// <summary>
        /// Gets or sets the maximum numerical value that a user can select when using a sliding control.
        /// </summary>
        public string SliderMaxValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum numerical value that a user can select when using a sliding control.
        /// </summary>
        public string SliderMinValue { get; set; }

        /// <summary>
        /// Gets or sets the type of <see cref="IValueConverter"/> to use when passing a value to and from the control.
        /// </summary>
        public Type ValueConverterType
        {
            get { return valueConverterType; }
            set
            {
                if (value != null && !Device.Reflector.HasInterface(value, typeof(IValueConverter)))
                {
                    throw new ArgumentException("ValueConverterType value must implement the iFactr.UI.IValueConverter interface");
                }

                valueConverterType = value;
            }
        }
        private Type valueConverterType;

        /// <summary>
        /// Gets or sets the key to use when submitting the control value.
        /// </summary>
        public string SubmitKey
        {
            get { return submitKey; }
            set
            {
                submitKey = value;
                IsSubmitKeySet = true;
            }
        }
        private string submitKey;
        internal bool IsSubmitKeySet { get; private set; }
        
        /// <summary>
        /// Gets or sets the alignment of the text that a user inputs into a text entry control.
        /// </summary>
        public TextAlignment TextEntryAlignment { get; set; }

        /// <summary>
        /// Gets or sets automatic completion behavior for text entered into a text entry control.
        /// </summary>
        public TextCompletion TextEntryCompletion { get; set; }

        /// <summary>
        /// Gets or sets a regular expression string for restricting the text that the user can input into a text entry control.
        /// </summary>
        public string TextEntryExpression { get; set; }

        /// <summary>
        /// Gets or sets the type of soft keyboard to use when a text entry control has focus.
        /// </summary>
        public KeyboardType TextEntryKeyboardType { get; set; }

        /// <summary>
        /// Gets or sets the text to display when a text entry control has no value.
        /// </summary>
        public string TextEntryPlaceholder { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlAttribute"/> class.
        /// </summary>
        /// <param name="type">The type of control that should be used to represent the member.</param>
        public ControlAttribute(ControlType type)
        {
            Type = type;
            FontName = Font.PreferredLabelFont.Name;
            FontSize = Font.PreferredLabelFont.Size.ToString();
            FontFormatting = Font.PreferredLabelFont.Formatting;
            SliderMaxValue = "100";
            SliderMinValue = "0";
            LabelLines = "1";
        }
    }

    /// <summary>
    /// Describes the available control types that can be used with reflective UI.
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// A control that can be clicked or tapped by the user.
        /// </summary>
        Button,
        /// <summary>
        /// A control that lets the user select from a range of date values.
        /// </summary>
        DatePicker,
        /// <summary>
        /// A read-only string of text.  If a <see cref="LabelAttribute"/> is also applied,
        /// this control will appear on the right; otherwise, it will appear on the left.
        /// </summary>
        Label,
        /// <summary>
        /// A control that can accept a single line of text input from the user.
        /// The input is masked so that it cannot be read, allowing for secured entry of sensitive information.
        /// </summary>
        PasswordBox,
        /// <summary>
        /// A control that lets the user make a single selection from a list of items.
        /// </summary>
        SelectList,
        /// <summary>
        /// A control that lets the user select from a range of numerical values by sliding a knob along a track.
        /// </summary>
        Slider,
        /// <summary>
        /// A control that holds a boolean value and can be toggled by the user.
        /// </summary>
        Switch,
        /// <summary>
        /// A control that can accept multiple lines of text input from the user.
        /// </summary>
        TextArea,
        /// <summary>
        /// A control that can accept a single line of text input from the user.
        /// </summary>
        TextBox,
        /// <summary>
        /// A control that lets the user select from a range of time values.
        /// </summary>
        TimePicker
    }
}

