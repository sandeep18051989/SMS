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
		public readonly IRepository<Blog> _repositoryBlog;
		public BlogService(IRepository<Blog> repositoryBlog)
		{
			_repositoryBlog = repositoryBlog;
		}
		#region IBlogService Members

		public void Insert(Blog blogs)
		{
			_repositoryBlog.Insert(blogs);
		}

		public void Update(Blog blogs)
		{
			_repositoryBlog.Update(blogs);
		}

		public void Delete(int id)
		{
			_repositoryBlog.Delete(id);
		}

		#endregion

		#region Methods

		public virtual IList<Blog> GetAllBlogs(bool? onlyActive = null)
		{
			return _repositoryBlog.GetAll().Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && x.IsDeleted == false).ToList();
		}

		public Blog GetBlogById(int id)
		{
			if (id == 0)
				throw new Exception("Blog Id Not Specified.");

			return _repositoryBlog.GetByID(id);
		}

		public virtual IList<Blog> GetBlogsByUser(int userid)
		{
			if (userid == 0)
				throw new Exception("User Id Not Specified.");

			return _repositoryBlog.Table.Where(x => x.UserId == userid && x.IsDeleted == false).OrderByDescending(x => x.CreatedOn).ToList();
		}

		public Blog GetBlogByName(string name)
		{
			if (!string.IsNullOrEmpty(name))
				return _repositoryBlog.Table.FirstOrDefault(a => a.Name.Trim().ToLower() == name.ToLower() && a.IsDeleted == false);

			return null;
		}

        public IList<Blog> GetLatestBlogs(int? exceptblogid=null, int userid=0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, totalDays).Date;
            return _repositoryBlog.Table.Where(x => (!exceptblogid.HasValue || x.Id != exceptblogid.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) >= startDate && DbFunctions.TruncateTime(x.CreatedOn) <= endDate))).ToList();
        }

        public IList<Blog> GetOlderBlogs(int? exceptblogid = null, int userid = 0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            return _repositoryBlog.Table.Where(x => (!exceptblogid.HasValue || x.Id != exceptblogid.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) < startDate))).ToList();
        }

        public IDictionary<string, int> GetDistinctSubjectAndCount(bool? onlyActive=null, int userid = 0)
        {
            var allSubjects = _repositoryBlog.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted).Distinct().ToList();
            var lstDistinct = allSubjects.Select(x => x.Subject).Distinct().ToList();
            return lstDistinct.ToDictionary(x => x, x => GetCountBySubject(x));
        }

        public int GetCountBySubject(string subject, int userid = 0)
        {
            return _repositoryBlog.Table.Count(x => (userid==0 || x.UserId == userid) && !x.IsDeleted && x.Subject.Trim().ToLower().Contains(subject.Trim().ToLower()));
        }

        #endregion
    }
}
