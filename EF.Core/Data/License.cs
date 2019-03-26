using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;
using System;

namespace EF.Core.Data
{
	public partial class License : BaseEntity
	{
		public string EmailAddress { get; set; }
		public string Company { get; set; }
		public string LicenseKey { get; set; }
        public string LicenseUrl { get; set; }
        public int LicenseStatusId { get; set; }
        public DateTime? LicenseStartDate { get; set; }
        public DateTime? LicenseEndDate { get; set; }
        public string Name { get; set; }
        public bool IsPlugin { get; set; }
        public bool IsProduct { get; set; }

        [NotMapped]
		public LicenseStatus LicenseStatus
        {
			get
			{
				return (LicenseStatus)this.LicenseStatusId;
			}
			set
			{
				this.LicenseStatusId = (int)value;
			}
		}
	}
}
