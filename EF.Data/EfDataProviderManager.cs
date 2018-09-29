using EF.Core;
using System;

namespace EF.Data
{
    public partial class EfDataProviderManager : DataManager
    {
        public EfDataProviderManager(DatabaseSettings settings):base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {

            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new Exception("Data Settings doesn't contain a providerName");

            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();
                default:
                    throw new Exception(string.Format("Not supported dataprovider name: {0}", providerName));
            }
        }

    }
}
