using MonoCross.Navigation;

namespace iFactr.Core.Targets.Settings
{
    /// <summary>
    /// Represents an application's settings.
    /// </summary>
    public class SettingsDictionary : SerializableDictionary<string, string>, ISettings
    {
        /// <summary>
        /// Loads the application's settings.
        /// </summary>
        public virtual void Load()
        {
            // base implementation is to do nothing, override functionality in specific platform as needed.
        }

        /// <summary>
        /// Saves the application's settings to disk.
        /// </summary>
        public virtual void Store( )
        {
            // do nothing.;  override functionality in specific platform as needed.
        }
    }
}