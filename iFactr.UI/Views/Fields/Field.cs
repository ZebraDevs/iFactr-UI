using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace iFactr.Core.Forms
{
    /// <summary>
    /// Represents a base class for all field types.  This class is abstract.
    /// </summary>
    public abstract class Field
    {
        /// <summary>
        /// Represents the method that will handle validation of a field.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="errMsg">The error message to display if validation fails.</param>
        /// <param name="args">Any additional arguments.</param>
        public delegate void ValidateDelegate(string value, string errMsg, params object[] args);

        #region Properties
        /// <summary>
        /// Gets or sets whether this instance is in focus.
        /// </summary>
        /// <value><c>true</c> if in focus; otherwise <c>false</c>.</value>
        public bool Focus { get; set; }
        /// <summary>
        /// Gets or sets the label text to display.
        /// </summary>
        /// <value>The label as a <see cref="String"/> value.</value>
        public virtual string Label { get; set; }
        /// <summary>
        /// Gets or sets the ID of this instance, which is used as the key when inserting values into a parameters dictionary.
        /// </summary>
        /// <value>The ID as a <see cref="String"/> value.</value>
        public string ID { get; set; }
        /// <summary>
        /// Gets or sets the placeholder text for this instance.
        /// </summary>
        /// <value>The placeholder text as a <see cref="String"/> value.</value>
        public string Placeholder { get; set; }
        /// <summary>
        /// Gets or sets the value text for this instance.
        /// </summary>
        /// <value>The text as a <see cref="String"/> value.</value>
        public virtual string Text { get; set; }
        /// <summary>
        /// Gets or sets the delegate to invoke when performing validation.
        /// </summary>
        /// <seealso href="http://support.ifactr.com/kb/forms/using-iformlayer-validation"/>
        [XmlIgnore]
        public ValidateDelegate Validate { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        public Field()
        {
            Placeholder = string.Empty;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Gets whether this instance has passed validation.
        /// </summary>
        public bool IsValid { get { return (BrokenRules.Count == 0); } }

        /// <summary>
        /// Gets a collection of error messages corresponding to all of the broken validation rules attached to this instance.
        /// </summary>
        public List<string> BrokenRules
        {
            get { return _brokenRules ?? (_brokenRules = new List<string>()); }
        }
        private List<string> _brokenRules;

        /// <summary>
        /// Clears out any broken rules that this instance currently contains.
        /// </summary>
        public void ClearBrokenRules()
        {
            BrokenRules.Clear();
        }


        // -- VALIDATION RULES LIBRARY --
        /// <summary>
        /// A validation rule that will fail if the specified value is null or empty.
        /// </summary>
        /// <param name="value">The text to check.</param>
        /// <param name="errMsg">The error message to display if there is no text.</param>
        public void TextIsRequired(string value, string errMsg)
        {
            // if we're null or empty we don't pass here...
            if (string.IsNullOrEmpty(value))
                BrokenRules.Add(errMsg);
        }

        /// <summary>
        /// A validation rule that will fail if the specified value is longer than the specified maximum.
        /// </summary>
        /// <param name="value">The text to check.</param>
        /// <param name="errMsg">The error message to display if the text is too long.</param>
        /// <param name="args">An <see cref="int"/> specifying the maximum length.</param>
        public void TextMaxLength(string value, string errMsg, params object[] args)
        {
            // if we're null or empty we do pass here.
            // value length needs to be a max of (int)args[0])
            if ((!string.IsNullOrEmpty(value)) && ((value.Length > (int)args[0])))
                BrokenRules.Add(errMsg);
        }

        /// <summary>
        /// A validation rule that will fail if the specified value is shorter than the specified minimum.
        /// </summary>
        /// <param name="value">The text to check.</param>
        /// <param name="errMsg">The error message to display if the text is too short.</param>
        /// <param name="args">An <see cref="int"/> specifying the minimum length.</param>
        public void TextMinLength(string value, string errMsg, params object[] args)
        {
            // if we're null or empty we don't pass here.
            // value length needs to be a min of (int)args[0])
            if ((string.IsNullOrEmpty(value)) || ((value.Length < (int)args[0])))
                BrokenRules.Add(errMsg);
        }

        /// <summary>
        /// A validation rule that will fail if the specified value is not in the format of an email address.
        /// </summary>
        /// <param name="value">The text to check.</param>
        /// <param name="errMsg">The error message to display if the text is not a valid email address.</param>
        public void ValidEmail(string value, string errMsg)
        {
            // if we're null or empty we don't pass here.
            if (string.IsNullOrEmpty(value))
            {
                BrokenRules.Add(errMsg);
                return;
            }
            var strIn = value;

            // value must be in valid e-mail format.
            if (!Regex.IsMatch(strIn,
              @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))
                BrokenRules.Add(errMsg);
        }
        #endregion

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public Field Clone()
        {
            return (Field)CloneOverride();
        }

        /// <summary>
        /// Creates a deep-copy clone of this instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        protected virtual object CloneOverride()
        {
            return MemberwiseClone();
        }
    }
}