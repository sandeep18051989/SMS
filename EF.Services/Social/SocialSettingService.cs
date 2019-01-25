using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EF.Core.Social;
using EF.Core;

namespace EF.Services.Social
{
    public partial class SocialSettingService : ISocialSettingService
    {
        #region Constants

        private const string SETTINGS_ALL_KEY = "Ef.setting.all";
        private const string SETTINGS_PATTERN_KEY = "Ef.setting.";

        #endregion

        #region Fields

        private readonly IRepository<SocialSetting> _settingRepository;

        #endregion

        #region Ctor
        public SocialSettingService(IRepository<SocialSetting> settingRepository)
        {
            this._settingRepository = settingRepository;
        }

        #endregion

        #region Methods

        public virtual IDictionary<string, IList<SocialSetting>> GetAllSettingsCached()
        {
            string key = string.Format(SETTINGS_ALL_KEY);
            var query = from s in _settingRepository.TableNoTracking
                        orderby s.Name
                        select s;
            var settings = query.ToList();
            var dictionary = new Dictionary<string, IList<SocialSetting>>();
            foreach (var s in settings)
            {
                var resourceName = s.Name.ToLowerInvariant();
                var settingForCaching = new SocialSetting
                {
                    Id = s.Id,
                    Name = s.Name,
                    Value = s.Value
                };
                if (!dictionary.ContainsKey(resourceName))
                {
                    //first setting
                    dictionary.Add(resourceName, new List<SocialSetting>
                        {
                            settingForCaching
                        });
                }
                else
                {
                    //already added
                    //most probably it's the setting with the same name but for some certain store (storeId > 0)
                    dictionary[resourceName].Add(settingForCaching);
                }
            }
            return dictionary;
        }

        public virtual void InsertSocialSetting(SocialSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("social setting");

            _settingRepository.Insert(setting);
        }

        public virtual void UpdateSocialSetting(SocialSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Update(setting);
        }

        public virtual void DeleteSocialSetting(SocialSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Delete(setting);
        }

        public virtual void DeleteSocialSettings(IList<SocialSetting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _settingRepository.Delete(settings);

        }

        public virtual SocialSetting GetSocialSettingById(int settingId)
        {
            if (settingId == 0)
                return null;

            return _settingRepository.GetByID(settingId);
        }

        public virtual SocialSetting GetSocialSetting(string key)
        {
            if (String.IsNullOrEmpty(key))
                return null;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault(x => x.Name.Trim().ToLower() == key.Trim().ToLower());
                if (setting != null)
                    return GetSocialSettingById(setting.Id);
            }

            return null;
        }

        public virtual T GetSocialSettingByKey<T>(string key, T defaultValue = default(T))
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault(x => x.Name.Trim().ToLower() == key.Trim().ToLower());
                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        public virtual void SetSocialSetting<T>(string key, T value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            key = key.Trim().ToLowerInvariant();
            string valueStr = TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;

            if (settingForCaching != null)
            {
                var setting = GetSocialSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSocialSetting(setting);
            }
            else
            {
                //insert
                var setting = new SocialSetting
                {
                    Name = key,
                    Value = valueStr
                };
                InsertSocialSetting(setting);
            }
        }

        public virtual IList<SocialSetting> GetAllSocialSettings()
        {
            var query = from s in _settingRepository.Table
                        orderby s.Name
                        select s;
            var settings = query.ToList();
            return settings;
        }

        public virtual bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int storeId = 0)
            where T : ISettings, new()
        {
            string key = settings.GetSettingKey(keySelector);

            var setting = GetSocialSettingByKey<string>(key);
            return setting != null;
        }

        public virtual T LoadSetting<T>(int storeId = 0) where T : ISettings, new()
        {
            var settings = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                //load by store
                var setting = GetSocialSettingByKey<string>(key);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                object value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings;
        }

        public virtual void SaveSetting<T>(T settings, int storeId = 0) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                string key = typeof(T).Name + "." + prop.Name;
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(settings, null);
                if (value != null)
                    SetSocialSetting(key, value);
                else
                    SetSocialSetting(key, "");
            }
        }

        public virtual void SaveSocialSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            int storeId = 0, bool clearCache = true) where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            string key = settings.GetSettingKey(keySelector);
            dynamic value = propInfo.GetValue(settings, null);
            if (value != null)
                SetSocialSetting(key, value);
            else
                SetSocialSetting(key, "");
        }

        public virtual void DeleteSocialSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<SocialSetting>();
            var allSettings = GetAllSocialSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                string key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            DeleteSocialSettings(settingsToDelete);
        }

        public virtual void DeleteSocialSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int storeId = 0) where T : ISettings, new()
        {
            string key = settings.GetSettingKey(keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                var setting = GetSocialSettingById(settingForCaching.Id);
                DeleteSocialSetting(setting);
            }
        }

        #endregion
    }
}