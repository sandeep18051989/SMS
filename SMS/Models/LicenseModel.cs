using EF.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Models
{
    public partial class LicenseModel
    {
        public LicenseModel()
        {
            AvailableStatuses = new List<SelectListItem>();
        }
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

        public IList<SelectListItem> AvailableStatuses { get; set; }

    }
}