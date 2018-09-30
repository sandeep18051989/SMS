using EF.Core.Data;
using EF.Core.Enums;
using System.Collections.Generic;

namespace EF.Services.Service
{
	public interface ISettingService
	{
		void Insert(Settings setting);
		void Update(Settings setting);
		Settings GetSettingById(int settingId);
		IList<Settings> GetSettings();
		IList<Settings> GetSettingsByType(SettingTypeEnum settingType);
		IList<Settings> GetSettingsByEntityId(int entityId);
		Settings GetSettingByKey(string key, int userid = 0);

	}
}
