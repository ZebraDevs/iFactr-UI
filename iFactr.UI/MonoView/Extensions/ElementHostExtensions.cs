using System;
using System.Linq;
using iFactr.Core;
using MonoCross.Navigation;

namespace iFactr.UI.Controls
{
    /// <summary>
    /// Provides methods for <see cref="IElementHost"/> objects.
    /// </summary>
    public static class ElementHostExtensions
    {
        /// <summary>
        /// Returns the first element found with the specified ID.
        /// </summary>
        /// <param name="host">The <see cref="IElementHost"/> object.</param>
        /// <param name="id">The identifier of the element.</param>
        /// <returns>The first element found with the specified ID -or- <c>null</c> if no element was found.</returns>
        public static IElement GetChild(this IElementHost host, string id)
        {
            return host.Children.FirstOrDefault(c => c.ID == id);
        }

        /// <summary>
        /// Returns the first element found with the specified ID that is of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="host">The <see cref="IElementHost"/> object.</param>
        /// <param name="id">The identifier of the element.</param>
        /// <returns>The first element found with the specified ID that is of type <typeparamref name="T"/> -or-
        /// <c>null</c> if no element was found.</returns>
        public static T GetChild<T>(this IElementHost host, string id)
            where T : IElement
        {
            return (T)host.Children.FirstOrDefault(c => c is T && c.ID == id);
        }

        /// <summary>
        /// Returns the first element found that is of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        /// <param name="host">The <see cref="IElementHost"/> object.</param>
        /// <returns>The first element found of type <typeparamref name="T"/> -or-
        /// <c>null</c> if no element of the specified type was found.</returns>
        public static T GetChild<T>(this IElementHost host)
            where T : IElement
        {
            return (T)host.Children.FirstOrDefault(c => c is T);
        }

        /// <summary>
        /// Returns the first element found with the specified ID that is of type <typeparamref name="T"/>.
        /// If no element is found, a new instance will be created with the specified ID and added to the parent.
        /// </summary>
        /// <typeparam name="T">The type of the element to get or create.</typeparam>
        /// <param name="host">The <see cref="IElementHost"/> object.</param>
        /// <param name="id">The identifier of the element.  If a new element is created, its ID will be set to this value.</param>
        /// <returns>The first element found of type <typeparamref name="T"/> -or-
        /// the newly created element if no element was found.</returns>
        /// <remarks>When creating a new instance, if <typeparamref name="T"/> is an interface type, the framework will use the
        /// <see cref="M:MXContainer.Resolve"/> method; otherwise, the framework will use the <see cref="M:Activator.CreateInstance"/> method.</remarks>
        public static T GetOrCreateChild<T>(this IElementHost host, string id)
            where T : IElement
        {
            var child = host.Children.FirstOrDefault(c => c is T && c.ID == id);
            if (child == null)
            {
                if (MonoCross.Utilities.Device.Reflector.IsInterface(typeof(T)))
                {
                    child = MXContainer.Resolve<T>();
                }
                else
                {
                    try
                    {
                        child = Activator.CreateInstance<T>();
                    }
                    catch (MissingMemberException)
                    {
                        iApp.Log.Error("Unable to find a default constructor for type " + typeof(T).Name);
                    }
                }

                if (child == null)
                {
                    iApp.Log.Error("Could not create an instance of element type " + typeof(T).Name);
                }
                else
                {
                    child.ID = id;
                    host.AddChild(child);
                }
            }

            return (T)child;
        }
    }
}