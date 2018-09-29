using EF.Services.Http;
using System.Net;

namespace EF.Services.Tasks
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAlive : ITask
    {
        private readonly IUrlHelper _urlContext;

        public KeepAlive(IUrlHelper urlContext)
        {
            this._urlContext = urlContext;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            string url = _urlContext.GetLocation() + "keepalive/index";
            using (var wc = new WebClient())
            {
                wc.DownloadString(url);
            }
        }
    }
}
