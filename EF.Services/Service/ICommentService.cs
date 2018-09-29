using System;
using System.Collections.Generic;
using EF.Core.Data;

namespace EF.Services.Service
{
	public interface ICommentService
	{
		void Insert(Comment comments);
		void Update(Comment comments);

		Comment GetCommentById(int commentId);
		IList<Comment> GetCommentsByUser(int userid);

		int GetCommentCountByCreatedDate(DateTime createddate);
		IList<Comment> GetCommentsByManualDate(DateTime date);

	}
}
