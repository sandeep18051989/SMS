using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Areas.Admin.Models
{
    public class AdminPictureModel
    {
        public string PictureSrc { get; set; }
        public string Url { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Size { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsThumb { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsLogo { get; set; }
        public string AlternateText { get; set; }
    }
}