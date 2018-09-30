using EF.Core.Data;

namespace SMS.Models
{
	public partial class EventBlogCommentsModel
    {
        public Blog blogs { get; set; }

        public Event events { get; set; }

        public Comment comments { get; set; }

    }
}