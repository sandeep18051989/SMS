using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Core.Data
{
    public partial class Reaction : BaseEntity
    {
        //[NotMapped]
        //public virtual ICollection<Blog> _Blog { get; set; }
        //[NotMapped]
        //public virtual ICollection<Product> _Products { get; set; }
        //[NotMapped]
        //public virtual ICollection<Event> _Events { get; set; }
        //[NotMapped]
        //public virtual ICollection<Picture> _Pictures { get; set; }
        //[NotMapped]
        //public virtual ICollection<Video> _Videos { get; set; }
 
        //[NotMapped]
        //public virtual ICollection<News> _News { get; set; }

        //[NotMapped]
        //public virtual ICollection<Comment> _Comments { get; set; }

        //[NotMapped]
        //public virtual ICollection<Reply> _Replies { get; set; }

        public string Username { get; set; }
        public bool? IsLike { get; set;}

        public bool? IsDislike { get; set; }

        public bool? IsAngry { get; set; }

        public bool? IsHappy { get; set; }

        public bool? IsSad { get; set; }

        public bool? IsLOL { get; set; }

        public int? Rating { get; set; }

        public int? BlogId { get; set; }
        public int? ProductId { get; set; }
        public int? EventId { get; set; }
        public int? PictureId { get; set; }
        public int? VideoId { get; set; }
        public int? NewsId { get; set; }
        public int? CommentId { get; set; }
        public int? ReplyId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Product Product { get; set; }
        public virtual Event Event { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual Video Video { get; set; }
        public virtual News News { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Reply Reply { get; set; }

        //#region Navigation Properties
        //public virtual ICollection<Blog> Blogs
        //{
        //    get { return _Blog ?? (_Blog = new List<Blog>()); }
        //    protected set { _Blog = value; }
        //}

        //public virtual ICollection<Product> Products
        //{
        //    get { return _Products ?? (_Products = new List<Product>()); }
        //    protected set { _Products = value; }
        //}

        //public virtual ICollection<Event> Events
        //{
        //    get { return _Events ?? (_Events = new List<Event>()); }
        //    protected set { _Events = value; }
        //}

        //public virtual ICollection<News> News
        //{
        //    get { return _News ?? (_News = new List<News>()); }
        //    protected set { _News = value; }
        //}

        //public virtual ICollection<Comment> Comments
        //{
        //    get { return _Comments ?? (_Comments = new List<Comment>()); }
        //    protected set { _Comments = value; }
        //}

        //public virtual ICollection<Reply> Replies
        //{
        //    get { return _Replies ?? (_Replies = new List<Reply>()); }
        //    protected set { _Replies = value; }
        //}

        //public virtual ICollection<Picture> Pictures
        //{
        //    get { return _Pictures ?? (_Pictures = new List<Picture>()); }
        //    protected set { _Pictures = value; }
        //}

        //public virtual ICollection<Video> Videos
        //{
        //    get { return _Videos ?? (_Videos = new List<Video>()); }
        //    protected set { _Videos = value; }
        //}
        //#endregion

    }
}
