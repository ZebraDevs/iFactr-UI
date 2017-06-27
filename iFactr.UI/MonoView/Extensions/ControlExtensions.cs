namespace iFactr.UI.Controls
{
	/// <summary>
	/// Provides methods for <see cref="IControl"/> objects.
	/// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the control should be included when gathering submission values.
        /// </summary>
        public static bool ShouldSubmit(this IControl control)
        {
            return control != null && !string.IsNullOrEmpty(control.SubmitKey);
        }
    }
}

