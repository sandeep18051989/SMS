using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
    public partial class User : BaseEntity
    {
        private ICollection<UserRole> _UserRoles;
        private ICollection<News> _News;
        private ICollection<SocialRecord> _SocialRecords;

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsApproved { get; set; }
        public Guid UserGuid { get; set; }
        public string SeoName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int? CityId { get; set; }
        public int? CoverPictureId { get; set; }
        public int? ProfilePictureId { get; set; }

        #region Navigation Properties
        public virtual ICollection<UserRole> Roles
        {
            get { return _UserRoles ?? (_UserRoles = new List<UserRole>()); }
            protected set { _UserRoles = value; }
        }
        public virtual ICollection<News> News
        {
            get { return _News ?? (_News = new List<News>()); }
            protected set { _News = value; }
        }

        public virtual ICollection<SocialRecord> SocialRecords
        {
            get { return _SocialRecords ?? (_SocialRecords = new List<SocialRecord>()); }
            protected set { _SocialRecords = value; }
        }
        #endregion

    }
}
