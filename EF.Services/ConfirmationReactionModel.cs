using EF.Core.Data;
using System.Collections;
using System.Collections.Generic;

namespace EF.Services
{
    public partial class ConfirmationReactionModel : BaseEntityModel
    {
        public ConfirmationReactionModel()
        {
            Reactions = new List<Reaction>();
        }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string WindowId { get; set; }
        public bool IsAuthenticated { get; set; }
        public IList<Reaction> Reactions { get; set; }
    }
}
