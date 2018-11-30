using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF.Core.Enums;
using EF.Services;
using FluentValidation.Attributes;
using SMS.Validations;

namespace SMS.Models
{
    [Validator(typeof(SliderValidation))]
    public partial class SliderModel : BaseEntityModel
    {
        public SliderModel()
        {
            Pictures = new List<PictureModel>();
            AvailableAreas = new List<SelectListItem>();
        }
        public bool IsActive { get; set; }

        public string Name { get; set; }

        public bool ShowNextPrevIndicators { get; set; }

        public bool ShowCaption { get; set; }

        public int DisplayArea { get; set; }

        public string DisplayAreaString { get; set; }

        [UIHint("Picture")]
        public int PictureId { get; set; }

        public int? DisplayOrder { get; set; }

        public int? MaxPictures { get; set; }

        public bool ShowThumbnails { get; set; }
        public IList<PictureModel> Pictures { get; set; }
        public IList<SelectListItem> AvailableAreas { get; set; }

    }
}