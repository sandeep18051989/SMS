using EF.Core.Data;
using System.Collections;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface IVideoService
    {
        void Insert(Video videos);
        void Update(Video videos);
        IList<Video> GetAllVideos();
        IList<Video> GetVideos(bool active=true);
        //IList<Videos> GetAllVideosByEvent(int eventId);
        //IList<Videos> GetAllVideosByBlog(int blogId);
        IList<User> GetAllVideosByUser(int userId);

    }
}
