using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public partial class MenuModel
    {
        public MenuModel()
        {
            User = new UserModel();
        }

        public int MenuId{get; set;}
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int ParentMenuId { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public bool IsActive { get; set; }
    }
}