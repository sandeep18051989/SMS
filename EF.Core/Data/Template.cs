using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
    public partial class Template : BaseEntity
    {
        [NotMapped]
        public virtual ICollection<DataToken> _Tokens { get; set; }
        public string Name { get; set; }
        public string BodyHtml { get; set; }
        public string Url { get; set; }
        public bool IsSystemDefined { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        #region Navigation Properties
        public virtual ICollection<DataToken> Tokens
        {
            get { return _Tokens ?? (_Tokens = new List<DataToken>()); }
            protected set { _Tokens = value; }
        }

        #endregion
    }
}
