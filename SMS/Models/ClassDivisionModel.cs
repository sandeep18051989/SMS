using EF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public partial class ClassDivisionModel : BaseEntityModel
    {
        public int DivisionId { get; set; }
        public int ClassId { get; set; }
        public int ClassRoomId { get; set; }
    }
}