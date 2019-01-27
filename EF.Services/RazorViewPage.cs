using System.IO;
using System.Web.Mvc;

namespace EF.Services
{
    public abstract class RazorViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public override string Layout
        {
            get
            {
                var layout = base.Layout;

                if (!string.IsNullOrEmpty(layout))
                {
                    var filename = Path.GetFileNameWithoutExtension(layout);
                    ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

                    if (viewResult.View != null && viewResult.View is RazorView)
                    {
                        layout = (viewResult.View as RazorView).ViewPath;
                    }
                }

                return layout;
            }
            set
            {
                base.Layout = value;
            }
        }

        public abstract class WebViewPage : WebViewPage<dynamic>
        {
        }
    }
}