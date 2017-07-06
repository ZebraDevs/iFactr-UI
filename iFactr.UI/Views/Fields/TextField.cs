using System;
using iFactr.UI;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a field for accepting text input.
    /// </summary>
    public class TextField : Field
    {
        #region Properties

        /// <summary>
        /// Gets or sets an optional Regex string for limiting what characters can be inputted.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance should mask its input.
        /// </summary>
        /// <value><c>true</c> if this instance should mask its input; otherwise, <c>false</c>.</value>
        public bool IsPassword
        {
            get { return _isPassword; }
            set
            {
                _isPassword = value;
                if (_isPassword) TextCompletion = TextCompletion.Disabled;
            }
        }
        private bool _isPassword;

        /// <summary>
        /// Gets or sets the virtual keyboard type for this instance.
        /// </summary>
        public virtual KeyboardType KeyboardType { get; set; }

        //public string Style { get; set; }
        /// <summary>
        /// Gets or sets the label style.
        /// </summary>
        /// <value>The label style as a <see cref="String"/> value.</value>
        public string LabelStyle { get; set; }

        /// <summary>
        /// Gets or sets the label subtext value.
        /// </summary>
        /// <value>The label subtext as a <see cref="String"/> value.</value>
        public string LabelSubtext { get; set; }

        /// <summary>
        /// Gets or sets text completion behavior for this instance.
        /// </summary>
        public virtual TextCompletion TextCompletion { get; set; }

        #endregion

        #region Constructorse

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class with no submission ID.
        /// </summary>
        public TextField() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class using the ID provided.
        /// </summary>
        /// <param name="id">A <see cref="String"/> representing the ID.</param>
        public TextField(string id)
        {
            ID = id;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        public new TextField Clone()
        {
            return (TextField)CloneOverride();
        }

        #endregion
    }
}