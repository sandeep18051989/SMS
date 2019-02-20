using System.Collections.Generic;
using EF.Core.Data;
namespace EF.Services.Service
{
	public interface IBlogService
	{
		void Insert(Blog blogs);
		void Update(Blog blogs);
		void Delete(int id);
		IList<Blog> GetAllBlogs(bool? onlyActive = null);
		Blog GetBlogById(int id);
		IList<Blog> GetBlogsByUser(int userid);
		Blog GetBlogByName(string name);
        IList<Blog> GetLatestBlogs(int? exceptblogid = null, int userid = 0);
        IList<Blog> GetOlderBlogs(int? exceptblogid = null, int userid = 0);
        IDictionary<string, int> GetDistinctSubjectAndCount(bool? onlyActive = null, int userid = 0);
        int GetCountBySubject(string subject, int userid=0);
        bool IsUserBlogger(int userid);
        int GetBlogCountByUser(int userid);

        #region Paging

        IPagedList<Blog> GetPagedBlogs(string keyword = null, string subject = null, int userid = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool? onlyActive = null);

        #endregion

    }
}
