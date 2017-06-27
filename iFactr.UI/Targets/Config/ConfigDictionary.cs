using MonoCross.Navigation;

namespace iFactr.Core.Targets.Config
{
    /// <summary>
    /// Represents a <see cref="SerializableDictionary&lt;TKey, TValue&gt;"/> that contains an application's configuration settings.
    /// </summary>
    public class ConfigDictionary : SerializableDictionary<string, string>, IConfig
    {
        /// <summary>
        /// Loads the application's configuration settings.
        /// </summary>
        public void Load()
        {
        }
    }
}