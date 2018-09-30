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

		public IList<Comment> GetCommentsByUser(int userid)
		{
			if (userid > 0)
				return _commentRepository.Table.Where(x => x.UserId == userid).OrderByDescending(x => x.CreatedOn).ToList();

			return new List<Comment>();
		}

		public int GetCommentCountByCreatedDate(DateTime createddate)
		{
			if (createddate == null)
				throw new ArgumentNullException("created date empty.");

			IList<Comment> lstComments = new List<Comment>();
			var query = _commentRepository.Table.ToList();

			foreach (var q in query)
			{
				if (q.CreatedOn.Date == createddate.Date)
					lstComments.Add(q);
			}

			return lstComments.ToList().Count;

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

		#endregion
	}
}
