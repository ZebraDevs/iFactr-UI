using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iFactr.UI
{
    /// <summary>
    /// Defines an object that converts a value from one type to another.
    /// </summary>
    public interface IValueConverter
    {
        /// <summary>
        /// Converts the specified value to the specified type and returns the result.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType">The type that the value is expected to be.</param>
        /// <param name="parameter">An optional parameter to assist with value conversion.</param>
        /// <returns>The converted value.</returns>
        object Convert(object value, Type targetType, object parameter);

        /// <summary>
        /// Converts the specified value to the specified type and returns the result.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <param name="targetType">The type that the value is expected to be.</param>
        /// <param name="parameter">An optional parameter to assist with value conversion.</param>
        /// <returns>The converted value.</returns>
        object ConvertBack(object value, Type targetType, object parameter);
    }
}
