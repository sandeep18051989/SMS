using System.Web.Mvc;

namespace SMS.Controllers
{
    public partial class KeepAliveController : PublicHttpController
    {
        public ActionResult Index()
        {
            return Content("I am Active!");
        }
    }
}
