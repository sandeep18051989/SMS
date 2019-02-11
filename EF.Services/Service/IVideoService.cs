using System.Collections.Generic;
using EF.Core.Data;

namespace EF.Services.Service
{
	public interface IVideoService
	{
		void Insert(Video videos);
		void Update(Video videos);
		void Delete(int id);
        IList<Video> GetAllVideos(bool? onlyActive = null, bool? onlyOpenResource = null);

        IList<User> GetAllVideosByUser(int userId, bool? onlyActive = null, bool? onlyOpenResource = null);
        Video GetVideoById(int videoId);
        void ToggleActiveStatus(int id);

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
