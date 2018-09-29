using System.Text;
using System.Web;

namespace EF.Services.Http
{
    public class AutoHttpResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection _cookies;
        public AutoHttpResponse()
        {
            this._cookies = new HttpCookieCollection();
        }
        private readonly StringBuilder _outputString = new StringBuilder();

        public string ResponseOutput
        {
            get { return _outputString.ToString(); }
        }

        public override int StatusCode { get; set; }

        public override string RedirectLocation { get; set; }

        public override void Write(string s)
        {
            _outputString.Append(s);
        }

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }
    }
}