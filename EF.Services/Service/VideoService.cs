﻿using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class VideoService : IVideoService
	{
		public readonly IRepository<Video> _videoRepository;
		public readonly IRepository<EventVideo> _eventVideoRepository;
		public readonly IRepository<ProductVideo> _productVideoRepository;
		public readonly IRepository<NewsVideo> _newsVideoRepository;
		public readonly IRepository<BlogVideo> _blogVideoRepository;
		public VideoService(IRepository<Video> videoRepository, IRepository<EventVideo> eventVideoRepository, IRepository<ProductVideo> productVideoRepository, IRepository<NewsVideo> newsVideoRepository, IRepository<BlogVideo> blogVideoRepository)
		{
			this._videoRepository = videoRepository;
			this._eventVideoRepository = eventVideoRepository;
			this._newsVideoRepository = newsVideoRepository;
			this._productVideoRepository = productVideoRepository;
			this._blogVideoRepository = blogVideoRepository;
		}
		#region IVideoService Members

		public void Insert(Video videos)
		{
			_videoRepository.Insert(videos);
		}

		public void Update(Video videos)
		{
			_videoRepository.Update(videos);
		}

		public void Delete(int id)
		{
			if (id == 0)
				throw new ArgumentNullException("role");

			_videoRepository.Delete(id);
		}

		#endregion

		#region Utilities

		public IList<Video> GetAllVideos(bool? onlyActive = null, bool? onlyOpenResource = null)
		{
			return _videoRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && (!onlyOpenResource.HasValue || x.IsOpenResource == onlyOpenResource.Value)).ToList();
		}

        public IList<User> GetAllVideosByUser(int userId, bool? onlyActive = null, bool? onlyOpenResource = null)
        {
            if (userId > 0)
            {
                var query = _videoRepository.Table.Where(x => x.UserId == userId && (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && (!onlyOpenResource.HasValue || x.IsOpenResource == onlyOpenResource.Value)).OrderBy(a => a.DisplayOrder).ToList();
            }

            return null;
        }

        public Video GetVideoById(int videoId)
		{
			if (videoId == 0)
				return null;

			return _videoRepository.Table.FirstOrDefault(a => a.Id == videoId);
		}

		public EventVideo GetEventVideoByVideoId(int id)
		{
			if (id == 0)
				throw new Exception("Event video id is missing");

			return _eventVideoRepository.Table.FirstOrDefault(x => x.VideoId == id);
		}

		public NewsVideo GetNewsVideoByVideoId(int id)
		{
			if (id == 0)
				throw new Exception("News video id is missing");

			return _newsVideoRepository.Table.FirstOrDefault(x => x.VideoId == id);
		}
		public ProductVideo GetProductVideoByVideoId(int id)
		{
			if (id == 0)
				throw new Exception("Event video id is missing");

			return _productVideoRepository.Table.FirstOrDefault(x => x.VideoId == id);
		}

		public BlogVideo GetBlogVideoByVideoId(int id)
		{
			if (id == 0)
				throw new Exception("Blog video id is missing");

			return _blogVideoRepository.Table.FirstOrDefault(x => x.VideoId == id);
		}

		public IList<EventVideo> GetEventVideosByEventId(int id)
		{
			if (id == 0)
				throw new Exception("Event id is missing");

			return _eventVideoRepository.Table.Where(x => x.EventId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

		public IList<ProductVideo> GetProductVideosByProductId(int id)
		{
			if (id == 0)
				throw new Exception("Event id is missing");

			return _productVideoRepository.Table.Where(x => x.ProductId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

		public IList<NewsVideo> GetNewsVideosByNewsId(int id)
		{
			if (id == 0)
				throw new Exception("Event id is missing");

			return _newsVideoRepository.Table.Where(x => x.NewsId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

		public IList<BlogVideo> GetBlogVideosByBlogId(int id)
		{
			if (id == 0)
				throw new Exception("Blog id is missing");

			return _blogVideoRepository.Table.Where(x => x.BlogId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

        public void ToggleActiveStatus(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objVideo = _videoRepository.GetByID(id);
            if (objVideo != null)
            {
                objVideo.IsActive = !objVideo.IsActive;
                objVideo.ModifiedOn = DateTime.Now;
                _videoRepository.Update(objVideo);
            }
        }

        #endregion

        #region Video Definetions

        #region Event

        public void InsertEventVideo(EventVideo eventVideo)
		{
			_eventVideoRepository.Insert(eventVideo);
		}
		public void UpdateEventVideo(EventVideo eventVideo)
		{
			_eventVideoRepository.Update(eventVideo);
		}
		public void DeleteEventVideo(int id)
		{
			var eventVideo = _eventVideoRepository.GetByID(id);
            if (eventVideo != null)
			{
                var video = _videoRepository.GetByID(eventVideo.VideoId);
				if (video != null)
					_videoRepository.Delete(video);
			}
		}
		public EventVideo GetEventVideoById(int id)
		{
			if (id == 0)
				throw new Exception("Event video id is missing");

			return _eventVideoRepository.GetByID(id);

        }
		public EventVideo GetEventVideoByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("Event video id is missing");

			return _eventVideoRepository.GetByID(id);

        }

		#endregion

		#region Product

		public void InsertProductVideo(ProductVideo productVideo)
		{
			_productVideoRepository.Insert(productVideo);
		}
		public void UpdateProductVideo(ProductVideo productVideo)
		{
			_productVideoRepository.Update(productVideo);
		}
		public void DeleteProductVideo(int id)
		{
			var productVideo = _productVideoRepository.GetByID(id);
			if (productVideo != null)
			{
				var video = _videoRepository.GetByID(productVideo.VideoId);
				if (video != null)
					_videoRepository.Delete(video);
			}
		}
		public ProductVideo GetProductVideoById(int id)
		{
			if (id == 0)
				throw new Exception("Product video id is missing");

			return _productVideoRepository.GetByID(id);

        }
		public ProductVideo GetProductVideoByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("Product video id is missing");

			return _productVideoRepository.GetByID(id);

        }

		#endregion

		#region News

		public void InsertNewsVideo(NewsVideo newsVideo)
		{
			_newsVideoRepository.Insert(newsVideo);
		}
		public void UpdateNewsVideo(NewsVideo newsVideo)
		{
			_newsVideoRepository.Update(newsVideo);
		}
		public void DeleteNewsVideo(int id)
		{
			var newsVideo = _newsVideoRepository.GetByID(id);
            if (newsVideo != null)
			{
				var video = _videoRepository.GetByID(newsVideo.VideoId);
				if (video != null)
					_videoRepository.Delete(video);
			}
		}
		public NewsVideo GetNewsVideoById(int id)
		{
			if (id == 0)
				throw new Exception("News video id is missing");

			return _newsVideoRepository.GetByID(id);

        }

		#endregion

		#region Blog

		public void InsertBlogVideo(BlogVideo blogVideo)
		{
			_blogVideoRepository.Insert(blogVideo);
		}
		public void UpdateBlogVideo(BlogVideo blogVideo)
		{
			_blogVideoRepository.Update(blogVideo);
		}
		public void DeleteBlogVideo(int id)
		{
			var blogVideo = _blogVideoRepository.GetByID(id);
			if (blogVideo != null)
			{
				var video = _videoRepository.GetByID(blogVideo.VideoId);
				if (video != null)
					_videoRepository.Delete(video);
			}
		}
		public BlogVideo GetBlogVideoById(int id)
		{
			if (id == 0)
				throw new Exception("Blog video id is missing");

			return _blogVideoRepository.GetByID(id);

		}

		#endregion

		#endregion

	}
}
