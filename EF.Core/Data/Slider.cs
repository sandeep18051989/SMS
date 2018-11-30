using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
    public partial class Slider : BaseEntity
    {
        [NotMapped]
        public virtual ICollection<Picture> _Pictures { get; set; }
        public bool IsActive { get; set; }

        public string Name { get; set; }

        public bool ShowNextPrevIndicators { get; set; }

        public bool ShowCaption { get; set; }

        public int DisplayArea { get; set; }

        public int DisplayOrder { get; set; }

        public int MaxPictures { get; set; }

        public bool ShowThumbnails { get; set; }

        public bool IsSystemDefined { get; set; }

        #region Navigation Properties
        public virtual ICollection<Picture> Pictures
        {
            get { return _Pictures ?? (_Pictures = new List<Picture>()); }
            protected set { _Pictures = value; }
        }
        #endregion

    }
}
