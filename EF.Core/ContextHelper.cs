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

        public static void Replace(IEngine engine)
        {
            SinglePattern<IEngine>.Instance = engine;
        }
        
        #endregion

        #region Properties

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
