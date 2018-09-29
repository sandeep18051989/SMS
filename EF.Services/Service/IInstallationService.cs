using EF.Core.Data;

namespace EF.Services.Service
{
	public interface IInstallationService
    {
        void InitConnectionFactory();
        void SetDatabaseInitializer();
        void InstallData(string AdminUsername, string AdminPassword, School school);
    }
}
