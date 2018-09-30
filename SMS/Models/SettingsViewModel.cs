using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMS.Models;

namespace SMS.Models
{
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            SliderSettings = new SliderSettingsModel();
            UserSettings = new UserSettingsModel();
            MenuSettings = new MenuSettingsModel();
            BlogSettings = new BlogSettingsModel();
            ColorSettings = new ColorSettingsModel();
            ConfigurationSettings = new ConfigurationSettingsModel();
            EmailSettings = new EmailSettingsModel();
            PictureSettings = new PictureSettingsModel();
        }

        public SliderSettingsModel SliderSettings { get; set; }

        public UserSettingsModel UserSettings { get; set; }
        public MenuSettingsModel MenuSettings { get; set; }
        public BlogSettingsModel BlogSettings { get; set; }

        public ColorSettingsModel ColorSettings { get; set; }

        public ConfigurationSettingsModel ConfigurationSettings { get; set; }

        public EmailSettingsModel EmailSettings { get; set; }

        public PictureSettingsModel PictureSettings { get; set; }

    }
}