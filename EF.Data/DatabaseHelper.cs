using System;

namespace EF.Data
{
    public partial class DatabaseHelper
    {
        private static bool? _databaseIsInstalled;

        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                var manager = new DataSettingsContext();
                var settings = manager.LoadSettings();
                _databaseIsInstalled = settings != null && !String.IsNullOrEmpty(settings.DataConnectionString);
            }
            return _databaseIsInstalled.Value;
        }

        //Reset information cached in the "DatabaseIsInstalled" method
        public static void ResetCache()
        {
            _databaseIsInstalled = null;
        }
    }
}
