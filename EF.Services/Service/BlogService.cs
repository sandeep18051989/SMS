using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

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

			return _repositoryBlog.Table.Where(x => x.UserId == userid).OrderByDescending(x => x.CreatedOn).ToList();
		}

		public Blog GetBlogByName(string name)
		{
			if (!string.IsNullOrEmpty(name))
				return _repositoryBlog.Table.FirstOrDefault(a => a.Name.Trim().ToLower() == name.ToLower() && a.IsDeleted == false);

			return null;
		}

		#endregion
	}
}
