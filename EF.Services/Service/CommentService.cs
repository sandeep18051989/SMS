using EF.Core.Data;
using EF.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using EF.Core;
using System.Data.Entity;

namespace EF.Services.Service
{
    public class CommentService : ICommentService
    {
        public readonly IRepository<Comment> _commentRepository;
        public CommentService(IRepository<Comment> repositoryComment)
        {
            this._commentRepository = repositoryComment;
        }

        #region IProductService Members

        public void Insert(Comment comment)
        {
            _commentRepository.Insert(comment);
        }

        public void Update(Comment comment)
        {
            _commentRepository.Update(comment);
        }

        public void DeleteComment(int id)
        {
            var objComment = _commentRepository.Table.FirstOrDefault(s => s.Id == id);
            if (objComment != null)
            {
                objComment.IsActive = false;
                objComment.IsDeleted = true;
                _commentRepository.Update(objComment);
            }
        }

        public virtual void DeleteComments(IList<Comment> comments)
        {
            if (comments == null)
                throw new ArgumentNullException("comments");

            foreach (var _comment in comments)
            {
                _comment.IsDeleted = true;
                _commentRepository.Update(_comment);
            }
        }

        public virtual IList<Comment> GetCommentsByIds(int[] commentIds)
        {
            if (commentIds == null || commentIds.Length == 0)
                return new List<Comment>();

            var query = from r in _commentRepository.Table
                        where commentIds.Contains(r.Id)
                        select r;

            var comments = query.ToList();

            var sortedComments = new List<Comment>();
            foreach (int id in commentIds)
            {
                var comment = comments.Find(x => x.Id == id);
                if (comment != null)
                    sortedComments.Add(comment);
            }
            return sortedComments;
        }

        #endregion

        #region Other Methods

        public Comment GetCommentById(int commentId)
        {
            if (commentId > 0)
            {
                return _commentRepository.GetByID(commentId);
            }

            return null;
        }

        public void ToggleActiveStatusComment(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("role");

            var objComment = _commentRepository.Table.Where(x => x.Id == id).FirstOrDefault();
            if (objComment != null)
            {
                objComment.IsActive = !objComment.IsActive;
                _commentRepository.Update(objComment);
            }

        }

        public IList<Comment> GetCommentsByUser(int userid)
        {
            if (userid > 0)
                return _commentRepository.Table.Where(x => x.UserId == userid).OrderByDescending(x => x.CreatedOn).ToList();

            return new List<Comment>();
        }

        public int GetCommentCountByCreatedDate(DateTime createddate)
        {
            if (createddate == null)
                throw new ArgumentNullException("createddate");

            return _commentRepository.Table.Count(x => DbFunctions.TruncateTime(x.CreatedOn) == createddate.Date);
        }

        public IList<Comment> GetCommentsByNews(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.News.Any(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByProduct(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Products.Any(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByReactions(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Reactions.Any(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByEvent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Events.Any(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByBlog(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Blogs.Any(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByDate(DateTime createddate)
        {
            return _commentRepository.Table.Where(x => DbFunctions.TruncateTime(x.CreatedOn) == createddate.Date).ToList();
        }

        #endregion
    }
}
