using EF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public partial class ClassRoomDivisionModel : BaseEntityModel
    {
        public int DivisionId { get; set; }
        public int ClassId { get; set; }
        public int ClassRoomId { get; set; }

        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }
    }
}