using System.Collections.Generic;
using System.Text;

namespace iFactr.Core.Layers
{
    /// <summary>
    /// Represents broken validation rules for a <see cref="iFactr.Core.Forms.Field"/>.
    /// </summary>
    public class ValidationInfo
    {
        /// <summary>
        /// Gets a string representation of the original value of the field prior to validation.
        /// </summary>
        public string OriginalFieldValue { get; private set; }

        /// <summary>
        /// Gets a list of validation rules that the value of the field has broken.
        /// </summary>
        public List<string> BrokenRulesPerField { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationInfo"/> class.
        /// </summary>
        /// <param name="originalFieldValue">A string representation of the original value of the field.</param>
        /// <param name="brokenRulesPerField">A list of validation rules that the value of the field has broken.</param>
        public ValidationInfo(string originalFieldValue, List<string> brokenRulesPerField)
        {
            OriginalFieldValue = originalFieldValue;
            BrokenRulesPerField = brokenRulesPerField;
        }

        /// <summary>
        /// Gets the field errors stored in <see cref="BrokenRulesPerField"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> representing the broken rules.</returns>
        public string GetFieldErrors()
        {
            var sb = new StringBuilder();
            foreach (var item in BrokenRulesPerField)
            {
                sb.Append(item);
                sb.Append("; ");
            }
            return sb.ToString().TrimEnd(' ', ';');
        }
    }
}