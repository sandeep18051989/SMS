using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;

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

		public IList<News> GetAllNews()
		{
			return _newsRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();
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
				if (q.CreatedOn.Date == createddate.Date)
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

		#endregion
	}
}
