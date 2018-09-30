using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;

namespace EF.Services.Service
{
	public class SettingService : ISettingService
	{
		#region Fields

		public readonly IRepository<Settings> _settingRepository;

		#endregion

		#region Const

		public SettingService(IRepository<Settings> settingRepository)
		{
			this._settingRepository = settingRepository;
		}
		#endregion


		#region ISetting Members

		public void Insert(Settings setting)
		{
			_settingRepository.Insert(setting);
		}

		public void Update(Settings setting)
		{
			_settingRepository.Update(setting);
		}

		#endregion

		#region Utilities

		public Settings GetSettingById(int settingId)
		{
			if (settingId > 0)
			{
				var setting = from c in _settingRepository.Table
								  orderby c.Id
								  where c.Id == settingId
								  select c;
				var query = setting.FirstOrDefault();
				return query;
			}
			else
			{
				return null;
			}
		}
		public IList<Settings> GetSettings()
		{
			return _settingRepository.Table.Distinct().ToList();
		}
		public IList<Settings> GetSettingsByType(SettingTypeEnum settingType)
		{
			var settings = (from c in _settingRepository.Table
								 orderby c.Id
								 where c.SettingType == (int)settingType
								 select c).ToList();
			return settings;
		}
		public IList<Settings> GetSettingsByEntityId(int entityId)
		{
			if (entityId > 0)
			{
				var settings = (from c in _settingRepository.Table
									 orderby c.Id
									 where c.EntityId == entityId
									 select c).ToList();
				return settings;
			}
			else
			{
				return null;
			}
		}
		public Settings GetSettingByKey(string key, int userid = 0)
		{
			if (string.IsNullOrEmpty(key))
				throw new System.Exception("Setting key missing.");

			var query = (from setting in _settingRepository.TableNoTracking
							 where setting.Name.Trim().ToLower() == key.Trim().ToLower() && (userid > 0 ? setting.UserId == userid : true)
							 select setting).FirstOrDefault();

			return query;
		}
		#endregion
	}
}
