using EF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Models
{
    public partial class ClassRoomDivisionModel : BaseEntityModel
    {
        public ClassRoomDivisionModel()
        {
            AvailableClassRooms = new List<SelectListItem>();
        }
        public int DivisionId { get; set; }
        public int ClassId { get; set; }
        public int ClassRoomId { get; set; }
        public string Division { get; set; }
        public string Class { get; set; }
        public string ClassRoom { get; set; }

        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

        public IList<SelectListItem> AvailableClassRooms { get; set; }
    }

    public partial class AllotClassRoomsToClass
    {
        public AllotClassRoomsToClass()
        {
            AvailableDivisions = new List<DivisionModel>();
        }
        public int ClassId { get; set; }
        public string Class { get; set; }

        public IList<DivisionModel> AvailableDivisions { get; set; }
    }
}