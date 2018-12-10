using EF.Core.Data;
using EF.Data;
using System.Linq;
using System.Collections.Generic;
using System;
using EF.Core;

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
                return _commentRepository.Table.FirstOrDefault(x => x.Id == commentId);
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

            int commentCount = 0;
            var comments = _commentRepository.Table.ToList();

            foreach (var q in comments)
            {
                DateTime dtcomment = q.CreatedOn;
                if (Equals(dtcomment.Date.Day, createddate.Date.Day) && Equals(dtcomment.Date.Month, createddate.Date.Month) && dtcomment.Date.Year == createddate.Date.Year)
                {
                    commentCount += 1;
                }
            }

            return commentCount;
        }

        public IList<Comment> GetCommentsByManualDate(DateTime date)
        {
            if (date == null)
                throw new ArgumentNullException("date is empty.");

            IList<Comment> lstComments = new List<Comment>();
            var query = _commentRepository.Table.ToList();

            foreach (var q in query)
            {
                if (q.CreatedOn.Date == date.Date)
                    lstComments.Add(q);
            }

            return lstComments.ToList();

        }

        public IList<Comment> GetCommentsByHomework(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Homeworks.All(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByNews(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.News.All(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByProduct(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Products.All(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByReactions(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Reactions.All(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByExam(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Exams.All(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByEvent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Events.All(h => h.Id == id)).ToList();
        }

        public IList<Comment> GetCommentsByBlog(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            return _commentRepository.Table.Where(c => c.Blogs.All(h => h.Id == id)).ToList();
        }

        #endregion
    }
}
