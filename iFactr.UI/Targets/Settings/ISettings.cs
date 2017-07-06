using System.Collections.Generic;
using System.Collections;

namespace iFactr.Core.Targets.Settings
{
    /// <summary>
    /// Defines an application's settings.
    /// </summary>
    public interface ISettings : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, ICollection, IEnumerable /*, ISerializable, IDeserializationCallback */
    {
        /// <summary>
        /// Loads an application's settings.
        /// </summary>
        void Load();

        /// <summary>
        /// Saves an application's settings to disk.
        /// </summary>
        void Store();
    }
}
