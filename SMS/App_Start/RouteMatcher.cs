using System;
using System.Web;
using System.Web.Routing;

namespace SMS
{
    public class RouteMatcher : IRouteConstraint
    {
        private readonly bool _allowEmpty;

        public RouteMatcher(bool allowEmpty)
        {
            this._allowEmpty = allowEmpty;
        }
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                string stringValue = values[parameterName] != null ? values[parameterName].ToString() : null;

                if (!string.IsNullOrEmpty(stringValue))
                {

					return Guid.TryParse(stringValue, out Guid guidValue) &&
						 (_allowEmpty || guidValue != Guid.Empty);
				}
            }

            return false;
        }
    }
}
