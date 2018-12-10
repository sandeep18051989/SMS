using System.Collections.Generic;
using EF.Services;

namespace SMS.Models
{
	public partial class RepliesModel : BaseEntityModel
	{
		public RepliesModel()
		{
			User = new UserModel();
			Reactions = new List<ReactionModel>();
		}
		public string ReplyHtml { get; set; }
		public int DisplayOrder { get; set; }
		public bool IsModified { get; set; }

		public string Username { get; set; }
		public UserModel User { get; set; }

		public string Type { get; set; }
        public string CreatedOnString { get; set; }
        public string ModifiedOnString { get; set; }

        public IList<ReactionModel> Reactions { get; set; }

	}
}