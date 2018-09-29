using System.Collections.Generic;
using EF.Core.Data;
using System.Linq;
using System;
using EF.Core;

namespace EF.Services.Service
{
	public class BlogService : IBlogService
	{
		public readonly IRepository<Blog> _repositoryBlog;
		public BlogService(IRepository<Blog> repositoryBlog)
		{
			this._repositoryBlog = repositoryBlog;
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

		public virtual IList<Blog> GetAllBlogs(bool? active)
		{
			var blogs = _repositoryBlog.Table.ToList();

			if (active.HasValue)
				blogs = blogs.Where(b => b.IsActive == active.Value).ToList();

			return blogs.OrderByDescending(x => x.CreatedOn).ToList();
		}

		public Blog GetBlogById(int id)
		{
			if (id == 0)
				throw new Exception("Blog Id Not Specified.");

			return _repositoryBlog.Table.FirstOrDefault(x => x.Id == id);
		}

		public virtual IList<Blog> GetBlogsByUser(int userid)
		{
			if (userid == 0)
				throw new Exception("User Id Not Specified.");

			return _repositoryBlog.Table.Where(x => x.UserId == userid).OrderByDescending(x => x.CreatedOn).ToList();
		}

		#endregion
	}
}
