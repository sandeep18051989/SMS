using EF.Services;
using System;

namespace SMS.Models
{
    public partial class QuestionTypeModel : BaseEntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemDefined { get; set; }
    }
}