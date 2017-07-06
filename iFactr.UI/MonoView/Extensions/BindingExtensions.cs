using iFactr.UI.Controls;
using MonoCross.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace iFactr.UI
{
    /// <summary>
    /// Provides methods for setting and clearing bindings.
    /// </summary>
    public static class BindingExtensions
    {
        private static WeakKeyDictionary<object, List<Binding>> bindingSet;

        static BindingExtensions()
        {
            bindingSet = new WeakKeyDictionary<object, List<Binding>>();
        }

        /// <summary>
        /// Sets the specified <see cref="Binding"/> instance for the object.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="binding">The <see cref="Binding"/> instance to be set.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="binding"/> is <c>null</c>.</exception>
        public static void SetBinding(this INotifyPropertyChanged obj, Binding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException("binding");
            }

            Binding current = null;

            var list = bindingSet.GetValueOrDefault(obj);
            if (list == null)
            {
                bindingSet[obj] = new List<Binding>();
            }
            else
            {
                current = list.FirstOrDefault(b => b.Equals(binding));
            }

            if (current == null)
            {
                binding.Target = obj;
                bindingSet[binding.Target].Add(binding);
                binding.Activate();
            }
            else
            {
                current.Mode = binding.Mode;
                current.ValueConverter = binding.ValueConverter;
                current.ValueConverterParameter = binding.ValueConverterParameter;
                current.Target = obj;
                current.Source = binding.Source;
            }
        }

        /// <summary>
        /// Sets the specified <see cref="Binding"/> instance for the object.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="binding">The <see cref="Binding"/> instance to be set.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="binding"/> is <c>null</c>.</exception>
        public static void SetBinding(this IPairable obj, Binding binding)
        {
            var notifier = obj as INotifyPropertyChanged ?? obj.Pair as INotifyPropertyChanged;
            if (notifier != null)
            {
                notifier.SetBinding(binding);
            }
        }

        /// <summary>
        /// Removes every binding where the object is either the target or the source.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void ClearAllBindings(this INotifyPropertyChanged obj)
        {
            var pairable = obj as IPairable;
            object pair = pairable == null ? null : pairable.Pair;

            foreach (var kvp in bindingSet)
            {
                var list = kvp.Value;
                if (list == null)
                {
                    continue;
                }

                if (kvp.Key == obj || kvp.Key == pair)
                {
                    foreach (var binding in list)
                    {
                        binding.Dispose();
                    }

                    list.Clear();
                }
                else
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        var binding = list[i];
                        if (binding.Source == obj || (binding.Source != null && binding.Source == pair))
                        {
                            binding.Dispose();
                            list.RemoveAt(i);
                        }
                    }
                }
            }

            bindingSet.Remove(obj);

            if (pair != null)
            {
                bindingSet.Remove(pair);
            }
        }

        /// <summary>
        /// Removes any bindings from the object that are currently targeting the property at the specified target path.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="targetPath">The path to the property that is to be cleared of any bindings.</param>
        public static void ClearBinding(this INotifyPropertyChanged obj, string targetPath)
        {
            var list = bindingSet.GetValueOrDefault(obj);
            if (list != null)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var binding = list[i];
                    if (binding.TargetPath == targetPath)
                    {
                        binding.Dispose();
                        list.Remove(binding);
                    }
                }

                if (list.Count == 0)
                {
                    bindingSet.Remove(obj);
                    var pairable = obj as IPairable;
                    if (pairable != null && pairable.Pair != null)
                    {
                        bindingSet.Remove(pairable.Pair);
                    }
                }
            }
        }

        /// <summary>
        /// Removes every binding where the object is either the target or the source.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void ClearAllBindings(this IPairable obj)
        {
            var notifier = obj as INotifyPropertyChanged ?? obj.Pair as INotifyPropertyChanged;
            if (notifier != null)
            {
                notifier.ClearAllBindings();
            }
        }

        /// <summary>
        /// Removes any bindings from the object that are currently targeting the property at the specified target path.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="targetPath">The path to the property that is to be cleared of any bindings.</param>
        public static void ClearBinding(this IPairable obj, string targetPath)
        {
            var notifier = obj as INotifyPropertyChanged ?? obj.Pair as INotifyPropertyChanged;
            if (notifier != null)
            {
                notifier.ClearBinding(targetPath);
            }
        }

        /// <summary>
        /// Recursively removes every binding where the object is either the target or the source.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void ClearAllBindings(this IElementHost obj)
        {
            var hosts = new Queue<IElementHost>(new[] { obj });
            while (hosts.Any())
            {
                var host = hosts.Dequeue();
                foreach (var child in host.Children)
                {
                    var subHost = child as IElementHost;
                    if (subHost != null) hosts.Enqueue(subHost);
                    else child.ClearAllBindings();
                }
                var pairable = host as IPairable;
                if (pairable != null) pairable.ClearAllBindings();
            }
        }
    }
}