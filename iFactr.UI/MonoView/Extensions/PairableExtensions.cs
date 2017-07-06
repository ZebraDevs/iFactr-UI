using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iFactr.Core;
using MonoCross;
using MonoCross.Utilities;

namespace iFactr.UI
{
    /// <summary>
    /// Provides additional methods for <see cref="IPairable"/> objects.
    /// </summary>
    public static class PairableExtensions
    {
        /// <summary>
        /// Returns the value of the property or field with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name of the property or field whose value is to be returned.</param>
        public static object GetValue(this IPairable obj, string name)
        {
            return obj.GetValue(name, null);
        }
        
        /// <summary>
        /// Returns the value of the property or field with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name of the property or field whose value is to be returned.</param>
        /// <param name="index">Optional index values for indexed properties.  This value should be <c>null</c> for non-indexed properties.</param>
        public static object GetValue(this IPairable obj, string name, object[] index)
        {
            if (obj == null)
                return null;
            
            var type = obj.GetType();
            var property = Device.Reflector.GetProperty(type, name);
            if (property != null)
            {
                return property.GetValue(obj, index);
            }

            var field = Device.Reflector.GetField(type, name);
            if (field != null)
            {
                return field.GetValue(obj);
            }

            var pairable = obj as IPairable;
            if (pairable == null || pairable.Pair == null)
            {
                return null;
            }

            obj = pairable.Pair;
            type = obj.GetType();
            property = Device.Reflector.GetProperty(type, name);
            if (property != null)
            {
                return property.GetValue(obj, index);
            }

            field = Device.Reflector.GetField(type, name);
            if (field != null)
            {
                return field.GetValue(obj);
            }

            return null;
        }

        /// <summary>
        /// Sets the value of the property or field with the specified <paramref name="name"/> when running on one of the specified <see cref="MobileTarget"/>s.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name of the property or field whose value is to be set.</param>
        /// <param name="value">The value to set to the property or field.</param>
        /// <param name="targets">The targets on which the code must be running for the value to be set.</param>
        public static void SetValue(this IPairable obj, string name, object value, MobileTarget targets)
        {
            obj.SetValue(name, value, null, null, targets);
        }

        /// <summary>
        /// Sets the value of the property or field with the specified <paramref name="name"/> when running on one of the specified <see cref="MobileTarget"/>s.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name the property or field whose value is to be set.</param>
        /// <param name="value">The value to set to the property or field.</param>
        /// <param name="index">Optional index values for indexed properties.  This value should be <c>null</c> for non-indexed properties.</param>
        /// <param name="targets">The targets on which the code must be running for the value to be set.</param>
        public static void SetValue(this IPairable obj, string name, object value, object[] index, MobileTarget targets)
        {
            obj.SetValue(name, value, index, null, targets);
        }

        /// <summary>
        /// Sets the value of the property or field with the specified <paramref name="name"/> when running on one of the specified <see cref="MobileTarget"/>s.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name of the property or field whose value is to be set.</param>
        /// <param name="value">The value to set to the property or field.</param>
        /// <param name="converter">An optional converter to use on the value prior to setting the property or field.</param>
        /// <param name="targets">The targets on which the code must be running for the value to be set.</param>
        public static void SetValue(this IPairable obj, string name, object value, IValueConverter converter, MobileTarget targets)
        {
            obj.SetValue(name, value, null, converter, targets);
        }

        /// <summary>
        /// Sets the value of the property or field with the specified <paramref name="name"/> when running on one of the specified <see cref="MobileTarget"/>s.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="name">The name of the property or field whose value is to be set.</param>
        /// <param name="value">The value to set to the property or field.</param>
        /// <param name="index">Optional index values for indexed properties.  This value should be <c>null</c> for non-indexed properties.</param>
        /// <param name="converter">An optional converter to use on the value prior to setting the property or field.</param>
        /// <param name="targets">The targets on which the code must be running for the value to be set.</param>
        public static void SetValue(this IPairable obj, string name, object value, object[] index, IValueConverter converter, MobileTarget targets)
        {
            if (obj == null || !targets.HasFlag(iApp.Factory.Target))
                return;

            var type = obj.GetType();
            try
            {
                var property = Device.Reflector.GetProperty(type, name);
                if (property != null)
                {
                    if (converter != null)
                    {
                        value = converter.Convert(value, property.PropertyType, null);
                    }
                    else
                    {
                        value = Binding.GetConvertedValue(value, property.PropertyType);
                    }
                    property.SetValue(obj, value, index);
                    return;
                }

                var field = Device.Reflector.GetField(type, name);
                if (field != null)
                {
                    if (converter != null)
                    {
                        value = converter.Convert(value, field.FieldType, null);
                    }
                    else
                    {
                        value = Binding.GetConvertedValue(value, field.FieldType);
                    }
                    field.SetValue(obj, value);
                    return;
                }

                if (obj != null && obj.Pair != null)
                {
                    type = obj.Pair.GetType();
                    property = Device.Reflector.GetProperty(type, name);
                    if (property != null)
                    {
                        if (converter != null)
                        {
                            value = converter.Convert(value, property.PropertyType, null);
                        }
                        else
                        {
                            value = Binding.GetConvertedValue(value, property.PropertyType);
                        }
                        property.SetValue(obj.Pair, value, index);
                        return;
                    }

                    field = Device.Reflector.GetField(type, name);
                    if (field != null)
                    {
                        if (converter != null)
                        {
                            value = converter.Convert(value, field.FieldType, null);
                        }
                        else
                        {
                            value = Binding.GetConvertedValue(value, field.FieldType);
                        }
                        field.SetValue(obj.Pair, value);
                    }
                }
            }
            catch (Exception e)
            {
                iApp.Log.Warn("Unable to set value of member on {0}.", e, obj.ToString());
            }
        }
    }
}
