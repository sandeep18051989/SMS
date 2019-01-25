using EF.Core;
using EF.Core.Social;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace EF.Services.Social
{
    public partial interface ISocialSettingService
    {
        IDictionary<string, IList<SocialSetting>> GetAllSettingsCached();

        void InsertSocialSetting(SocialSetting setting);

        void UpdateSocialSetting(SocialSetting setting);

        void DeleteSocialSetting(SocialSetting setting);

        void DeleteSocialSettings(IList<SocialSetting> settings);

        SocialSetting GetSocialSettingById(int settingId);

        SocialSetting GetSocialSetting(string key);

        T GetSocialSettingByKey<T>(string key, T defaultValue = default(T));

        void SetSocialSetting<T>(string key, T value);

        IList<SocialSetting> GetAllSocialSettings();

        bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int storeId = 0)
            where T : ISettings, new();

        T LoadSetting<T>(int storeId = 0) where T : ISettings, new();

        void SaveSetting<T>(T settings, int storeId = 0) where T : ISettings, new();

        void SaveSocialSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            int storeId = 0, bool clearCache = true) where T : ISettings, new();

        void DeleteSocialSetting<T>() where T : ISettings, new();

        void DeleteSocialSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, int storeId = 0) where T : ISettings, new();
    }
}
