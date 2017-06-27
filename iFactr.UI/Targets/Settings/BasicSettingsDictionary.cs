using MonoCross.Utilities;
using MonoCross.Utilities.Serialization;
using System.Collections.Generic;

namespace iFactr.Core.Targets.Settings
{
    /// <summary>
    /// Stores and retrieves a user settings file.
    /// </summary>
    public class BasicSettingsDictionary : SettingsDictionary
    {
        private readonly string _settingsPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSettingsDictionary"/> class.
        /// </summary>
        public BasicSettingsDictionary()
        {
            // Use Session Settings
            _settingsPath = Device.SessionDataPath.AppendPath("AppSettings.xml");
        }

        /// <summary>
        /// Loads the user settings from disk.
        /// </summary>
        public override void Load()
        {
            if (!Device.File.Exists(_settingsPath)) return;

            byte[] settingsBytes = Device.File.Read(_settingsPath);

            var serializer = new SerializerXml<BasicSettingsDictionary>();
            BasicSettingsDictionary storedSettings = serializer.DeserializeObject(settingsBytes);
            if (storedSettings != null)
            {
                Clear();
                foreach (KeyValuePair<string, string> kvp in storedSettings)
                {
                    Add(kvp);
                }
            }
            else
            {
                Device.Log.Warn("Failed to load Settings from : " + _settingsPath);
            }
        }

        /// <summary>
        /// Saves the application's settings to disk.
        /// </summary>
        public override void Store()
        {
            var serializer = new SerializerXml<BasicSettingsDictionary>();
            serializer.SerializeObjectToFile(this, _settingsPath);
        }
    }
}