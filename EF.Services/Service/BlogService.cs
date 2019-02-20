using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using System.Data.Entity;

namespace EF.Services.Service
{
	public class BlogService : IBlogService
	{
		public readonly IRepository<Blog> _blogRepository;
		public BlogService(IRepository<Blog> blogRepository)
		{
			_blogRepository = blogRepository;
		}
		#region IBlogService Members

		public void Insert(Blog blogs)
		{
			_blogRepository.Insert(blogs);
		}

		public void Update(Blog blogs)
		{
			_blogRepository.Update(blogs);
		}

		public void Delete(int id)
		{
			_blogRepository.Delete(id);
		}

		#endregion

		#region Methods

		public virtual IList<Blog> GetAllBlogs(bool? onlyActive = null)
		{
			return _blogRepository.GetAll().Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
		}

		public Blog GetBlogById(int id)
		{
			if (id == 0)
				throw new Exception("Blog Id Not Specified.");

			return _blogRepository.GetByID(id);
		}

		public virtual IList<Blog> GetBlogsByUser(int userid)
		{
			if (userid == 0)
				throw new Exception("User Id Not Specified.");

			return _blogRepository.Table.Where(x => x.UserId == userid && x.IsDeleted == false).OrderByDescending(x => x.CreatedOn).ToList();
		}

		public Blog GetBlogByName(string name)
		{
			if (!string.IsNullOrEmpty(name))
				return _blogRepository.Table.FirstOrDefault(a => a.Name.Trim().ToLower() == name.ToLower() && a.IsDeleted == false);

			return null;
		}

        public IList<Blog> GetLatestBlogs(int? exceptblogid=null, int userid=0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, totalDays).Date;
            return _blogRepository.Table.Where(x => (!exceptblogid.HasValue || x.Id != exceptblogid.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) >= startDate && DbFunctions.TruncateTime(x.CreatedOn) <= endDate))).ToList();
        }

        public IList<Blog> GetOlderBlogs(int? exceptblogid = null, int userid = 0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            return _blogRepository.Table.Where(x => (!exceptblogid.HasValue || x.Id != exceptblogid.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) < startDate))).ToList();
        }

        public IDictionary<string, int> GetDistinctSubjectAndCount(bool? onlyActive=null, int userid = 0)
        {
            var allSubjects = _blogRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted).Distinct().ToList();
            var lstDistinct = allSubjects.Select(x => x.Subject).Distinct().ToList();
            return lstDistinct.ToDictionary(x => x, x => GetCountBySubject(x));
        }

        public int GetCountBySubject(string subject, int userid = 0)
        {
            return _blogRepository.Table.Count(x => (userid==0 || x.UserId == userid) && !x.IsDeleted && x.Subject.Trim().ToLower().Contains(subject.Trim().ToLower()));
        }

        public int GetBlogCountByUser(int userid)
        {
            return _blogRepository.Table.Count(x => !x.IsDeleted && x.IsApproved && x.IsActive && x.UserId == userid);
        }

        public bool IsUserBlogger(int userid)
        {
            return _blogRepository.Table.Any(x => !x.IsDeleted && x.IsApproved && x.IsActive && x.UserId == userid);
        }

        #endregion

        #region Paging

        public virtual IPagedList<Blog> GetPagedBlogs(string keyword = null, string subject = null, int userid=0, int pageIndex = 0, int pageSize = int.MaxValue, bool? onlyActive = null)
        {
            var query = _blogRepository.Table;
            if (onlyActive.HasValue)
            {
                query = query.Where(n => n.IsActive == onlyActive.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword) || x.Subject.Contains(keyword) || x.BlogHtml.Contains(keyword));

            if (!string.IsNullOrEmpty(subject))
                query = query.Where(x => x.Subject.Contains(subject));

            if (userid > 0)
                query = query.Where(x => x.UserId == userid);

            query = query.Where(n => n.IsApproved && !n.IsDeleted).OrderByDescending(n => n.ModifiedOn);

            var blogs = new PagedList<Blog>(query, pageIndex, pageSize);
            return blogs;
        }

        #endregion
    }
}
