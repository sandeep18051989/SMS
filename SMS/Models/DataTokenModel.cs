using SMS.Areas.Admin.Validations;
using SMS.Validations;
using EF.Services;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Models
{
    public partial class DataTokenModel : BaseEntityModel
    {
        public string Name { get; set; }
        [AllowHtml]
        [UIHint("HtmlTemplate")]
        public string Value { get; set; }
        public string SystemName { get; set; }
        public bool IsSystemDefined { get; set; }
    }
}