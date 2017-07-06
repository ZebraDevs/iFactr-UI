using System.Collections.Generic;
using System.Collections;

namespace iFactr.Core.Targets.Config
{
    /// <summary>
    /// Defines an application's configuration settings.
    /// </summary>
    public interface IConfig : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, ICollection, IEnumerable /*, ISerializable, IDeserializationCallback */
    {
        /// <summary>
        /// Loads the application's configuration settings.
        /// </summary>
        void Load();
    }
}
