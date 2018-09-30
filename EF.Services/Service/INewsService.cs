using EF.Core.Data;
using System.Collections.Generic;
using System;
using EF.Core.Enums;

namespace EF.Services.Service
{
	public interface INewsService
	{
		void Insert(News news);
		void Update(News news);
		IList<News> GetAllNews();
		IList<News> GetActiveNews();
		News GetNewsById(int newsId);
		IList<News> GetAllNewsByUser(int userId);
		IList<News> GetAllNewsByStatus(NewsStatus newsStatus);
		void Delete(int id);
		int GetNewsCountByCreatedDate(DateTime createddate);

	}
}
