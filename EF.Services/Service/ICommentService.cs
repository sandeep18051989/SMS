using System;
using System.Collections.Generic;
using EF.Core.Data;

namespace EF.Services.Service
{
	public interface ICommentService
	{
		void Insert(Comment comments);
		void Update(Comment comments);

        void DeleteComment(int id);

        void DeleteComments(IList<Comment> comments);
        IList<Comment> GetCommentsByIds(int[] commentIds);

        Comment GetCommentById(int commentId);

        void ToggleActiveStatusComment(int id);

        IList<Comment> GetCommentsByUser(int userid);

		int GetCommentCountByCreatedDate(DateTime createddate);
		IList<Comment> GetCommentsByManualDate(DateTime date);

        #region Get Related Comments

        IList<Comment> GetCommentsByHomework(int id);

        IList<Comment> GetCommentsByNews(int id);

        IList<Comment> GetCommentsByProduct(int id);

        IList<Comment> GetCommentsByReactions(int id);

        IList<Comment> GetCommentsByExam(int id);

        IList<Comment> GetCommentsByEvent(int id);

        IList<Comment> GetCommentsByBlog(int id);

        #endregion

    }
}
