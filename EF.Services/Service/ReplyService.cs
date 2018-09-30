using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class ReplyService : IReplyService
    {
        public readonly IRepository<Reply> _repositoryReply;
        public ReplyService(IRepository<Reply> repositoryBlog)
        {
            this._repositoryReply = repositoryBlog;
        }

        #region IBlogService Members

        public void Insert(Reply replies)
        {
            _repositoryReply.Insert(replies);
        }

        public void Update(Reply replies)
        {
            _repositoryReply.Update(replies);
        }

        public IList<Reply> GetAllRepliesByComment(int commentId)
        {
            if (commentId == 0)
                throw new System.Exception("Comment Id Missing");

            var query = _repositoryReply.Table;

            if (commentId > 0)
                query = query.Where(x => x.CommentId == commentId);

            return query.ToList();
        }

        #endregion
    }
}
