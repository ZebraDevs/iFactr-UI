using iFactr.Core.Targets;
using iFactr.Core.Targets.Config;
using iFactr.Core.Targets.Settings;
using MonoCross.Utilities;
using MonoCross.Utilities.ImageComposition;
using System;

/*
This namespace contains the classes representating the abstract Native (i.e. Single User/Single Tenant) Factory
that is implemented for each Native platform.
representation of an iApp and iLayers into the platform and target-specific user
experience.
*/
namespace iFactr.Core.Native
{
    /// <summary>
    /// Represents a binding factory for native targets.  This class is abstract.
    /// <para></para>
    /// </summary>
    /// <remarks>
    /// <para><img src="Diagrams\NativeFactory.cd"/></para></remarks>
    public abstract class NativeFactory : TargetFactory, ITargetFactory
    {
        #region ctor/abstract singleton initializers

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeFactory"/> class.
        /// </summary>
        protected NativeFactory()
        {

        }

        #endregion

        #region ITargetFactory Members

        /// <summary>
        /// Gets the path for temporary application data.  The contents of this path are emptied on application startup.
        /// </summary>
        /// <value>The temporary data path as a <see cref="String"/> instance.</value>
        public override string TempPath
        {
            get
            {
                return SessionDataPath.AppendPath("Temp");
            }
        }

        /// <summary>
        /// Gets the application configuration settings.
        /// </summary>
        public override IConfig Config
        {
            get { return _config ?? (_config = new ConfigDictionary()); }
        }

        /// <summary>
        /// The application configuration settings.
        /// </summary>
        protected ConfigDictionary _config;

        /// <summary>
        /// The application's image compositor.
        /// </summary>
        protected ICompositor _compositor;

        /// <summary>
        /// Gets the current application settings.
        /// </summary>
        public override ISettings Settings
        {
            get { return _settings ?? (_settings = new SettingsDictionary()); }
        }

        /// <summary>
        /// The current application settings.
        /// </summary>
        protected SettingsDictionary _settings;

        #endregion
    }
}