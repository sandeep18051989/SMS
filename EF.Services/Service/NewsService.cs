using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using System.Data.Entity;

namespace EF.Services.Service
{
	public class NewsService : INewsService
	{
		public readonly IRepository<News> _newsRepository;
		public NewsService(IRepository<News> newsRepository)
		{
			this._newsRepository = newsRepository;
		}
		#region INewsService Members

		public void Insert(News news)
		{
			_newsRepository.Insert(news);
		}

		public void Update(News news)
		{
			_newsRepository.Update(news);
		}

		public void Delete(int id)
		{
			_newsRepository.Delete(id);
		}

		#endregion

		#region Methods

		public IList<News> GetAllNews(bool? onlyActive = null)
		{
			return _newsRepository.Table.Where(x => (!onlyActive.HasValue || onlyActive.Value == x.IsActive) && !x.IsDeleted).ToList();
		}

		public IList<News> GetActiveNews()
		{
			return _newsRepository.Table.Where(a => a.IsActive == true).OrderByDescending(a => a.CreatedOn).ToList();
		}
		public News GetNewsById(int newsId)
		{
			if (newsId > 0)
				return _newsRepository.Table.FirstOrDefault(a => a.Id == newsId);

			return null;
		}
		public IList<News> GetAllNewsByUser(int userId)
		{
			if (userId > 0)
				return _newsRepository.Table.Where(a => a.UserId == userId).ToList();

			return new List<News>();
		}

		public IList<News> GetAllNewsByStatus(NewsStatus newsStatus)
		{
			return _newsRepository.Table.Where(a => a.NewsStatusId == (int)newsStatus).ToList();
		}

		public int GetNewsCountByCreatedDate(DateTime createddate)
		{
			if (createddate == null)
				throw new ArgumentNullException("created date empty.");

			var query = _newsRepository.Table.ToList();
			var lstNews = new List<News>();
			foreach (var q in query)
			{
				if (q.CreatedOn.HasValue && q.CreatedOn.Value.Date == createddate.Date)
				{
					lstNews.Add(q);
				}
			}
			return lstNews.ToList().Count;

		}

		public News GetNewsByShortName(string name)
		{
			if (!string.IsNullOrEmpty(name))
				return _newsRepository.Table.FirstOrDefault(a => a.ShortName.Trim().ToLower() == name.ToLower() && a.IsDeleted == false);

			return null;
		}

        public IDictionary<string, int> GetDistinctAuthorAndCount(bool? onlyActive = null)
        {
            var allAuthors = _newsRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && !x.IsDeleted).Distinct().ToList();
            var lstDistinct = allAuthors.Select(x => x.Author).Distinct().ToList();
            return lstDistinct.ToDictionary(x => x, x => GetCountByAuthor(x));
        }

        public int GetCountByAuthor(string author)
        {
            return _newsRepository.Table.Count(x => !x.IsDeleted && x.Author.Trim().ToLower().Contains(author.Trim().ToLower()));
        }

        public IList<News> GetLatestNews(int? excepteventid = null)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, totalDays).Date;
            return _newsRepository.Table.Where(x => (!excepteventid.HasValue || x.Id != excepteventid.Value) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) >= startDate && DbFunctions.TruncateTime(x.CreatedOn) <= endDate))).ToList();
        }

        public IList<News> GetOlderNews(int? excepteventid = null)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            return _newsRepository.Table.Where(x => (!excepteventid.HasValue || x.Id != excepteventid.Value) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) < startDate))).ToList();
        }

        #endregion

        #region Paging

        public virtual IPagedList<News> GetPagedNews(string keyword = null, string author = null, int statusid = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool? onlyActive = null)
        {
            var query = _newsRepository.Table;
            if (onlyActive.HasValue)
            {
                query = query.Where(n => n.IsActive == onlyActive.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.ShortName.Contains(keyword) || x.Author.Contains(keyword) || x.Description.Contains(keyword));

            if (!string.IsNullOrEmpty(author))
                query = query.Where(x => x.Author.Contains(author));

            if (statusid > 0)
                query = query.Where(x => x.NewsStatusId == statusid);

            query = query.Where(n => !n.IsDeleted).OrderByDescending(n => n.ModifiedOn);

            var news = new PagedList<News>(query, pageIndex, pageSize);
            return news;
        }

        #endregion
    }
}
