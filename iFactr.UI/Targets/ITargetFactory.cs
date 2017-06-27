using iFactr.Core.Styles;
using iFactr.Core.Targets.Config;
using iFactr.Core.Targets.Settings;
using MonoCross;
using MonoCross.Navigation;
using MonoCross.Utilities.Encryption;
using MonoCross.Utilities.ImageComposition;
using MonoCross.Utilities.Logging;
using MonoCross.Utilities.Network;
using MonoCross.Utilities.Storage;
using MonoCross.Utilities.Threading;


namespace iFactr.Core.Targets
{
    /// <summary>
    /// Defines a bindings factory.
    /// </summary>
    /// <remarks>
    /// This interface is implemented in the target bindings by the factory to provide runtime values for properties used by an iFactr application.
    /// </remarks>
    public interface ITargetFactory
    {
        /// <summary>
        /// Gets whether the factory is running on a large form-factor device.
        /// </summary>
        bool LargeFormFactor
        {
            get;
        }

        /// <summary>
        /// Gets or sets the restrictions to impose on network GET methods.
        /// </summary>
        NetworkGetMethod NetworkGetMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the restrictions to impose on network POST methods.
        /// </summary>
        NetworkPostMethod NetworkPostMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the factory style.  If no application style is set, this style is used.
        /// </summary>
        Style Style
        {
            get;
        }

        /// <summary>
        /// Gets the platform that the factory is running on.
        /// </summary>
        MobilePlatform Platform
        {
            get;
        }

        /// <summary>
        /// Gets the target that the factory is running on.
        /// </summary>
        MobileTarget Target
        {
            get;
        }

        /// <summary>
        /// Gets the path for application assets.
        /// </summary>
        string ApplicationPath
        {
            get;
        }

        /// <summary>
        /// Gets the session-scoped path for application data.
        /// </summary>
        string SessionDataPath
        {
            get;
        }

        /// <summary>
        /// Gets the session-scoped root path for application data.
        /// </summary>
        string SessionDataRoot
        {
            get;
        }

        /// <summary>
        /// Gets the session-scoped appended path for application data.
        /// </summary>
        string SessionDataAppend
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the path for application data.
        /// </summary>
        string DataPath
        {
            get;
        }

        /// <summary>
        /// Gets the path for temporary application data.  The contents of this path is emptied on application startup.
        /// </summary>
        string TempPath
        {
            get;
        }

        /// <summary>
        /// Gets a unique identifier for the device running the application.
        /// </summary>
        string DeviceId
        {
            get;
        }

        /// <summary>
        /// Gets the application's configuration settings.
        /// </summary>
        IConfig Config
        {
            get;
        }

        /// <summary>
        /// Gets the application's settings.
        /// </summary>
        ISettings Settings
        {
            get;
        }

        /// <summary>
        /// Gets the application's session settings.
        /// </summary>
        ISession Session
        {
            get;
        }

        /// <summary>
        /// Gets the application's logging utility.
        /// </summary>
        ILog Logger
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the application's file system interface.
        /// </summary>
        IFile File
        {
            get;
        }

        /// <summary>
        /// Gets the application's threading utility.
        /// </summary>
        IThread Thread
        {
            get;
        }

        /// <summary>
        /// Gets the application's networking utility.
        /// </summary>
        INetwork Network
        {
            get;
        }

        /// <summary>
        /// Gets the application's data encryptor.
        /// </summary>
        IEncryption Encryption
        {
            get;
        }

        /// <summary>
        /// Gets the application's image compositor.
        /// </summary>
        ICompositor Compositor
        {
            get;
        }

        /// <summary>
        /// Outputs a layer from the specified network response.
        /// </summary>
        /// <param name="response">The network response from which to output a layer.</param>
        void OutputLayerResponse( NetworkResponse response );
    }

}
