using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public partial class SliderModel
    {
        public SliderModel()
        {
            Pictures = new List<PictureModel>();
            Setings = new SettingsModel();
        }
        public int SliderId { get; set; }
        public int MaxPictures { get; set; }
        public bool CaptionOff { get; set; }
        public IList<PictureModel> Pictures { get; set; }

        public SettingsModel Setings { get; set; }
    }
}