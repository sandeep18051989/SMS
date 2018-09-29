using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace EF.Services.Http
{
    public class AutoHttpContext : HttpContextBase
    {
        private readonly HttpCookieCollection _cookies;
        private readonly NameValueCollection _formParams;
        private IPrincipal _principal;
        private readonly NameValueCollection _queryStringParams;
        private readonly string _relativeUrl;
        private readonly string _method;
        private readonly SessionStateItemCollection _sessionItems;
        private readonly NameValueCollection _serverVariables;
        private HttpResponseBase _response;
        private HttpRequestBase _request;
        private readonly Dictionary<object, object> _items;

        public static AutoHttpContext Root()
        {
            return new AutoHttpContext("~/");
        }

        public AutoHttpContext(string relativeUrl, string method)
            : this(relativeUrl, method, null, null, null, null, null, null)
        {
        }

        public AutoHttpContext(string relativeUrl) 
            : this(relativeUrl, null, null, null, null, null, null)
        {
        }

        public AutoHttpContext(string relativeUrl, IPrincipal principal, NameValueCollection formParams,
                               NameValueCollection queryStringParams, HttpCookieCollection cookies,
                               SessionStateItemCollection sessionItems, NameValueCollection serverVariables) 
            : this(relativeUrl, null, principal, formParams, queryStringParams, cookies, sessionItems, serverVariables)
        {
        }

        public AutoHttpContext(string relativeUrl, string method, IPrincipal principal, NameValueCollection formParams,
                               NameValueCollection queryStringParams, HttpCookieCollection cookies,
                               SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
        {
            _relativeUrl = relativeUrl;
            _method = method;
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;
            _serverVariables = serverVariables;

            _items = new Dictionary<object, object>();
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _request ??
                       new AutoHttpRequest(_relativeUrl, _method, _formParams, _queryStringParams, _cookies, _serverVariables);
            }
        }

        public void SetRequest(HttpRequestBase request)
        {
            _request = request;
        }

        public override HttpResponseBase Response
        {
            get
            {
                return _response ?? new AutoHttpResponse();
            }
        }

        public void SetResponse(HttpResponseBase response)
        {
            _response = response;
        }

        public override IPrincipal User
        {
            get { return _principal; }
            set { _principal = value; }
        }

        public override HttpSessionStateBase Session
        {
            get { return new AutoHttpSessionState(_sessionItems); }
        }

        public override System.Collections.IDictionary Items
        {
            get
            {
                return _items;
            }
        }


        public override bool SkipAuthorization { get; set; }

        public override object GetService(Type serviceType)
        {
            return null;
        }
    }
}