using System.Collections.Generic;
using EF.Core.Data;
namespace EF.Services.Service
{
	public interface IBlogService
	{
		void Insert(Blog blogs);

		void Update(Blog blogs);

		void Delete(int id);

		IList<Blog> GetAllBlogs(bool? active);

		Blog GetBlogById(int id);

		IList<Blog> GetBlogsByUser(int userid);



	}
}
