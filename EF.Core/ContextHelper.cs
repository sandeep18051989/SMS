using System.Configuration;
using System.Runtime.CompilerServices;
using EF.Core;

namespace EF.Core
{
    public class ContextHelper
    {
        #region Methods

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (SinglePattern<IEngine>.Instance == null || forceRecreate)
            {
                SinglePattern<IEngine>.Instance = new CMSEngine();

                var config = ConfigurationManager.GetSection("CMSConfig") as CMSConfig;
                SinglePattern<IEngine>.Instance.Initialize(config);
            }
            return SinglePattern<IEngine>.Instance;
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            SinglePattern<IEngine>.Instance = engine;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the SinglePattern Nop engine used to access Nop services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (SinglePattern<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return SinglePattern<IEngine>.Instance;
            }
        }

        #endregion
    }
}
