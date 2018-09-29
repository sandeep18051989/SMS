using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF.Core.Enums;

namespace EF.Core.Data
{
	public partial class Message : BaseEntity
	{
		[NotMapped]
		public virtual ICollection<File> _Files { get; set; }
		public int MessageGroupId { get; set; }
		public DateTime MessageDate { get; set; }
		public string Content { get; set; }
		public virtual MessageGroup MessageGroup { get; set; }
		private int MessageStatusId { get; set; }
		public bool IsDeleted { get; set; }
		public bool IsActive { get; set; }
		[NotMapped]
		public MessageStatus MessageStatus
		{
			get
			{
				return (MessageStatus)this.MessageStatusId;
			}
			set
			{
				this.MessageStatusId = (int)value;
			}
		}

		public virtual ICollection<File> Files
		{
			get { return _Files ?? (_Files = new List<File>()); }
			protected set { _Files = value; }
		}
	}
}
