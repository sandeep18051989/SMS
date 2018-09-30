using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Services
{
    public partial class ConfirmationForDeleteModel : BaseEntityModel
    {
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public string WindowId { get; set; }
    }
}
