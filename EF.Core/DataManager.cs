using System;

namespace EF.Core
{
    /// <summary>
    /// Base data provider manager
    /// </summary>
    public abstract class DataManager
    {
        /// <summary>
        /// Ctor
        /// </summary>
        protected DataManager(DatabaseSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            this.Settings = settings;
        }

        /// <summary>
        /// Gets or sets settings
        /// </summary>
        protected DatabaseSettings Settings { get; private set; }

        /// <summary>
        /// Load data provider
        /// </summary>
        /// <returns>Data provider</returns>
        public abstract IDataProvider LoadDataProvider();

    }
}
