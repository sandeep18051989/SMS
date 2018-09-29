using EF.Core.Data;
using System;
using EF.Services;

namespace SMS.Models
{
    public partial class VideoModel : BaseEntityModel
    {
        public string Url { get; set; }
        public decimal Size { get; set; }
        public bool IsActive { get; set; }

    }
}