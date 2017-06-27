using System;
using System.Collections;
using System.Linq;
using MonoCross.Utilities;

namespace iFactr.Core
{
    /// <summary>
    /// Defines methods used internally by the framework to check parameter validity.
    /// </summary>
    public static class Parameter
    {
        /// <summary>
        /// Checks if the specified object is an instance of any of the expected types.
        /// </summary>
        /// <param name="parameter">The object to check.</param>
        /// <param name="parameterName">The name of the parameter that the object represents.</param>
        /// <param name="expectedTypes">The types that the object is allowed to be.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parameter"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parameter"/> is not an instance of any of the <paramref name="expectedTypes"/>.</exception>
        public static void CheckParameterType(object parameter, string parameterName, params Type[] expectedTypes)
        {
            CheckParameterType(parameter, parameterName, false, expectedTypes);
        }

        /// <summary>
        /// Checks if the specified object is an instance of any of the expected types.
        /// </summary>
        /// <param name="parameter">The object to check.</param>
        /// <param name="parameterName">The name of the parameter that the object represents.</param>
        /// <param name="allowNull">Whether or not the object can be null.</param>
        /// <param name="expectedTypes">The types that the object is allowed to be.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="parameter"/> is <c>null</c> and <paramref name="allowNull"/> is <c>false</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parameter"/> is not an instance of any of the <paramref name="expectedTypes"/>.</exception>
        public static void CheckParameterType(object parameter, string parameterName, bool allowNull, params Type[] expectedTypes)
        {
            if (parameter == null)
            {
                if (!allowNull)
                {
                    throw new ArgumentNullException(parameterName);
                }
                return;
            }

            string typeNames = string.Empty;
            foreach (var expectedType in expectedTypes)
            {
                if (Device.Reflector.IsSubclassOf(parameter.GetType(), expectedType))
                {
                    return;
                }

                typeNames += expectedType.Name + ", ";
            }

            if (typeNames.Length > 1)
                typeNames = typeNames.Remove(typeNames.Length - 2, 2);
            throw new ArgumentException(string.Format("{0} must be one of the following types: {1}.", parameterName, typeNames));
        }

        /// <summary>
        /// Checks if all objects in the specified collection are instances of any of the expected types.
        /// </summary>
        /// <param name="collection">The collection in which to check each object.</param>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="expectedTypes">The types that each object is allowed to be.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when an object in the collection is not an instance of any of the <paramref name="expectedTypes"/>.</exception>
        public static void CheckParameterTypes(IEnumerable collection, string collectionName, params Type[] expectedTypes)
        {
            CheckParameterTypes(collection, collectionName, false, expectedTypes);
        }

        /// <summary>
        /// Checks if all objects in the specified collection are instances of any of the expected types.
        /// </summary>
        /// <param name="collection">The collection in which to check each object.</param>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="allowNull">Whether or not the objects in the collection can be null.</param>
        /// <param name="expectedTypes">The types that each object is allowed to be.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when an object in the collection is not an instance of any of the <paramref name="expectedTypes"/>.</exception>
        public static void CheckParameterTypes(IEnumerable collection, string collectionName, bool allowNull, params Type[] expectedTypes)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(collectionName);
            }

            var typeNames = string.Join(", ", expectedTypes.Select(t => t.Name).ToArray());

            foreach (var parameter in collection)
            {
                if (parameter == null)
                {
                    if (!allowNull)
                    {
                        throw new ArgumentException("Collection contains a null element.  Elements in the collection must not be null.", collectionName);
                    }
                    continue;
                }

                if (!expectedTypes.Any(expectedType => Device.Reflector.IsSubclassOf(parameter.GetType(), expectedType)))
                {
                    throw new ArgumentException(string.Format("Collection contains one or more elements of an invalid type.  All elements must derive from one of the following types: {0}.", typeNames), collectionName);
                }
            }
        }

        /// <summary>
        /// Checks if the specified object resides within the specified collection.
        /// </summary>
        /// <param name="collection">The collection in which to check for the object.</param>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="parameter">The object to check.</param>
        /// <param name="parameterName">The name of the parameter that the object represents.</param>
        /// <returns>The index of the object within the collection.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="collection"/> is <c>null</c> -or- when the <paramref name="parameter"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="parameter"/> is not found within the <paramref name="collection"/>.</exception>
        public static int CheckObjectExists(IEnumerable collection, string collectionName, object parameter, string parameterName)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(collectionName);
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            int collectionIndex = -1;
            int objectIndex = -1;
            foreach (var entry in collection)
            {
                collectionIndex++;
                if (entry.Equals(parameter))
                {
                    objectIndex = collectionIndex;
                }
            }

            if (objectIndex < 0)
            {
                throw new ArgumentException(string.Format("{0} does not exist in the {1} collection.", parameterName, collectionName));
            }

            return objectIndex;
        }

        /// <summary>
        /// Checks if the specified index is within range of the specified collection.
        /// </summary>
        /// <param name="collection">The collection that the index is to be checked against.</param>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="index">The index to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> exceeds the upper or lower bound of the <paramref name="collection"/>.</exception>
        public static void CheckIndex(IEnumerable collection, string collectionName, int index)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(collectionName);
            }

            if (index < 0 || index >= collection.Count())
            {
                throw new ArgumentOutOfRangeException(string.Format("Index must be greater than or equal to 0 and less than the total number of entries in the {0} collection.", collectionName));
            }
        }

        /// <summary>
        /// Checks if the specified url is valid.
        /// </summary>
        /// <param name="url">The url to check.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="url"/> is <c>null</c> or an empty string.</exception>
        public static void CheckUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", "url");
            }
        }
    }
}