using System.Collections.Generic;
using EF.Core.Data;

namespace EF.Services.Service
{
	public interface IVideoService
	{
		void Insert(Video videos);
		void Update(Video videos);
		void Delete(int id);
		IList<Video> GetAllVideos();
		IList<Video> GetVideos(bool active = true);
		Video GetVideoById(int videoId);
		IList<User> GetAllVideosByUser(int userId);

		#region Video Definetions

		IList<EventVideo> GetEventVideosByEventId(int id);
		IList<NewsVideo> GetNewsVideosByNewsId(int id);
		IList<ProductVideo> GetProductVideosByProductId(int id);

		IList<BlogVideo> GetBlogVideosByBlogId(int id);
		BlogVideo GetBlogVideoByVideoId(int id);

		#region Event

		void InsertEventVideo(EventVideo eventVideo);

		void UpdateEventVideo(EventVideo eventVideo);

		void DeleteEventVideo(int id);

		EventVideo GetEventVideoById(int id);

		EventVideo GetEventVideoByVideoId(int id);

		#endregion

		#region Product

		void InsertProductVideo(ProductVideo productVideo);

		void UpdateProductVideo(ProductVideo productVideo);

		void DeleteProductVideo(int id);

		ProductVideo GetProductVideoById(int id);

		ProductVideo GetProductVideoByVideoId(int id);

		#endregion

		#region News

		void InsertNewsVideo(NewsVideo newsVideo);

		void UpdateNewsVideo(NewsVideo newsVideo);

		void DeleteNewsVideo(int id);

		NewsVideo GetNewsVideoById(int id);

		NewsVideo GetNewsVideoByVideoId(int id);

		#endregion

		#region Blog

		void InsertBlogVideo(BlogVideo blogVideo);
		void UpdateBlogVideo(BlogVideo blogVideo);
		void DeleteBlogVideo(int id);
		BlogVideo GetBlogVideoById(int id);

		#endregion

		#endregion
	}
}
