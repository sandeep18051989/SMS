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
        IList<News> GetAllNews(bool? onlyActive = null);
		IList<News> GetActiveNews();
		News GetNewsById(int newsId);
		IList<News> GetAllNewsByUser(int userId);
		IList<News> GetAllNewsByStatus(NewsStatus newsStatus);
		void Delete(int id);
		int GetNewsCountByCreatedDate(DateTime createddate);
		News GetNewsByShortName(string name);
        IDictionary<string, int> GetDistinctAuthorAndCount(bool? onlyActive = null);

        int GetCountByAuthor(string author);

        IList<News> GetLatestNews(int? excepteventid = null);

        IList<News> GetOlderNews(int? excepteventid = null);

        #region Paging

        IPagedList<News> GetPagedNews(string keyword = null, string author = null, int statusid = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool? onlyActive = null);

        #endregion
    }
}
