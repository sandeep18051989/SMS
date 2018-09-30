using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Core.Data
{
	public partial class Reply : BaseEntity
	{
        [NotMapped]
        public virtual ICollection<Reaction> _Reactions { get; set; }
        public string ReplyHtml { get; set; }
		public int DisplayOrder { get; set; }
		public bool IsModified { get; set; }
		public string TeacherName { get; set; }
		public int TeacherId { get; set; }
		public int StudentId { get; set; }
		public string StudentName { get; set; }
		public int CommentId { get; set; }
		public virtual Comment Comment { get; set; }
		public virtual Teacher Teacher { get; set; }
		public virtual Student Student { get; set; }
		public virtual User User { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public virtual ICollection<Reaction> Reactions
        {
            get { return _Reactions ?? (_Reactions = new List<Reaction>()); }
            protected set { Reactions = value; }
        }

    }
}
