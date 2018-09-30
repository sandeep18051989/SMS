using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class VideoService : IVideoService
    {
        public readonly IRepository<Video> _videoRepository;
        public VideoService(IRepository<Video> videoRepository)
        {
            this._videoRepository = videoRepository;
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
        //public IList<Videos> GetAllVideosByEvent(int eventId)
        //{
        //    if (eventId > 0)
        //    {
        //        var query = _videoRepository.Table.Where(a=>a.events.Any(b=>b.Id == eventId)).OrderBy(a => a.DisplayOrder).ToList();
        //        return query;
        //    }

        //    return null;
        //}
        //public IList<Videos> GetAllVideosByBlog(int blogId)
        //{
        //    if (blogId > 0)
        //    {
        //        var query = _videoRepository.Table.Where(a => a.blogs.Any(b => b.Id == blogId)).OrderBy(a => a.DisplayOrder).ToList();
        //    }

        //    return null;
        //}
        public IList<User> GetAllVideosByUser(int userId)
        {
            if (userId > 0)
            {
                var query = _videoRepository.Table.Where(a => a.UserId == userId).OrderBy(a => a.DisplayOrder).ToList();
            }

            return null;
        }

        #endregion
    }
}
