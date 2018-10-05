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
		public VideoService(IRepository<Video> videoRepository, IRepository<EventVideo> eventVideoRepository, IRepository<ProductVideo> productVideoRepository, IRepository<NewsVideo> newsVideoRepository)
		{
			this._videoRepository = videoRepository;
			this._eventVideoRepository = eventVideoRepository;
			this._newsVideoRepository = newsVideoRepository;
			this._productVideoRepository = productVideoRepository;
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

		public IList<Video> GetAllVideos()
		{
			return _videoRepository.Table.OrderBy(a => a.DisplayOrder).ToList();
		}

		public IList<Video> GetVideos(bool active = true)
		{
			if (active)
				return _videoRepository.Table.OrderBy(a => a.DisplayOrder).ToList();

			return _videoRepository.Table.OrderBy(a => a.DisplayOrder).ToList();


		}

		public Video GetVideoById(int videoId)
		{
			if (videoId == 0)
				return null;

			return _videoRepository.Table.FirstOrDefault(a => a.Id == videoId);
		}

		public IList<User> GetAllVideosByUser(int userId)
		{
			if (userId > 0)
			{
				var query = _videoRepository.Table.Where(a => a.UserId == userId).OrderBy(a => a.DisplayOrder).ToList();
			}

			return null;
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
			var eventVideo = _eventVideoRepository.Table.FirstOrDefault(s => s.Id == id);
			if (eventVideo != null)
			{
				var video = _videoRepository.Table.FirstOrDefault(pic => pic.Id == eventVideo.VideoId);
				if (video != null)
					_videoRepository.Delete(video);

				_eventVideoRepository.Delete(eventVideo);
			}
		}
		public EventVideo GetEventVideoById(int id)
		{
			if (id == 0)
				throw new Exception("Event video id is missing");

			return _eventVideoRepository.Table.FirstOrDefault(x => x.Id == id);

		}
		public EventVideo GetEventVideoByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("Event video id is missing");

			return _eventVideoRepository.Table.FirstOrDefault(x => x.VideoId == id);

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
			var productVideo = _productVideoRepository.Table.FirstOrDefault(s => s.Id == id);
			if (productVideo != null)
			{
				var video = _videoRepository.Table.FirstOrDefault(pic => pic.Id == productVideo.VideoId);
				if (video != null)
					_videoRepository.Delete(video);

				_productVideoRepository.Delete(productVideo);
			}
		}
		public ProductVideo GetProductVideoById(int id)
		{
			if (id == 0)
				throw new Exception("Product video id is missing");

			return _productVideoRepository.Table.FirstOrDefault(x => x.Id == id);

		}
		public ProductVideo GetProductVideoByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("Product video id is missing");

			return _productVideoRepository.Table.FirstOrDefault(x => x.VideoId == id);

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
			var newsVideo = _newsVideoRepository.Table.FirstOrDefault(s => s.Id == id);
			if (newsVideo != null)
			{
				var video = _videoRepository.Table.FirstOrDefault(pic => pic.Id == newsVideo.VideoId);
				if (video != null)
					_videoRepository.Delete(video);

				_newsVideoRepository.Delete(newsVideo);
			}
		}
		public NewsVideo GetNewsVideoById(int id)
		{
			if (id == 0)
				throw new Exception("News video id is missing");

			return _newsVideoRepository.Table.FirstOrDefault(x => x.Id == id);

		}

		#endregion

		#endregion

	}
}
